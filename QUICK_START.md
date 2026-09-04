# 🎉 Sibiu Events API - Documentație Completă

## 📋 Ceea ce am realizat

Ți-am creat o **API REST completă și profesională** pentru managementul evenimentelor din Sibiu, cu următoarele caracteristici:

### ✅ Caracteristici Implementate

| Caracteristică | Status | Detalii |
|---|---|---|
| **Swagger/OpenAPI** | ✅ | Documentație interactivă la `http://localhost:5155` |
| **4-5 Clase Principale** | ✅ | User, Event, EventRegistration, + DTOs |
| **SQLite Configurare** | ✅ | Bază de date SQLite cu EF Core |
| **DbContext** | ✅ | SibiuEventsContext cu configurare relaționări |
| **Controller Layer** | ✅ | 3 Controllers cu 10+ endpoints |
| **Service Layer** | ✅ | EventService și UserService cu logică business |
| **Repository Layer** | ✅ | IUserRepository, IEventRepository, IEventRegistrationRepository |
| **Admin/User Roles** | ✅ | Role-based access control complet |
| **Admin Funcționalități** | ✅ | Creare, modificare, ștergere evenimente |
| **User Funcționalități** | ✅ | Vizualizare și înscriiere la evenimente |
| **Validări** | ✅ | Validări de business logic |
| **Seed Data** | ✅ | 2 utilizatori + 4 evenimente preconfigurare |

---

## 🚀 Cum să Pornești

### 1. Instalare și Setup
```bash
cd /Users/eduard/Documents/GitHub/medii-si-platforme-de-dezvoltare-avansate-events

# Restaurare pachete
dotnet restore

# Creare bază de date
dotnet ef database update

# Pornire server
dotnet run
```

### 2. Acces API
- **Swagger UI**: http://localhost:5155
- **API Base URL**: http://localhost:5155/api

---

## 📁 Structura Proiectului

```
📦 medii-si-platforme-de-dezvoltare-avansate-events/
│
├── 📂 Controllers/
│   ├── EventsController.cs          # HTTP endpoints pentru evenimente
│   ├── UsersController.cs           # HTTP endpoints pentru utilizatori
│   └── RegistrationsController.cs   # HTTP endpoints pentru înscrierii
│
├── 📂 Services/
│   ├── IServices.cs                 # Interfețe servicii
│   └── Services.cs                  # EventService și UserService
│
├── 📂 Repositories/
│   ├── IRepositories.cs             # Interfețe repository
│   └── Repositories.cs              # Implementări repository
│
├── 📂 Models/
│   ├── User.cs                      # Model utilizator
│   ├── Event.cs                     # Model eveniment
│   ├── EventRegistration.cs         # Model înscriiere
│   └── DTOs.cs                      # Data Transfer Objects
│
├── 📂 Data/
│   └── SibiuEventsContext.cs        # DbContext și seed data
│
├── 📂 Migrations/
│   └── 20260904050621_InitialCreate.cs  # Migrare inițială
│
├── 📄 Program.cs                    # Configurare aplicație
├── 📄 appsettings.json              # Setări conexiune SQLite
├── 📄 README.md                     # Documentație detaliată
├── 📄 TESTING.md                    # Exemple de testare
├── 📄 sibiu-events-api.http         # Fișier REST pentru VS Code
└── 📄 SibiuEvents.db               # Baza de date SQLite
```

---

## 📊 Modelele de Date

### User
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }      // Admin sau User
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
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
    public string Location { get; set; }
    public int AvailableSpots => MaxParticipants - Registrations.Count;
    public bool IsRegistrationOpen => DateTime.UtcNow < RegistrationDeadline;
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
```

---

## 🔑 API Endpoints

### 🎯 Events (`/api/events`)

| Metodă | Endpoint | Descriere | Rol |
|--------|----------|-----------|-----|
| GET | `/api/events` | Toate evenimentele | Public |
| GET | `/api/events/active` | Doar active | Public |
| GET | `/api/events/{id}` | Event specific | Public |
| POST | `/api/events` | Creare event | Admin |
| PUT | `/api/events/{id}` | Modificare event | Admin |
| DELETE | `/api/events/{id}` | Ștergere event | Admin |
| GET | `/api/events/{id}/participants` | Participanți | Admin |

### 👤 Users (`/api/users`)

| Metodă | Endpoint | Descriere | Rol |
|--------|----------|-----------|-----|
| GET | `/api/users` | Toți utilizatorii | Public |
| GET | `/api/users/{id}` | User specific | Public |
| POST | `/api/users` | Creare user | Public |

### 📝 Registrations (`/api/registrations`)

| Metodă | Endpoint | Descriere | Rol |
|--------|----------|-----------|-----|
| POST | `/api/registrations/join` | Înscriiere | User |
| POST | `/api/registrations/cancel` | Anulare înscriiere | User |
| GET | `/api/registrations/user/{userId}` | Înscriieri utilizator | Public |

---

## 💡 Exemple de Utilizare

### 1. Obținere Evenimente
```bash
curl http://localhost:5155/api/events | jq .
```

### 2. Creare Event (Admin)
```bash
curl -X POST "http://localhost:5155/api/events?adminUserId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Workshop: Docker",
    "description": "Învață Docker",
    "startDate": "2026-11-15T18:00:00Z",
    "registrationDeadline": "2026-11-10T23:59:59Z",
    "maxParticipants": 30,
    "location": "Sibiu"
  }'
```

### 3. Înscriiere la Event (User)
```bash
curl -X POST "http://localhost:5155/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 1}'
```

### 4. Obținere Înscrierii (User)
```bash
curl http://localhost:5155/api/registrations/user/2 | jq .
```

---

## 📚 Fișiere Documentație

| Fișier | Conținut |
|--------|----------|
| **README.md** | Documentație detaliată, endpoints, exemple |
| **TESTING.md** | Scenarii complete de testare cu curl |
| **sibiu-events-api.http** | Fișier REST pentru testare din VS Code |

---

## 🧪 Testare

### Metoda 1: Curl
```bash
# Toate evenimentele
curl http://localhost:5155/api/events | jq .

# Înscriiere la event
curl -X POST "http://localhost:5155/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 3}'
```

### Metoda 2: Swagger UI
1. Mergi la http://localhost:5155
2. Fiecare endpoint are butoane "Try it out"
3. Completezi parametri și teste direct din interfață

### Metoda 3: VS Code REST Client
1. Deschide fișierul `sibiu-events-api.http`
2. Click pe "Send Request" pentru fiecare secțiune
3. Vezi răspunsul în panoul de output

---

## 🔐 Roller și Permisiuni

### Admin (ID: 1)
- Email: admin@sibiuevents.com
- ✅ Crează, modifică, șterge evenimente
- ✅ Vede lista de participanți
- ✅ Vede toți utilizatorii

### User (ID: 2)
- Email: user@sibiuevents.com
- ✅ Vede listă de evenimente
- ✅ Se înscrie la evenimente
- ✅ Se retrage de la înscrierii
- ✅ Vede propriile înscrierii

---

## 📈 Seed Data

Baza de date se inițializează cu date de test:

### Utilizatori
- **Admin**: admin@sibiuevents.com
- **User**: user@sibiuevents.com

### Evenimente
1. **Workshop: ASP.NET Core Avançat** - 30 locuri
2. **Conferință: Cloud Computing în 2025** - 50 locuri
3. **Hackathon: Sibiu Tech Challenge** - 20 locuri
4. **Meetup: C# Developers** - 40 locuri

---

## ⚙️ Stack Tehnologic

| Tehnologie | Versiune | Rol |
|-----------|----------|-----|
| .NET | 8.0 | Framework |
| ASP.NET Core | 8.0 | Web framework |
| Entity Framework Core | 8.0.0 | ORM |
| SQLite | Implicit | Bază de date |
| Swagger/OpenAPI | 6.4.0 | Documentație API |
| C# | 12 | Limbaj |

---

## 🎓 Arhitectură Detalii

### Pattern-uri Utilizate

1. **Repository Pattern**
   - IUserRepository, IEventRepository, IEventRegistrationRepository
   - Decuplare logică business de data access

2. **Service Pattern**
   - EventService, UserService
   - Centralizare logică business
   - Validări și transformări

3. **Dependency Injection**
   - Configurare în Program.cs
   - Scoped lifecycle pentru repositories și services

4. **DTO Pattern**
   - CreateEventRequest, UpdateEventRequest
   - EventDto, UserDto, EventRegistrationDto
   - ApiResponse<T> wrapper

---

## 🐛 Troubleshooting

| Problemă | Soluție |
|----------|---------|
| Connection refused | Verifică `dotnet run` și portul 5155 |
| Database locked | Restart serverul |
| Event not found | Verifică ID-ul cu `curl http://localhost:5155/api/events` |
| Unauthorized error | Verifică `adminUserId` query parameter |
| ValidationError | Verifică RequestBody format (JSON) |

---

## 📞 Suport

Toate endpoint-urile sunt documentate în:
1. **Swagger UI** - http://localhost:5155
2. **README.md** - Documentație completă
3. **TESTING.md** - Exemple practice

---

## 🎯 Următorii Pași (Opțional)

1. **Autentificare JWT** - Adaugă token-based auth
2. **Database Production** - Migrare pe PostgreSQL
3. **Logging** - Adaugă Serilog
4. **Testing** - Unit tests cu xUnit
5. **Frontend** - React/Vue client
6. **Deploy** - Azure/AWS

---

**Versiune**: 1.0  
**Data**: 4 septembrie 2026  
**Status**: ✅ Funcțional și gata de utilizare
