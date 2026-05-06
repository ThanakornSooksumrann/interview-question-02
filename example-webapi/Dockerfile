# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /src

COPY Examlple.sln ./
COPY Example.API/Example.API.csproj ./Example.API/
RUN dotnet restore

COPY . .
RUN dotnet publish Example.API/Example.API.csproj -c Release -o /app/publish --no-restore

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runner

WORKDIR /app

COPY --from=builder /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Example.API.dll"]
