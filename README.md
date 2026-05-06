# interview-question-02

Full-stack authentication system — Angular 19 + .NET 9 Web API + PostgreSQL

---

# Docker

## Services

| Service | Image | Port |
|---------|-------|------|
| webapp | node:20-alpine (serve) | http://localhost:4000 |
| webapi | dotnet/aspnet:9.0 | http://localhost:8080 |
| db | postgres:17-alpine | localhost:5432 |

## Run all services
```
docker compose up --build
```

## Run in background
```
docker compose up --build -d
```

## Stop all services
```
docker compose down
```

## Stop and remove volumes (clear DB)
```
docker compose down -v
```

## Build specific service
```
docker compose build webapp
docker compose build webapi
```

## View logs
```
docker compose logs -f
docker compose logs -f webapi
```

---

## Generate JWT Secret
```
openssl rand -base64 64
```

## Generate Password
```
openssl rand -base64 16
```
