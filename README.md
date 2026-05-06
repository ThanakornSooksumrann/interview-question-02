# interview-question-02

ระบบ Authentication แบบ Full-stack

---

# Tech Stack

## Frontend
| Technology | Version |
|------------|---------|
| Angular | 19.2 |
| PrimeNG | 19.1 |
| PrimeFlex | 4.0 |
| PrimeIcons | 7.0 |
| Node.js (runtime) | 20 LTS |

## Backend
| Technology | Version |
|------------|---------|
| .NET | 9.0 |
| ASP.NET Core Web API | 9.0 |
| Entity Framework Core | 9.0.7 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 9.0.4 |
| JWT Bearer Authentication | 9.0.7 |
| BCrypt.Net-Next | 4.1.0 |
| Swashbuckle.AspNetCore (Swagger) | 6.9.0 |
| Newtonsoft.Json | 13.0.4 |
| log4net | 3.3.1 |

## Database
| Technology | Version |
|------------|---------|
| PostgreSQL | 17 |

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
