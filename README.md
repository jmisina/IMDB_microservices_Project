# IMDB E-Shop - System Mikroserwisowy

IMDB E-Shop to nowoczesna platforma e-commerce oparta na architekturze mikroserwisów. Projekt demonstruje wykorzystanie nowoczesnych technologii .NET oraz React w budowie skalowalnych systemów rozproszonych.

## Spis treści
- [Opis Projektu](#opis-projektu)
- [Architektura i Technologie](#architektura-i-technologie)
- [Struktura Projektu](#struktura-projektu)
- [Zmienne Środowiskowe](#zmienne-środowiskowe)
- [Uruchomienie Projektu](#uruchomienie-projektu)
- [API i Bramka (Gateway)](#api-i-bramka-gateway)

---

## Opis Projektu

Projekt to kompletny system sklepu internetowego, w skład którego wchodzą usługi zarządzania użytkownikami, katalogiem produktów oraz zamówieniami. System wykorzystuje bramkę API (API Gateway) do centralizacji ruchu i routingu do odpowiednich usług.

## Architektura i Technologie

### Backend
- **.NET 8 / ASP.NET Core**: Główna platforma serwerowa.
- **YARP (Yet Another Reverse Proxy)**: Pełni rolę API Gateway.
- **Entity Framework Core**: Mapowanie obiektowo-relacyjne (PostgreSQL).
- **Marten**: Wykorzystywany w serwisie produktów jako baza dokumentowa.
- **Carter**: Biblioteka do eleganckiego definiowania Endpointów w Minimal API.
- **CQRS (Command Query Responsibility Segregation)**: Wzorzec stosowany do separacji operacji zapisu i odczytu.
- **PostgreSQL**: Główna baza danych.

### Frontend
- **React 19**: Biblioteka UI.
- **Vite**: Narzędzie do budowania i serwowania aplikacji frontendowej.
- **Tailwind CSS 4**: Stylowanie aplikacji.
- **Zustand**: Zarządzanie stanem.
- **React Query (TanStack Query)**: Zarządzanie stanem asynchronicznym i zapytaniami API.
- **React Router 7**: Routing wewnątrz aplikacji.

---

## Struktura Projektu

- `src/APIGateways/YARPAPIGateway`: Bramka wejściowa do systemu.
- `src/Services/UsersAPI`: Zarządzanie użytkownikami, profilami i autoryzacją (JWT, Google OAuth).
- `src/Services/ProductsAPI`: Zarządzanie katalogiem produktów.
- `src/Services/OrdersAPI`: Obsługa procesów zamówień.
- `src/BuildingBlocks`: Współdzielony kod i interfejsy (np. CQRS).
- `src/Web/frontend-react`: Aplikacja kliencka.

---

## Zmienne Środowiskowe

### Backend (Mikroserwisy)
Mikroserwisy wykorzystują plik konfiguracyjny `.env` znajdujący się w `src/Services/UsersAPI/.env`.

Kluczowe zmienne:
- `Jwt__Secret`: Klucz do podpisywania tokenów JWT.
- `Jwt__Issuer`: Wydawca tokena.
- `Jwt__Audience`: Odbiorca tokena.
- `DB_PASSWORD`: Hasło do bazy danych PostgreSQL.
- `ConnectionStrings__Database`: Connection string do bazy danych.

### API Gateway (YARP)
Bramka wymaga zdefiniowania adresów URL usług (np. w pliku `.env` lub przez Docker Compose):
- `PRODUCTS_API_URL`: URL usługi produktów.
- `USERS_API_URL`: URL usługi użytkowników.
- `ORDERS_API_URL`: URL usługi zamówień.

### Frontend (React)
W pliku `src/Web/frontend-react/.env`:
- `VITE_API_URL`: URL bramki API.
- `VITE_GOOGLE_CLIENT_ID`: ID klienta Google OAuth dla logowania zewnętrznego.

---

## Uruchomienie Projektu

### Wymagania
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/)

### Krok 1: Uruchomienie infrastruktury i usług (Docker)
W katalogu `src`:
```bash
docker-compose up -d --build
```

### Krok 2: Konfiguracja i uruchomienie Frontendu
W katalogu `src/Web/frontend-react`:
1. Zainstaluj zależności:
   ```bash
   npm install
   ```
2. Uruchom aplikację:
   ```bash
   npm run dev
   ```

---

## API i Bramka (Gateway)

System jest dostępny pod adresem bramki YARP. Routing odbywa się według prefiksów:
- `/catalog-service/*` -> ProductsAPI
- `/user-service/*` -> UsersAPI
- `/orders-service/*` -> OrdersAPI

