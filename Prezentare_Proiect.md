# 📋 Prezentare Proiect SibiuEvents

## Ce face proiectul?
**SibiuEvents** e o aplicație ASP.NET Core Web API pentru gestionarea evenimentelor. Permite:
- Creare și gestionare de evenimente (admin-only)
- Înscrierea utilizatorilor la evenimente
- Anularea înscrierii
- Vizualizarea participanților și disponibilității locurilor

Stack: **ASP.NET Core 8 + EF Core + SQLite**

---

## 🏗️ Arhitectura

```
Controllers → Services → Repositories → EF Core → SQLite
                ↓
          Validation Rules (Strategy)
          Registration Observers (Observer)
```

**Straturi:**
- **Controllers** — HTTP endpoints
- **Services** — logica de business + validări
- **Repositories** — acces la date
- **Models/Entities** — Event, User, EventRegistration
- **DTOs** — contract API

---

## 🎯 Design Patterns Utilizați

### 1. **Strategy Pattern** (Reguli de Validare)
**Cum funcționează:**
```csharp
// Fiecare regulă e o strategie independentă
IEventValidationRule: DeadlineBeforeStartRule, PositiveMaxParticipantsRule
IRegistrationEligibilityRule: RegistrationWindowOpenRule, NotAlreadyRegisteredRule
```

**De ce?** Orice regulă nouă se adaugă prin DI, fără a modifica EventService (Open/Closed Principle). Vor fi testate independent.

### 2. **Observer Pattern** (Notificări)
**Cum funcționează:**
```csharp
IRegistrationObserver: LoggingRegistrationObserver, EventCapacityObserver
// Se apelează când utilizator se înscrie/anulează
```

**De ce?** Observatori independenți pot reacționa la schimbări (logs, email, SMS, etc.) fără a modifica EventService.

---

## ⚙️ CI/CD Running

**GitHub Actions (.github/workflows/ci.yml):**
- Trigger: `push` la main/develop sau `pull_request`
- Steps:
  1. Checkout code
  2. Setup .NET 8
  3. `dotnet restore SibiuEvents.sln`
  4. `dotnet build SibiuEvents.sln --configuration Release`
  5. `dotnet test SibiuEvents.sln --configuration Release`
  6. Upload test results

**Status:** ✅ Build succeeds, 9/9 tests passing

---

## 🧪 Teste Local

### Rulare Teste
```bash
# Rulează toate testele
dotnet test SibiuEvents.sln --verbosity normal

# Doar test project
dotnet test Tests/SibiuEvents.Tests.csproj
```

### Fișierele de Test
- `Tests/Rules/DeadlineBeforeStartRuleTests.cs` — Strategy rules validation (3 test cases)
- `Tests/Rules/RegistrationWindowOpenRuleTests.cs` — Registration eligibility (3 test cases)
- `Tests/Services/EventServiceRegistrationTests.cs` — Integration tests cu fake repos (3 test cases)

**Total: 9 teste, fără dependencies de mocking**

---

## 🚀 Rulare Aplicație (Swagger)

### Start API
```bash
dotnet run
```

### Acces Swagger UI
- URL: `http://localhost:5000`
- Swagger Docs: `http://localhost:5000/swagger/v1/swagger.json`

### Endpoints Principali
```
POST   /api/events              — Creare eveniment (admin)
GET    /api/events              — Toate evenimentele
GET    /api/events/{id}         — Detalii eveniment
POST   /api/events/{id}/join    — Înscriere utilizator
POST   /api/events/{id}/cancel  — Anulare înscriere (admin)
GET    /api/events/{id}/participants — Lista participanți (admin)
```

### Test în Swagger
1. Mergi la `POST /api/events`
2. Încearcă creare cu deadline **după** start date → error din `DeadlineBeforeStartRule` ✓
3. Mergi la `POST /api/events/{id}/join`
4. Înscrie-te → log din `LoggingRegistrationObserver` în consolă ✓

---

## 🔨 Build & Compilare

### Build Soluție Completă
```bash
# Debug
dotnet build SibiuEvents.sln

# Release
dotnet build SibiuEvents.sln --configuration Release
```

### Build Proiect Principal
```bash
dotnet build medii-si-platforme-de-dezvoltare-avansate-events.csproj
```

### Build Proiect Teste
```bash
dotnet build Tests/SibiuEvents.Tests.csproj
```

### Clean Build
```bash
dotnet clean SibiuEvents.sln
dotnet build SibiuEvents.sln
```

**Result:** ✅ 0 errors, 2 warnings (nullability, non-blocking)

---

## 📊 Structură Fișiere Relevante

```
├── Services/
│   ├── Rules/                          # Strategy Pattern
│   │   ├── IEventValidationRule.cs
│   │   ├── IRegistrationEligibilityRule.cs
│   │   ├── DeadlineBeforeStartRule.cs
│   │   ├── PositiveMaxParticipantsRule.cs
│   │   ├── RegistrationWindowOpenRule.cs
│   │   ├── NotAlreadyRegisteredRule.cs
│   │   └── ValidationResult.cs
│   ├── Notifications/                  # Observer Pattern
│   │   ├── IRegistrationObserver.cs
│   │   ├── LoggingRegistrationObserver.cs
│   │   └── EventCapacityObserver.cs
│   └── EventService.cs                 # Injector de reguli și observatori
├── Tests/                              # xUnit Project
│   ├── Rules/
│   ├── Services/
│   └── SibiuEvents.Tests.csproj
├── SibiuEvents.sln
├── Program.cs                          # DI wiring
└── .github/workflows/ci.yml
```

---

## 📝 Comandă Rapidă Complete

```bash
# Build + Test + Run
dotnet build SibiuEvents.sln && dotnet test SibiuEvents.sln && dotnet run

# sau individual
dotnet build                                          # Build
dotnet test                                           # Test
dotnet run                                            # Run Swagger at localhost:5000
```

---

## ✅ Cerințe Îndeplinite

| Cerință | Status | Detaliu |
|---------|--------|---------|
| 2 Design Patterns GoF | ✅ | Strategy + Observer, ambele integrate în logica de business |
| 2-3 Unit Tests | ✅ | 9 teste total, toate passing |
| OOP Principles | ✅ | Encapsulation, Inheritance, Polymorphism, DI |
| Architecture solid | ✅ | Layers: Controllers → Services → Repositories → Data |
| CI Pipeline | ✅ | GitHub Actions, build + test automat |
| Funcționalitate minimală | ✅ | CRUD evenimente + înscrierea utilizatorilor |

---

**Prezentat de:** Eduard Precup | **Timp:** ~5 minute
