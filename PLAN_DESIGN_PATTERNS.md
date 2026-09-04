# Plan: Design patterns + OOP + unit tests pentru SibiuEvents

## Context

Proiectul SibiuEvents (ASP.NET Core 8 / EF Core / SQLite) e o aplicație CRUD simplă pentru evenimente
(`Event`, `User`, `EventRegistration`). Cerința temei este să integrăm minim 2 design patterns GoF
(non-arhitecturale, non-Singleton) care au sens în logica de business, plus 2-3 unit teste, respectând
principii OOP/SOLID. Fără a adăuga funcționalitate nouă inutilă — folosim exact regulile de business
care există deja, doar le reorganizăm corect.

Codul actual (`Services/EventService.cs`) are toată logica de validare scrisă inline cu `if`-uri:
verificare deadline < start date, max participanți > 0, verificare rol admin, verificare înscriere
duplicată, verificare fereastră de înscriere deschisă. Asta încalcă Open/Closed Principle — orice
regulă nouă înseamnă modificarea directă a metodei.

## Cele 2 design patterns alese

### 1. Strategy — reguli de business ca obiecte interschimbabile

Extragem regulile de validare din `CreateEventAsync` (linii 118-134) și `JoinEventAsync` (linia 318)
din `EventService.cs` în clase separate, implementând interfețe comune, injectate prin DI ca listă.

- `Services/Rules/IEventValidationRule.cs` — `ValidationResult Validate(Event eventObj)`
  - `DeadlineBeforeStartRule` — deadline-ul de înscriere trebuie să fie înainte de `StartDate`
  - `PositiveMaxParticipantsRule` — `MaxParticipants > 0`
- `Services/Rules/IRegistrationEligibilityRule.cs` — `ValidationResult Validate(Event eventObj, bool alreadyRegistered)`
  - `RegistrationWindowOpenRule` — folosește `Event.IsRegistrationOpen`
  - `NotAlreadyRegisteredRule` — respinge dublă înscriere

`EventService` primește `IEnumerable<IEventValidationRule>` și `IEnumerable<IRegistrationEligibilityRule>`
prin constructor și le rulează într-un `foreach`, oprindu-se la prima eroare. Regulile noi se adaugă
doar prin DI, fără a atinge `EventService` (Open/Closed Principle).

`ValidationResult` = un mic record `(bool IsValid, string? ErrorMessage)` în `Services/Rules/ValidationResult.cs`.

### 2. Observer — notificare la înscriere/anulare

`EventService` devine "subiect" care anunță observatori independenți când un utilizator se
înscrie sau anulează, fără să știe ce fac aceștia (extensibil ulterior spre email/SMS, fără a
modifica `EventService`).

- `Services/Notifications/IRegistrationObserver.cs`
  - `void OnRegistrationCreated(EventRegistration registration, Event eventObj)`
  - `void OnRegistrationCancelled(EventRegistration registration, Event eventObj)`
- `Services/Notifications/LoggingRegistrationObserver.cs` — loghează via `ILogger<T>` (simulează o notificare)
- `Services/Notifications/EventCapacityObserver.cs` — loghează separat un mesaj de avertizare când
  evenimentul devine plin (`AvailableSpots == 0`) după înscriere — a doua implementare concretă,
  demonstrează polimorfismul real al pattern-ului

`EventService` primește `IEnumerable<IRegistrationObserver>` prin constructor, le apelează în
`JoinEventAsync` (după `CreateRegistrationAsync`) și `CancelRegistrationAsync` (după anulare).

## Modificări în `EventService.cs`

- Constructor: adaugă 3 parametri noi (`IEnumerable<IEventValidationRule>`, `IEnumerable<IRegistrationEligibilityRule>`, `IEnumerable<IRegistrationObserver>`)
- `CreateEventAsync`: înlocuiește cele 2 blocuri `if` (deadline, max participanți) cu iterare prin `_eventValidationRules`
- `JoinEventAsync`: înlocuiește verificarea `!eventObj.IsRegistrationOpen` și verificarea de dublă înscriere cu iterare prin `_registrationEligibilityRules`; după succes, apelează observatorii
- `CancelRegistrationAsync`: după anulare, apelează observatorii

Verificările de autorizare (rol admin, creator eveniment) **rămân neschimbate** — sunt autorizare,
nu reguli de business validabile independent, și nu trebuie forțate în pattern doar ca să bifăm o cerință.

## `Program.cs` — înregistrare DI

```csharp
builder.Services.AddScoped<IEventValidationRule, DeadlineBeforeStartRule>();
builder.Services.AddScoped<IEventValidationRule, PositiveMaxParticipantsRule>();
builder.Services.AddScoped<IRegistrationEligibilityRule, RegistrationWindowOpenRule>();
builder.Services.AddScoped<IRegistrationEligibilityRule, NotAlreadyRegisteredRule>();
builder.Services.AddScoped<IRegistrationObserver, LoggingRegistrationObserver>();
builder.Services.AddScoped<IRegistrationObserver, EventCapacityObserver>();
```
(ASP.NET Core suportă nativ înregistrarea mai multor implementări pentru aceeași interfață, injectate ca `IEnumerable<T>`.)

## Proiect de teste (nou)

Nu există niciun proiect de teste în repo. Se creează:

- `SibiuEvents.Tests/SibiuEvents.Tests.csproj` — xUnit + `Microsoft.NET.Test.Sdk` + `xunit.runner.visualstudio`, cu `<ProjectReference>` către proiectul principal
- `SibiuEvents.sln` la rădăcină, incluzând ambele proiecte (necesar pentru `dotnet build`/`dotnet test` uniform și pentru pipeline-ul CI)

### 3 unit teste (fără DB, teste unitare pure)

1. `SibiuEvents.Tests/Rules/DeadlineBeforeStartRuleTests.cs` — deadline după/egal cu start date → invalid; deadline înainte → valid
2. `SibiuEvents.Tests/Rules/RegistrationWindowOpenRuleTests.cs` — eveniment cu deadline trecut sau fără locuri libere → invalid
3. `SibiuEvents.Tests/Services/EventServiceRegistrationTests.cs` — test de integrare la nivel de unitate pentru `EventService.JoinEventAsync`: cu repository-uri fake (implementări simple în-memory ale `IEventRepository`/`IUserRepository`/`IEventRegistrationRepository`, nu Moq — păstrăm simplu, fără dependențe noi de mocking) + regulile reale + un `IRegistrationObserver` spy, verifică că înscrierea reușește și observatorul e notificat exact o dată

Repository-urile fake sunt clase mici, ad-hoc, definite direct în fișierul de test (implementează
interfețele existente din `Repositories/`), suficiente cât să nu depindem de EF Core/SQLite în teste.

## Bonus — pipeline CI

`.github/workflows/ci.yml`:
```yaml
name: CI
on: [push, pull_request]
jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - run: dotnet restore
      - run: dotnet build --no-restore
      - run: dotnet test --no-build
```

## Fișiere noi
- `Services/Rules/ValidationResult.cs`, `IEventValidationRule.cs`, `DeadlineBeforeStartRule.cs`, `PositiveMaxParticipantsRule.cs`
- `Services/Rules/IRegistrationEligibilityRule.cs`, `RegistrationWindowOpenRule.cs`, `NotAlreadyRegisteredRule.cs`
- `Services/Notifications/IRegistrationObserver.cs`, `LoggingRegistrationObserver.cs`, `EventCapacityObserver.cs`
- `SibiuEvents.Tests/SibiuEvents.Tests.csproj` + cele 3 fișiere de test
- `SibiuEvents.sln`
- `.github/workflows/ci.yml`

## Fișiere modificate
- `Services/EventService.cs` (constructor + 3 metode)
- `Program.cs` (înregistrări DI)

## Verificare
- `dotnet build` la rădăcină — trebuie să compileze fără erori
- `dotnet test` — cele 3 teste trebuie să treacă
- Pornire API (`dotnet run`) și test manual prin Swagger: creare eveniment cu deadline invalid → mesaj de eroare din regulă; înscriere la eveniment → verificare log în consolă de la `LoggingRegistrationObserver`
