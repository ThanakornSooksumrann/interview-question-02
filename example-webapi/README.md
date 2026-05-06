
# Run Commands

## Run (Development)
```
dotnet run --project Example.API --environment Development
```

## Watch (auto reload code)
```
dotnet watch --project Example.API
```

## Build check error
```
dotnet build Example.API
```

---

# Entity Framework Commands

## Add migration
```
dotnet ef migrations add <MigrationName> --project Example.API
```

## Update database
```
dotnet ef database update --project Example.API
```

## Remove last migration
```
dotnet ef migrations remove --project Example.API
```

## List all migrations
```
dotnet ef migrations list --project Example.API
```

## Generate SQL script
```
dotnet ef migrations script --project Example.API
```

## Scaffold (Database-First)
- สร้าง Entity และ DbContext จาก DB ที่มีอยู่แล้ว
```
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=example_db;Username=example_db_user;Password=VyKUlV/7+cjdy3T0Yeqduw==" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Entities --context-dir Data --context AppDbContext --project Example.API --force
```
