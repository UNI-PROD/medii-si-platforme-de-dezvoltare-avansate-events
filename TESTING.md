## Testare Sibiu Events API

Acest fișier conține comenzi de exemplu pentru testarea API-ului Sibiu Events.

### Variabile de mediu pentru testare
```bash
API_URL="http://localhost:5155"
ADMIN_USER_ID=1
REGULAR_USER_ID=2
```

---

## 1️⃣ EVENTS - Obținere și Creiere

### Obținerea tuturor evenimentelor
```bash
curl -s "$API_URL/api/events" | jq .
```

### Obținerea evenimentelor active
```bash
curl -s "$API_URL/api/events/active" | jq .
```

### Obținerea unui eveniment specific (ID: 1)
```bash
curl -s "$API_URL/api/events/1" | jq .
```

### Crearea unui eveniment nou (de admin)
```bash
curl -X POST "$API_URL/api/events?adminUserId=$ADMIN_USER_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Workshop: Docker & Kubernetes",
    "description": "Învață containerizare și orchestrare cu Docker și Kubernetes",
    "startDate": "2026-10-20T18:00:00Z",
    "registrationDeadline": "2026-10-15T23:59:59Z",
    "maxParticipants": 35,
    "location": "Sibiu - Innovation Hub"
  }' | jq .
```

### Modificarea unui eveniment (ID: 5)
```bash
curl -X PUT "$API_URL/api/events/5?adminUserId=$ADMIN_USER_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Workshop: Docker & Kubernetes - Updated",
    "maxParticipants": 40,
    "location": "Sibiu - Tech Center"
  }' | jq .
```

### Ștergerea unui eveniment (ID: 5)
```bash
curl -X DELETE "$API_URL/api/events/5?adminUserId=$ADMIN_USER_ID" | jq .
```

---

## 2️⃣ USERS - Gestionarea utilizatorilor

### Obținerea tuturor utilizatorilor
```bash
curl -s "$API_URL/api/users" | jq .
```

### Obținerea unui utilizator specific (ID: 2)
```bash
curl -s "$API_URL/api/users/2" | jq .
```

### Crearea unui nou utilizator
```bash
curl -X POST "$API_URL/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "alexandra@sibiuevents.com",
    "fullName": "Alexandra Popescu"
  }' | jq .
```

### Crearea unui alt utilizator
```bash
curl -X POST "$API_URL/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "mihai@sibiuevents.com",
    "fullName": "Mihai Ionescu"
  }' | jq .
```

---

## 3️⃣ REGISTRATIONS - Înscrierea la Evenimente

### Înscrierea la un eveniment (User 2 -> Event 3)
```bash
curl -X POST "$API_URL/api/registrations/join?userId=$REGULAR_USER_ID" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 3}' | jq .
```

### Obținerea tuturor înscrierii unui utilizator (User 2)
```bash
curl -s "$API_URL/api/registrations/user/$REGULAR_USER_ID" | jq .
```

### Anularea unei înscriieri (User 2, Event 3)
```bash
curl -X POST "$API_URL/api/registrations/cancel?userId=$REGULAR_USER_ID&eventId=3" | jq .
```

### Obținerea participanților la un eveniment (Event 1, Admin only)
```bash
curl -s "$API_URL/api/events/1/participants?adminUserId=$ADMIN_USER_ID" | jq .
```

---

## 4️⃣ SCENARII COMPLETE DE TESTARE

### Scenariu 1: User se înscrie la mai multe evenimente
```bash
# User 2 se înscrie la Event 4
curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 4}' | jq .

# User 2 se înscrie la Event 2
curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 2}' | jq .

# Verificare: obtine toate inschierile user 2
curl -s "$API_URL/api/registrations/user/2" | jq '.data | length'
```

### Scenariu 2: Admin creează evento și vede participanții
```bash
# Admin creează eveniment
EVENT_RESPONSE=$(curl -s -X POST "$API_URL/api/events?adminUserId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Training: C# Best Practices",
    "description": "Învață design patterns și best practices în C#",
    "startDate": "2026-11-05T19:00:00Z",
    "registrationDeadline": "2026-11-01T23:59:59Z",
    "maxParticipants": 20,
    "location": "Sibiu - Code Academy"
  }')

# Extrage ID-ul evenimentului nou
NEW_EVENT_ID=$(echo $EVENT_RESPONSE | jq '.data.id')
echo "New Event ID: $NEW_EVENT_ID"

# User 2 se înscrie
curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d "{\"eventId\": $NEW_EVENT_ID}" | jq .

# Admin vede participanții
curl -s "$API_URL/api/events/$NEW_EVENT_ID/participants?adminUserId=1" | jq .
```

### Scenariu 3: Testare validări (erori)
```bash
# Tentativa: Non-admin creează eveniment
curl -X POST "$API_URL/api/events?adminUserId=2" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Event",
    "description": "This should fail",
    "startDate": "2026-10-01T10:00:00Z",
    "registrationDeadline": "2026-10-15T23:59:59Z",
    "maxParticipants": 10,
    "location": "Sibiu"
  }' | jq .

# Tentativa: Înscrierea la evenimentul care nu există
curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 999}' | jq .

# Tentativa: User se înscrie la același eveniment de 2 ori
curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 1}' | jq .

curl -X POST "$API_URL/api/registrations/join?userId=2" \
  -H "Content-Type: application/json" \
  -d '{"eventId": 1}' | jq .  # Ar trebui să dea eroare
```

---

## 5️⃣ TESTE CU JQ FILTERS

### Contarea evenimentelor
```bash
curl -s "$API_URL/api/events" | jq '.data | length'
```

### Obținerea doar a titlurilor evenimentelor
```bash
curl -s "$API_URL/api/events" | jq '.data[].title'
```

### Filtrarea evenimentelor cu mai mult de 30 de locuri
```bash
curl -s "$API_URL/api/events" | jq '.data[] | select(.maxParticipants > 30)'
```

### Obținerea locurilor disponibile pe fiecare eveniment
```bash
curl -s "$API_URL/api/events" | jq '.data[] | {title, availableSpots}'
```

### Verificare: care evenimente sunt în plin (0 locuri disponibile)
```bash
curl -s "$API_URL/api/events" | jq '.data[] | select(.availableSpots == 0)'
```

---

## 6️⃣ TESTARE PERFORMANCE

### Cere toate evenimentele și măsoară tempo
```bash
time curl -s "$API_URL/api/events" > /dev/null
```

### Cere toți utilizatorii
```bash
time curl -s "$API_URL/api/users" > /dev/null
```

---

## 7️⃣ DEBUGGING

### Afișare response headers
```bash
curl -i "$API_URL/api/events/1"
```

### Verificare HTTP status code
```bash
curl -o /dev/null -s -w "%{http_code}\n" "$API_URL/api/events/1"
```

### Verbose mode (affiche request și response)
```bash
curl -v "$API_URL/api/events/1"
```

---

## 📊 FORMATUL RĂSPUNSURILOR

Toate răspunsurile urmează formatul:
```json
{
  "success": true/false,
  "message": "Descriere mesaj",
  "data": { /* Datele cerute */ }
}
```

### Exemplu: Succes
```json
{
  "success": true,
  "message": "Eveniment recuperat cu succes.",
  "data": {
    "id": 1,
    "title": "Workshop: ASP.NET Core Avançat",
    ...
  }
}
```

### Exemplu: Eroare
```json
{
  "success": false,
  "message": "Evenimentul nu a fost găsit.",
  "data": null
}
```

---

## 🔐 NOTIȚE PRIVIND SECURITATEA

- **adminUserId** este trimis ca query parameter (în producție, trebuie trimis din JWT token)
- **userId** trebuie să fie validat din context (în producție, din claims ale token-ului)
- Nu sunt implementate validări de token/autentificare în această versiune
- SQLite este pentru development - pentru producție, folosiți PostgreSQL sau SQL Server

---

## 🐛 TROUBLESHOOTING

**Eroare: Connection refused**
- Verificați că serverul rulează: `dotnet run`
- Verificați portul: default este 5155

**Eroare: Database locked**
- Aplicația poate fi veche și să blocheze DB
- Ștartiți din nou serverul

**Eroare: EventId not found**
- Verificați că ID-ul evenimentului existe: `curl -s http://localhost:5155/api/events | jq '.data[].id'`
