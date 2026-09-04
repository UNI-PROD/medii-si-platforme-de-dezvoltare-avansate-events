# Sibiu Events API

O API REST modernă pentru gestiunea evenimentelor din Sibiu, cu suport pentru roller-uri (Admin/User), înscriieri la evenimente și managementul participanților.

## 🎯 Caracteristici

- **Autentificare pe bază de roller**: Admin și User roles
- **Managementul evenimentelor**: Crearea, modificarea și ștergerea evenimentelor
- **Înscrierea la evenimente**: Utilizatorii se pot înscrie la evenimente cu locuri disponibile
- **Limitare de participanți**: Fiecare eveniment are un număr maxim de participanți
- **Deadline de înscrierie**: Fiecare eveniment are un deadline pentru înscrierii
- **Swagger/OpenAPI documentație**: Interfață interactivă pentru testarea API-ului
- **SQLite bază de date**: Stocarea datelor în SQLite

## 🏗️ Arhitectură

### Straturile aplicației:

1. **Controller Layer** (`Controllers/`)
   - `EventsController.cs` - Managementul evenimentelor
   - `UsersController.cs` - Managementul utilizatorilor
   - `RegistrationsController.cs` - Managementul înscrierii

2. **Service Layer** (`Services/`)
   - `EventService.cs` - Logica de business pentru evenimente
   - `UserService.cs` - Logica de business pentru utilizatori

3. **Repository Layer** (`Repositories/`)
   - `UserRepository.cs` - Accesul la date pentru utilizatori
   - `EventRepository.cs` - Accesul la date pentru evenimente
   - `EventRegistrationRepository.cs` - Accesul la date pentru înscrierii

4. **Data Layer** (`Data/`)
   - `SibiuEventsContext.cs` - DbContext cu configurația bazei de date

5. **Models** (`Models/`)
   - `User.cs` - Modelul utilizatorului
   - `Event.cs` - Modelul evenimentului
   - `EventRegistration.cs` - Modelul înscrierii
   - `DTOs.cs` - Data Transfer Objects

## 📊 Modelele de date

### User
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; } // Admin sau User
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public enum UserRole
{
    User = 0,
    Admin = 1
}
```

### Event
```csharp
public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime RegistrationDeadline { get; set; }
    public int MaxParticipants { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Location { get; set; }
}
```

### EventRegistration
```csharp
public class EventRegistration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public RegistrationStatus Status { get; set; }
}

public enum RegistrationStatus
{
    Registered = 0,
    Cancelled = 1,
    Completed = 2
}
```

## 🚀 Pornirea aplicației

### Prerequisite
- .NET 8.0 SDK
- SQLite (se include cu Entity Framework)

### Comenzi

1. **Instalarea dependențelor**
   ```bash
   dotnet restore
   ```

2. **Crearea bazei de date**
   ```bash
   dotnet ef database update
   ```

3. **Pornirea aplicației**
   ```bash
   dotnet run
   ```

Aplicația va fi disponibilă la:
- **API**: `http://localhost:5155`
- **Swagger UI**: `http://localhost:5155`

## 📚 Endpoints API

### Events Controller (`/api/events`)

#### 1. Obținerea tuturor evenimentelor
```http
GET /api/events
```
**Răspuns**: Lista tuturor evenimentelor cu detalii complete

#### 2. Obținerea evenimentelor active (cu înscrierea deschisă)
```http
GET /api/events/active
```
**Răspuns**: Lista evenimentelor cu deadline încă valabil și locuri disponibile

#### 3. Obținerea unui eveniment după ID
```http
GET /api/events/{id}
```
**Parametri**: 
- `id` (int) - ID-ul evenimentului

**Răspuns**: Detaliile evenimentului

#### 4. Crearea unui eveniment (doar Admin)
```http
POST /api/events?adminUserId={userId}
Content-Type: application/json

{
  "title": "Denumirea evenimentului",
  "description": "Descrierea evenimentului",
  "startDate": "2026-10-01T10:00:00Z",
  "registrationDeadline": "2026-09-25T23:59:59Z",
  "maxParticipants": 50,
  "location": "Sibiu - Locația"
}
```
**Parametri**:
- `adminUserId` (query) - ID-ul utilizatorului admin

**Răspuns**: Evenimentul creat

#### 5. Modificarea unui eveniment (doar Admin creator)
```http
PUT /api/events/{id}?adminUserId={userId}
Content-Type: application/json

{
  "title": "Titlu nou",
  "description": "Descriere nouă",
  "startDate": "2026-10-01T10:00:00Z",
  "registrationDeadline": "2026-09-25T23:59:59Z",
  "maxParticipants": 60,
  "location": "Sibiu - Locația nouă"
}
```

#### 6. Ștergerea unui eveniment (doar Admin)
```http
DELETE /api/events/{id}?adminUserId={userId}
```

#### 7. Obținerea participanților (doar Admin)
```http
GET /api/events/{id}/participants?adminUserId={userId}
```
**Răspuns**: Lista tuturor utilizatorilor înscriși la eveniment

---

### Registrations Controller (`/api/registrations`)

#### 1. Înscrierea unui utilizator la un eveniment
```http
POST /api/registrations/join?userId={userId}
Content-Type: application/json

{
  "eventId": 1
}
```
**Parametri**:
- `userId` (query) - ID-ul utilizatorului care se înscrie

**Răspuns**: Detaliile înscrierii

#### 2. Anularea înscrierii
```http
POST /api/registrations/cancel?userId={userId}&eventId={eventId}
```

#### 3. Obținerea înscrierii utilizatorului
```http
GET /api/registrations/user/{userId}
```
**Răspuns**: Lista tuturor înscrierii utilizatorului la diverse evenimente

---

### Users Controller (`/api/users`)

#### 1. Obținerea tuturor utilizatorilor
```http
GET /api/users
```
**Răspuns**: Lista tuturor utilizatorilor

#### 2. Obținerea unui utilizator după ID
```http
GET /api/users/{id}
```

#### 3. Crearea unui nou utilizator
```http
POST /api/users
Content-Type: application/json

{
  "email": "user@example.com",
  "fullName": "Nume Utilizator"
}
```
**Notă**: Rolul default este "User". Adminul trebuie setat manual.

---

## 🔐 Roller și permisiuni

### Admin
- ✅ Creează, modifică și șterge evenimente
- ✅ Vede lista de participanți la fiecare eveniment
- ✅ Poate vedea toți utilizatorii

### User
- ✅ Vede lista de evenimente disponibile
- ✅ Se înscrie la evenimente (dacă sunt locuri și deadline valabil)
- ✅ Se retrage de la înscriieri
- ✅ Vede propriile înscrierii

---

## 📝 Exemple de utilizare

### 1. Obținere Evenimente
```bash
curl -s http://localhost:5155/api/events | jq .
```

### 2. Crearea unui eveniment (Admin)
```bash
curl -X POST "http://localhost:5155/api/events?adminUserId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Workshop: React 2026",
    "description": "Învață React cu hooks și modern patterns",
    "startDate": "2026-11-15T18:00:00Z",
    "registrationDeadline": "2026-11-10T23:59:59Z",
    "maxParticipants": 25,
    "location": "Sibiu - IT Hub"
  }' | jq .
```

### 3. Înscrierea la un eveniment
```bash
curl -X POST "http://localhost:5155/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 1}' | jq .
```

### 4. Obținerea înscrierii unui utilizator
```bash
curl -s http://localhost:5155/api/registrations/user/2 | jq .
```

### 5. Obținerea participanților la un eveniment (Admin)
```bash
curl -s "http://localhost:5155/api/events/1/participants?adminUserId=1" | jq .
```

---

## 🗄️ Date inițiale (Seed Data)

Baza de date se inițializează cu:

### Utilizatori
- **Admin**: `admin@sibiuevents.com` (ID: 1)
- **User**: `user@sibiuevents.com` (ID: 2)

### Evenimente (4 exemple)
1. **Workshop: ASP.NET Core Avançat** - 30 locuri
2. **Conferință: Cloud Computing în 2025** - 50 locuri
3. **Hackathon: Sibiu Tech Challenge** - 20 locuri
4. **Meetup: C# Developers** - 40 locuri

---

## 🛠️ Stack Tehnologic

- **.NET 8.0** - Framework
- **ASP.NET Core** - Web framework
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Bază de date
- **Swagger/OpenAPI** - Documentație API
- **C# 12** - Limbaj de programare

---

## 📂 Structura proiectului

```
medii-si-platforme-de-dezvoltare-avansate-events/
├── Controllers/
│   ├── EventsController.cs
│   ├── UsersController.cs
│   └── RegistrationsController.cs
├── Services/
│   ├── IServices.cs
│   └── Services.cs
├── Repositories/
│   ├── IRepositories.cs
│   └── Repositories.cs
├── Models/
│   ├── User.cs
│   ├── Event.cs
│   ├── EventRegistration.cs
│   └── DTOs.cs
├── Data/
│   └── SibiuEventsContext.cs
├── Migrations/
│   └── 20260904050621_InitialCreate.cs
├── Program.cs
├── appsettings.json
└── medii-si-platforme-de-dezvoltare-avansate-events.csproj
```

---

## 🐛 Debugging

### Consultarea logurilor
Logurile sunt afișate în consolă în modurile Development. Puteți ajusta nivelul de log în `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  }
}
```

### Resetarea bazei de date
```bash
# Ștergerea bazei de date
rm SibiuEvents.db

# Recreaerea
dotnet ef database update
```

---

## 📄 Licență

Acest proiect este creat pentru scopuri educaționale.

---

## 👨‍💼 Contact

Pentru întrebări sau sugestii, vă rog contactați administratorul.

**Versiunea API**: 1.0
**Data creării**: 4 Septembrie 2026
