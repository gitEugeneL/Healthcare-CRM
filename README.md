# Healthcare-CRM

<!-- Stack -->
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![C%23](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)

<!-- Architecture & Patterns -->
![Clean Architecture](https://img.shields.io/badge/arch-Clean%20Architecture-9cf)
![CQRS](https://img.shields.io/badge/pattern-CQRS-blueviolet)
![MediatR](https://img.shields.io/badge/pattern-MediatR-512BD4)
![Result Pattern](https://img.shields.io/badge/pattern-Result-2E7D32)
![Minimal API](https://img.shields.io/badge/API-MinimalApi-6A1B9A)

<!-- Meta -->
![License](https://img.shields.io/badge/license-MIT-green)
![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)
![GitHub last commit](https://img.shields.io/github/last-commit/gitEugeneL/Healthcare-CRM)
![GitHub repo size](https://img.shields.io/github/repo-size/gitEugeneL/Healthcare-CRM)


A management backend for small clinics, private medical offices, and cosmetic salons.

Covers the full operational loop of a clinic — staff and doctors, patients and offices,
work schedules and appointments, all the way through to the medical records that
close each visit.

Security is built in-house rather than outsourced: rotating JWTs, 
token family tracking, session control, brute-force protection, and account lockout.
The security model is owned, not borrowed.

---

## 👷 Frameworks, Libraries and Technologies

- [.NET](https://github.com/dotnet/core)
- [C#](https://github.com/dotnet/csharplang)
- [FastEndpoints](https://fast-endpoints.com/)
- [xUnit](https://github.com/xunit/xunit)
- [ASP.NET Core](https://github.com/dotnet/aspnetcore)
- [MediatR](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://github.com/dotnet/efcore)
- [PostgreSQL](https://github.com/postgres)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [Docker](https://github.com/docker)
- [Bogus](https://github.com/bchavez/Bogus)


## 🐳 List of Docker Containers

- **app** - ASP.NET Core container for all application layers
- **database** - PostgreSQL database container for persistent data storage
- **redis** - Redis in-memory cache database container
- **redis-commander** - Web-based Redis management interface for cache monitoring and debugging
- **cloudbeaver** - Web-based database management interface for development and debugging

## 🩺 How to run tests

*Allows you to run all integration and unit tests.*

   ```sh
   > dotnet test  # dotnet SKD is required
   ```

## 🚀 How to run the application

***Make*** commands work on Linux/macOS. Alternatively, Docker Compose can be used.


| Action               | Make                                   | Command / Equivalent                                                                                                                       |
|----------------------|----------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| **Start & Build**    | `make up`                              | `docker compose -f docker-compose.yml up -d --build`                                                                                       |
| **Stop**             | `make down`                            | `docker compose -f docker-compose.yml down`                                                                                                |
| **Stop & Clean**     | `make down-and-clean`                  | `docker compose down -v`                                                                                                                   |
| **Create Migration** | `make db-migrate name=<MigrationName>` | `dotnet ef migrations add <Name> --project Infrastructure/Persistence --startup-project Presentation/Api --output-dir Database/Migrations` |
| **Apply Migrations** | `make db-update`                       | `dotnet ef database update --project Infrastructure/Persistence --startup-project Presentation/Api`                                        |



### 🧪 Seed Data (Development Mode)

If the database is empty, the system automatically seeds fake data on startup using Bogus.

| Entity              | Count   | Details                                                                       |
|---------------------|---------|-------------------------------------------------------------------------------|
| **Admin**           | 1       | System administrator account                                                  |
| **Manager**         | 1       | Main clinic manager account                                                   |
| **Doctors**         | 12      | Profiles with education, descriptions, and statuses                           |
| **Patients**        | 25      | Personal profiles with DOB, PESEL, and insurance details                      |
| **Addresses**       | 25      | Patient addresses (province, city, street, house/apartment, zip)              |
| **Work Schedules**  | 12      | Working hours, appointment duration, and workdays (3–6 days/week)             |
| **Offices**         | 7       | Cabinet names and 3-digit room numbers                                        |
| **Specializations** | 5       | Medical specializations (Orthodontics, Surgery, etc.)                         |
| **Appointments**    | ~60–108 | Past & upcoming visits (`Initialized`, `Confirmed`, `Completed`, `Cancelled`) |
| **Medical Records** | ~30+    | Completed appointment logs with ICD-10 codes, diagnoses, and notes            |

### 🔑 Test Credentials (Development Mode)

| Role        | Email / Format            | Password     |
|-------------|---------------------------|--------------|
| **Admin**   | `dev@mail.dev`            | `devDev123!` |
| **Manager** | `manager@mail.dev`        | `devDev123!` |
| **Doctor**  | `doctor-*-{id}@mail.dev`  | `devDev123!` |
| **Patient** | `patient-*-{id}@mail.dev` | `devDev123!` |

## 🔐 Local Access

| Service             | Port | Login  | Password    | URL / GUI             |
|---------------------|------|--------|-------------|-----------------------|
| **PostgreSQL**      | 5440 | `user` | `password`  | `-`                   |
| **Redis**           | 6379 | `-`    | `-`         | `-`                   |
| **Redis Commander** | 8081 | `-`    | `-`         | http://localhost:8081 |
| **CloudBeaver**     | 9000 | `-`    | `-`         | http://localhost:9000 |
| **Backend API**     | 8080 | `-`    | `-`         | http://localhost:8080 |


## 🖨️ Swagger documentation

1. Scalar UI

        http://localhost:8080/scalar

2. [Scalar static file](https://github.com/gitEugeneL/Healthcare-CRM/tree/main/Backend/scalar.json)

        https://github.com/gitEugeneL/Healthcare-CRM/tree/main/Backend/scalar.json

## 💾 Database diagram

![Database diagram](https://github.com/gitEugeneL/Healthcare-CRM/blob/main/diagram.png?raw=true)


## 🔧 Implementation features

------

### Security

*Functionality that allows to authenticate, refresh tokens, log out, and manage account credentials (email, password, email confirmation, code generation)*

#### Login
<details>
<summary>
    <code>POST</code> <code><b>/auth/login</b></code><code>(allows you to login, issues accessToken and sets refreshToken in cookies)</code>
</summary>

##### Body
> | name     | type     | data type |
> |----------|----------|-----------|
> | email    | required | string    |
> | password | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                              |
> |-----------|--------------------|-----------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"accessToken": "eyJhbGciOi...........", "accessTokenExpires": "2024-04-20T19:46:52.893Z", "refreshToken": { ... }}` |
> | `400`     | `application/json` | `array`                                                                                                               |
> | `404`     | `application/json` | `string`                                                                                                              |

##### Set Cookies
> | name         | example                                                              |
> |--------------|----------------------------------------------------------------------|
> | refreshToken | refreshToken=Wna@3da...; Expires=...; Secure; HttpOnly; Domain=...;` |
</details>

#### Refresh
<details>
<summary>
    <code>POST</code> <code><b>/auth/refresh</b></code><code>(allows to refresh access and refresh tokens)</code>
</summary>

##### Body
> | name  | type     | data type |
> |-------|----------|-----------|
> | email | required | string    |

##### Required Cookies
> | name         | example                  |
> |--------------|--------------------------|
> | refreshToken | refreshToken=Wna@3da...; |

##### Responses
> | http code | content-type       | response                                                                                                              |
> |-----------|--------------------|-----------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"accessToken": "eyJhbGciOi...........", "accessTokenExpires": "2024-04-20T19:46:52.893Z", "refreshToken": { ... }}` |
> | `400`     | `application/json` | `array`                                                                                                               |
> | `404`     | `application/json` | `string`                                                                                                              |

##### Set Cookies
> | name         | example                                                              |
> |--------------|----------------------------------------------------------------------|
> | refreshToken | refreshToken=Wna@3da...; Expires=...; Secure; HttpOnly; Domain=...;` |
</details>

#### Logout
<details>
<summary>
    <code>POST</code> <code><b>/auth/logout</b></code><code>(allows to logout, deactivates refresh token and removes HttpOnly cookie)</code>
</summary>

##### Body
> | name  | type     | data type |
> |-------|----------|-----------|
> | email | required | string    |

##### Required Cookies
> | name         | example                  |
> |--------------|--------------------------|
> | refreshToken | refreshToken=Wna@3da...; |

##### Responses
> | http code | content-type                                    | response    |
> |-----------|-------------------------------------------------|-------------|
> | `204`     | `application/json` `and remove HttpOnly Cookie` | `NoContent` |
> | `400`     | `application/json`                              | `string`    |
> | `404`     | `application/json`                              | `string`    |

##### Set Cookies
> | name         | example                                                                              |
> |--------------|--------------------------------------------------------------------------------------|
> | refreshToken | refreshToken=; Expires=Thu, 01 Jan 1970 00:00:00 GMT; Secure; HttpOnly; Domain=...;` |
</details>

#### Change email (*Token required*, 🔒base policy)
<details>
<summary>
    <code>POST</code> <code><b>/auth/change-email</b></code><code>(allows to change email 🔒[base policy])</code>
</summary>

##### Body
> | name     | type     | data type |
> |----------|----------|-----------|
> | newEmail | required | string    |
> | code     | required | string    |

##### Responses
> | http code | content-type       | response |
> |-----------|--------------------|----------|
> | `200`     | `application/json` | -        |
> | `400`     | `application/json` | `array`  |
> | `404`     | `application/json` | `string` |
> | `409`     | `application/json` | `string` |
</details>

#### Change password
<details>
<summary>
    <code>POST</code> <code><b>/auth/change-password</b></code><code>(allows to change password)</code>
</summary>

##### Body
> | name                 | type     | data type |
> |----------------------|----------|-----------|
> | email                | required | string    |
> | code                 | required | string    |
> | password             | required | string    |
> | passwordConfirmation | required | string    |

##### Responses
> | http code | content-type       | response |
> |-----------|--------------------|----------|
> | `200`     | `application/json` | -        |
> | `400`     | `application/json` | `array`  |
> | `404`     | `application/json` | `string` |
</details>

#### Confirm email
<details>
<summary>
    <code>PATCH</code> <code><b>/auth/confirm-email</b></code><code>(allows to confirm email)</code>
</summary>

##### Body
> | name  | type     | data type |
> |-------|----------|-----------|
> | email | required | string    |
> | code  | required | string    |

##### Responses
> | http code | content-type       | response |
> |-----------|--------------------|----------|
> | `200`     | `application/json` | -        |
> | `400`     | `application/json` | `array`  |
> | `404`     | `application/json` | `string` |
</details>

#### Generate code
<details>
<summary>
    <code>POST</code> <code><b>/auth/generate-code</b></code><code>(allows to generate a confirmation code)</code>
</summary>

##### Body
> | name  | type     | data type |
> |-------|----------|-----------|
> | email | required | string    |

##### Responses
> | http code | content-type       | response                                                     |
> |-----------|--------------------|--------------------------------------------------------------|
> | `200`     | `application/json` | `{"email": "string", "expires": "2024-04-20T19:46:52.893Z"}` |
> | `400`     | `application/json` | `array`                                                      |
> | `404`     | `application/json` | `string`                                                     |
</details>

------

### Managers

*Functionality that allows to manage and interact with managers*

#### Create new manager (*Token required*, 🔒admin policy)
<details>
<summary>
    <code>POST</code> <code><b>/managers</b></code><code>(allows to register new manager 🔒[admin policy])</code>
</summary>

##### Body
> | name      | type     | data type |
> |-----------|----------|-----------|
> | email     | required | string    |
> | password  | required | string    |
> | phone     | required | string    |
> | firstName | required | string    |
> | lastName  | required | string    |
> | position  | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                         |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"managerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "position": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                          |
> | `409`     | `application/json` | `string`                                                                                                                                                         |
</details>

#### Get all managers (*Token required*, 🔒admin policy)
<details>
<summary>
    <code>GET</code> <code><b>/managers</b></code><code>(allows to get all managers 🔒[admin policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                           |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `[{"managerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "position": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}]` |
</details>

#### Update manager (*Token required*, 🔒admin policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/managers/{ managerId:uuid }</b></code><code>(allows to update manager 🔒[admin policy])</code>
</summary>

##### Body
> | name      | type         | data type |
> |-----------|--------------|-----------|
> | phone     | not required | string    |
> | position  | not required | string    |
> | firstName | not required | string    |
> | lastName  | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                         |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"managerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "position": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                          |
> | `404`     | `application/json` | `string`                                                                                                                                                         |
</details>

------

### Doctors

*Functionality that allows to manage and interact with doctors*

#### Create new doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/doctors</b></code><code>(allows to create new doctor 🔒[manager policy])</code>
</summary>

##### Body
> | name        | type     | data type |
> |-------------|----------|-----------|
> | email       | required | string    |
> | password    | required | string    |
> | phone       | required | string    |
> | firstName   | required | string    |
> | lastName    | required | string    |
> | education   | required | string    |
> | description | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializations": [{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string"}], "status": "string", "description": "string", "education": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                        |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                       |
</details>

#### Get all doctors (*Token required*, 🔒manager-or-patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/doctors</b></code><code>(allows to get all doctors 🔒[manager-or-patient policy])</code>
</summary>

##### Parameters
> | name             | type         | data type |
> |------------------|--------------|-----------|
> | SpecializationId | not required | uuid      |
> | PageNumber       | not required | int32     |
> | PageSize         | not required | int32     |
> | IsActive         | not required | boolean   |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                        |
> |-----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializations": [{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string"}], "status": "string", "description": "string", "education": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get one doctor (*Token required*, 🔒manager-or-patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/doctors/{ doctorId:uuid }</b></code><code>(allows to get one doctor 🔒[manager-or-patient policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializations": [{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string"}], "status": "string", "description": "string", "education": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                       |
</details>

#### Update doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/doctors/{ doctorId:uuid }</b></code><code>(allows to update doctor 🔒[manager policy])</code>
</summary>

##### Body
> | name        | type         | data type |
> |-------------|--------------|-----------|
> | phone       | not required | string    |
> | firstName   | not required | string    |
> | lastName    | not required | string    |
> | description | not required | string    |
> | education   | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializations": [{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string"}], "status": "string", "description": "string", "education": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                       |
</details>

#### Change doctor status (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/doctor/{ doctorId:uuid }/status</b></code><code>(allows to change doctor status 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializations": [{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string"}], "status": "string", "description": "string", "education": "string", "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                       |
</details>

------

### Patients

*Functionality that allows to manage and interact with patients*

#### Register new patient (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/patients</b></code><code>(allows to register new patient 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name        | type         | data type |
> |-------------|--------------|-----------|
> | email       | required     | string    |
> | password    | required     | string    |
> | phone       | required     | string    |
> | pesel       | required     | string    |
> | dateOfBirth | required     | date      |
> | firstName   | required     | string    |
> | lastName    | required     | string    |
> | insurance   | required     | string    |
> | province    | required     | string    |
> | postalCode  | required     | string    |
> | city        | required     | string    |
> | street      | required     | string    |
> | house       | required     | string    |
> | apartment   | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                             |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "dateOfBirth": "2024-04-20", "insurance": "string", "address": {"province": "string", "postalCode": "string", "city": "string", "street": "string", "house": "string", "apartment": "string"}, "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                                              |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                             |
</details>

#### Get all patients (*Token required*, 🔒manager-or-patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/patients</b></code><code>(allows to get all patients 🔒[manager-or-patient policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | DoctorId   | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "dateOfBirth": "2024-04-20", "insurance": "string", "address": {"province": "string", "postalCode": "string", "city": "string", "street": "string", "house": "string", "apartment": "string"}, "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get one patient (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/patients/{ patientId:uuid }</b></code><code>(allows to get one patient 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                             |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "dateOfBirth": "2024-04-20", "insurance": "string", "address": {"province": "string", "postalCode": "string", "city": "string", "street": "string", "house": "string", "apartment": "string"}, "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                             |
</details>

#### Update patient (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/patients/{ patientId:uuid }</b></code><code>(allows to update patient 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name        | type         | data type |
> |-------------|--------------|-----------|
> | phone       | not required | string    |
> | pesel       | not required | string    |
> | dateOfBirth | not required | date      |
> | firstName   | not required | string    |
> | lastName    | not required | string    |
> | insurance   | not required | string    |
> | province    | not required | string    |
> | postalCode  | not required | string    |
> | city        | not required | string    |
> | street      | not required | string    |
> | house       | not required | string    |
> | apartment   | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                             |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "dateOfBirth": "2024-04-20", "insurance": "string", "address": {"province": "string", "postalCode": "string", "city": "string", "street": "string", "house": "string", "apartment": "string"}, "email": "string", "firstName": "string", "lastName": "string", "phone": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                                              |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                             |
</details>

#### Delete patient (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>DELETE</code> <code><b>/patients/{ patientId:uuid }</b></code><code>(allows to delete patient 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response    |
> |-----------|--------------------|-------------|
> | `204`     | `application/json` | `NoContent` |
> | `404`     | `application/json` | `string`    |
> | `409`     | `application/json` | `string`    |
</details>

------

### Appointments

*Functionality that allows to manage and interact with appointments*

#### Find doctor's free time (*Token required*, 🔒manager-or-patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/appointment/date/doctor-slots/{ doctorId:uuid }</b></code><code>(allows to find doctor's free time 🔒[manager-or-patient policy])</code>
</summary>

##### Parameters
> | name | type         | data type |
> |------|--------------|-----------|
> | Date | not required | date      |

##### Responses
> | http code | content-type       | response                                                                                                                          |
> |-----------|--------------------|-----------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "timeSlots": [{"start": "08:00", "end": "09:00"}]}`  |
> | `400`     | `application/json` | `array`                                                                                                                           |
> | `404`     | `application/json` | `string`                                                                                                                          |
</details>

#### Create new appointment as patient (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>POST</code> <code><b>/appointments/patient</b></code><code>(allows to create new appointment 🔒[patient policy])</code>
</summary>

##### Body
> | name      | type     | data type |
> |-----------|----------|-----------|
> | doctorId  | required | uuid      |
> | date      | required | date      |
> | startTime | required | time      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                     |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                      |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                     |
</details>

#### Create new appointment as doctor (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>POST</code> <code><b>/appointments/doctor</b></code><code>(allows to create new appointment 🔒[doctor policy])</code>
</summary>

##### Body
> | name      | type     | data type |
> |-----------|----------|-----------|
> | patientId | required | uuid      |
> | date      | required | date      |
> | startTime | required | time      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                     |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                      |
> | `404`     | `application/json` | `string`                                                                                                                     |
</details>

#### Create new appointment as manager (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/appointments/manager</b></code><code>(allows to create new appointment 🔒[manager policy])</code>
</summary>

##### Body
> | name      | type     | data type |
> |-----------|----------|-----------|
> | patientId | required | uuid      |
> | doctorId  | required | uuid      |
> | date      | required | date      |
> | startTime | required | time      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                     |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                      |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                     |
</details>

#### Get appointments as patient (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/appointments/patient</b></code><code>(allows to get your appointments 🔒[patient policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | Date       | not required | date      |
> | DoctorId   | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                          |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get appointments as doctor (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>GET</code> <code><b>/appointments/doctor</b></code><code>(allows to get your appointments 🔒[doctor policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | Date       | not required | date      |
> | PatientId  | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                          |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get appointments as manager (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/appointments/manager</b></code><code>(allows to get appointments 🔒[manager policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | Date       | not required | date      |
> | DoctorId   | not required | uuid      |
> | PatientId  | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                          |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Change appointment status (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/appointments/{ appointmentId:uuid }/status</b></code><code>(allows to change appointment status 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name   | type     | data type |
> |--------|----------|-----------|
> | status | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                     |
> |-----------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "2024-04-20", "startTime": "08:00", "endTime": "09:00", "status": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                      |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                     |
</details>

------

### WorkSchedules

*Functionality for configuring doctors' work schedules (entity is created automatically when a doctor is created)*

#### Configure doctor work schedule (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/work-schedules/{ doctorId:uuid }</b></code><code>(allows to config a doctor 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name                | type     | data type |
> |---------------------|----------|-----------|
> | startTime           | required | time      |
> | endTime             | required | time      |
> | appointmentDuration | required | int32     |
> | workdays            | required | array     |

##### Responses
> | http code | content-type       | response                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "startTime": "10:00", "endTime": "17:00", "appointmentDuration": 30, "workdays": ["1", "2"]}`            |
> | `400`     | `application/json` | `array`                                                                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                                                                       |
</details>

#### Get doctor work schedule
<details>
<summary>
    <code>GET</code> <code><b>/work-schedules/{ doctorId:uuid }</b></code><code>(allows to get a doctor's work schedule 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "startTime": "10:00", "endTime": "17:00", "appointmentDuration": 30, "workdays": ["1", "2"]}`            |
> | `404`     | `application/json` | `string`                                                                                                                                                       |
</details>

------

### Specializations

*Functionality that allows to manage and interact with specializations*

#### Create new specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/specializations</b></code><code>(allows to create new specialization 🔒[manager policy])</code>
</summary>

##### Body
> | name        | type         | data type |
> |-------------|--------------|-----------|
> | name        | required     | string    |
> | description | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                        |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
</details>

#### Get all specializations
<details>
<summary>
    <code>GET</code> <code><b>/Specialization</b></code><code>(allows to get all specializations)</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                         |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `[{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}]` |
</details>

#### Get one specialization
<details>
<summary>
    <code>GET</code> <code><b>/specialization/{ specializationId:uuid }</b></code><code>(allows to get one specialization)</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}` |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
</details>

#### Update specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/specialization/{ specializationId:uuid }</b></code><code>(allows to update specialization 🔒[manager policy])</code>
</summary>

##### Body
> | name        | type     | data type |
> |-------------|----------|-----------|
> | description | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                        |
</details>

#### Delete specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>DELETE</code> <code><b>/specialization/{ specializationId:uuid }</b></code><code>(allows to delete specialization 🔒[manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response    |
> |-----------|--------------------|-------------|
> | `204`     | `application/json` | `NoContent` |
> | `400`     | `application/json` | `array`     |
> | `404`     | `application/json` | `string`    |
</details>

#### Include a doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/specialization/{ specializationId:uuid }/include-doctor/{ doctorId:uuid }</b></code><code>(allows to include a doctor 🔒[manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
</details>

#### Exclude a doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/specialization/{ specializationId:uuid }/exclude-doctor/{ doctorId:uuid }</b></code><code>(allows to exclude a doctor 🔒[manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string", "doctors": [{"doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "status": "string", "firstName": "string", "lastName": "string"}]}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                       |
</details>

------

### MedicalRecords

*Functionality that allows to manage and interact with medical records*

#### Create medical record (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>POST</code> <code><b>/medical-record</b></code><code>(allows to create medical record 🔒[doctor policy])</code>
</summary>

##### Body
> | name                     | type         | data type |
> |--------------------------|--------------|-----------|
> | appointmentId            | required     | uuid      |
> | title                    | required     | string    |
> | doctorNote               | required     | string    |
> | recommendationForPatient | required     | string    |
> | diagnosis                | required     | string    |
> | icdCode                  | required     | string    |
> | followUpDate             | required     | date      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                            |
> |-----------|--------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "recommendationForPatient": "string", "diagnosis": "string", "icdCode": "string", "followUpDate": "2024-04-20"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                            |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                            |
</details>

#### Update medical record (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/medical-record/{ medicalRecordId:uuid }</b></code><code>(allows to update medical record 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name                     | type         | data type |
> |--------------------------|--------------|-----------|
> | title                    | not required | string    |
> | recommendationForPatient | not required | string    |
> | followUpDate             | not required | date      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                            |
> |-----------|--------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "recommendationForPatient": "string", "diagnosis": "string", "icdCode": "string", "followUpDate": "2024-04-20"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                            |
</details>

#### Get all medical records for manager (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/medical-records/manager</b></code><code>(allows to get medical records 🔒[manager policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | DoctorId   | not required | uuid      |
> | PatientId  | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
> |-----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "recommendationForPatient": "string", "diagnosis": "string", "icdCode": "string", "followUpDate": "2024-04-20"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get all medical records for patient (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/medical-records/patient</b></code><code>(allows to get your records 🔒[patient policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | DoctorId   | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
> |-----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "recommendationForPatient": "string", "diagnosis": "string", "icdCode": "string", "followUpDate": "2024-04-20"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

#### Get all medical records for doctor (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>GET</code> <code><b>/medical-records/doctor</b></code><code>(allows to get your records 🔒[doctor policy])</code>
</summary>

##### Parameters
> | name       | type         | data type |
> |------------|--------------|-----------|
> | PageNumber | not required | int32     |
> | PageSize   | not required | int32     |
> | PatientId  | not required | uuid      |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
> |-----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [{"medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "recommendationForPatient": "string", "diagnosis": "string", "icdCode": "string", "followUpDate": "2024-04-20"}], "totalItems": 0, "pageNumber": 0, "pageSize": 0, "totalPages": 0}` |
</details>

------

### Offices

*Functionality that allows to manage and interact with offices*

#### Create office (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/offices</b></code><code>(allows to create new office 🔒[manager policy])</code>
</summary>

##### Body
> | name   | type     | data type |
> |--------|----------|-----------|
> | name   | required | string    |
> | number | required | int32     |

##### Responses
> | http code | content-type       | response    |
> |-----------|--------------------|-------------|
> | `200`     | `application/json` | `uuid`      |
> | `400`     | `application/json` | `array`     |
> | `409`     | `application/json` | `string`    |
</details>

#### Get all offices (*Token required*, 🔒base policy)
<details>
<summary>
    <code>GET</code> <code><b>/offices</b></code><code>(allows to get all offices 🔒[base policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `[{"officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 10, "isAvailable": true}]` |
</details>

#### Update office name (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/offices/{ officeId:uuid }/name</b></code><code>(allows to update office name 🔒[manager policy])</code>
</summary>

##### Body
> | name | type     | data type |
> |------|----------|-----------|
> | name | required | string    |

##### Responses
> | http code | content-type       | response                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 10, "isAvailable": true}`   |
> | `400`     | `application/json` | `array`                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                       |
</details>

#### Lock or unlock office (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/offices/{ officeId:uuid }/status</b></code><code>(allows to lock or unlock office 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                       |
> |-----------|--------------------|----------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 10, "isAvailable": true}`   |
> | `400`     | `application/json` | `array`                                                                                                        |
> | `404`     | `application/json` | `string`                                                                                                       |
</details>