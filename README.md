# Healthcare-CRM

Clinic management system. Supports the management of staff, offices, appointments, patients and medical records.

The project implements Clean Architecture, CQRS pattern, MediatR, minimal api with api versioning, repository pattern, EF, custom JWT authorization, fluent validation.

### [PHP Symfony 6 version](https://github.com/gitEugeneL/Healthcare-CRM/tree/PHP-Symfony-6)

---

## 👷 Frameworks, Libraries and Technologies

- [.NET 8](https://github.com/dotnet/core)
- [ASP.NET Core 8](https://github.com/dotnet/aspnetcore)
- [Entity Framework Core](https://github.com/dotnet/efcore)
- [SQL Server 2022](https://github.com/Microsoft/sqllinuxlabs)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [IdentityModel](https://github.com/IdentityModel)
- [MediatR](https://github.com/jbogard/MediatR)
- [Carter](https://github.com/CarterCommunity/Carter)
- [Asp.Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Bogus](https://github.com/bchavez/Bogus)
- [Docker](https://github.com/docker)


## 🐳 List of docker containers

- **app** - container for all application layers

- **database** - MSSQL database container

- **cloudbeaver** - graphical access to SQLServer

## 🚜 How to run the server

*The first time the containers are launched, random data generation will be performed to check the functionality
(Bogus package).*

1. Build and start Docker images based on the configuration defined in the docker-compose.yml

   ```sh
   > make up  # docker-compose up --build
   ```

2. Stop and remove containers
   ```sh
   > make down  # docker-compose down
   ```

## 🔐 Local access

| container   | port | login | password       | GUI                                      |
|-------------|------|-------|----------------|------------------------------------------|
| database    | 1433 | SA    | Dev_password@1 | -                                        |
| cloudbeaver | 9000 | -     | -              | http://localhost:9000                    |
| app         | 8080 | -     | -              | http://localhost:8080/swagger/index.html |    


## 🖨️ Swagger documentation

1. Swagger UI

        http://localhost:5000/swagger/index.html

2. [Swagger static file](https://github.com/gitEugeneL/RightPlace/blob/main/swagger.json)

        https://github.com/gitEugeneL/RightPlace/blob/main/swagger.json

## 💾 Database diagram

![Database diagram](https://github.com/gitEugeneL/Healthcare-CRM/blob/main/diagram.png?raw=true)


## 🔧 Implementation features

### Authentication


*Authentication is implemented using a JWT access token and refresh token.*

*AccessToken is used to authorize users, the refresh token is used to update a pair of tokens.*

*RefreshToken is recorded in the database and allows each user to have 5 active devices at the same time.*

#### Login
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/login</b></code><code>(allows you to login, issues accessToken and sets refreshToken in cookies)</code>
</summary>

##### Body
> | name     | type       | data type    |                                                           
> |----------|------------|--------------|
> | email    | required   | string       |
> | password | required   | string       |

##### Responses
> | http code | content-type       | response                                                    |
> |-----------|--------------------|-------------------------------------------------------------|
> | `200`     | `application/json` | `{"type: "Bearer", "accessToken": "eyJhbGciOi..........."}` |
> | `400`     | `application/json` | `array`                                                     |

##### Set Cookies
> | name         | example                                                              |                                                      
> |--------------|----------------------------------------------------------------------|
> | refreshToken | refreshToken=Wna@3da...; Expires=...; Secure; HttpOnly; Domain=...;` |
</details>

#### Refresh
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/refresh</b></code><code>(allows to refresh access and refresh tokens)</code>
</summary>

##### Required Cookies
> | name         | example                  |                                                      
> |--------------|--------------------------|
> | refreshToken | refreshToken=Wna@3da...; |

##### Responses
> | http code | content-type       | response                                                    |
> |-----------|--------------------|-------------------------------------------------------------|
> | `200`     | `application/json` | `{"type: "Bearer", "accessToken": "eyJhbGciOi..........."}` |
> | `400`     | `application/json` | `array`                                                     |
> | `401`     | `application/json` | `string`                                                    |
</details>

#### Logout

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/logout</b></code><code>(allows to logout and deactivates refresh token)</code>
</summary>

##### Body
> 1. Valid access JWT Bearer token in the header

##### Responses
> | http code | content-type                                    | response    |
> |-----------|-------------------------------------------------|-------------|
> | `204`     | `application/json` `and remove HttpOnly Cookie` | `NoCOntent` |
> | `400`     | `application/json`                              | `string`    |
> | `401`     | `application/json`                              | `string`    |
</details>

--------------

### Manager

*Functionality that allows to manage and interact with managers*

#### Register new manager (*Token required*, 🔒admin policy)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/manager</b></code><code>(allows to register new manager 🔒️[admin policy])</code>
</summary>

##### Body
> | name              | type       | data type |                                                           
> |-------------------|------------|-----------|
> | "email"           | required   | string    |
> | "password"        | required   | string    |


##### Responses
> | http code | content-type       | response                                                                                                                                                      |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json` | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "position": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                       |
> | `401`     | `application/json` | `string`                                                                                                                                                      |
> | `403`     | `application/json` | `string`                                                                                                                                                      |
> | `409`     | `application/json` | `string`                                                                                                                                                      |
</details>

#### Update your manager account (*Token required*, 🔒manager policy)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/manager</b></code><code>(allows to update manager account 🔒️[manager policy])</code>
</summary>

##### Body
> | name        | type         | data type |                                                           
> |-------------|--------------|-----------|
> | "firstName" | not required | string    |
> | "lastName"  | not required | string    |
> | "phone"     | not required | string    |
> | "position"  | not required | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                      |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "position": "string"}` |
> | `400`     | `application/json` | `array`                                                                                                                                                       |
> | `401`     | `application/json` | `string`                                                                                                                                                      |
> | `403`     | `application/json` | `string`                                                                                                                                                      |
> | `404`     | `application/json` | `string`                                                                                                                                                      |
</details>

------

### Doctor

*Functionality that allows to manage and interact with doctors*

#### Create new doctor (*Token required*, 🔒manager policy)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/doctor</b></code><code>(allows to create new doctor 🔒️[manager policy])</code>
</summary>

##### Body
> | name              | type       | data type |                                                           
> |-------------------|------------|-----------|
> | "email"           | required   | string    |
> | "password"        | required   | string    |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json` | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "status": "string", "description": "string", "education": "string", "appointmentSettingsId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializationIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] }` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                        |
> | `401`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |
> | `403`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |                                                                                                                                                                                                                                                                                                                                
</details>

#### Update your doctor account (*Token required*, 🔒doctor policy)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/doctor</b></code><code>(allows to update doctor account 🔒️[doctor policy])</code>
</summary>

##### Body
> | name          | type         | data type  |                                                           
> |---------------|--------------|------------|
> | "firstName"   | not required | string     |
> | "lastName"    | not required | string     |
> | "phone"       | not required | string     |
> | "status"      | not required | string     |
> | "description" | not required | string     |
> | "education"   | not required | string     |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                       |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "status": "string", "description": "string", "education": "string", "appointmentSettingsId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializationIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] }` |
> | `400`     | `application/json` | `array`                                                                                                                                                                                                                                                                                                                                        |
> | `401`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |
> | `403`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                       |                                                                                                                                                                                                                                                                                                                                
</details>

#### Get all doctors
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/doctor</b></code><code>(allows to get all doctors)</code>
</summary>

##### Parameters
> | name                    | type         | data type |                                                           
> |-------------------------|--------------|-----------|
> | PageNumber              | not required | int32     |
> | PageSize                | not required | int32     |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                |
> |-----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{"items": [ { "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "status": "string", "description": "string", "education": "string", "appointmentSettingsId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializationIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] } ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }` |
</details>

#### Get one doctor
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/doctor/{ userId:uuid }</b></code><code>(allows to get one doctor)</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                                       |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "status": "string", "description": "string", "education": "string", "appointmentSettingsId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "specializationIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] }` |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                       |

</details>

-----

### Patient

*Functionality that allows to manage and interact with patients*

#### Register new patient

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/patient</b></code><code>(allows to register new patient)</code>
</summary>

##### Body
> | name              | type       | data type |                                                           
> |-------------------|------------|-----------|
> | "email"           | required   | string    |
> | "password"        | required   | string    |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                          |
> |-----------|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{ "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "pesel": "string", "dateOfBirth": "string", "insurance": "string", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                           |
> | `409`     | `application/json`  | `string`                                                                                                                                                                                                                                                          |
</details>

#### Update your patient account (*Token required*, 🔒patient policy)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/patient</b></code><code>(allows to update your patient account 🔒[patient policy])</code>
</summary>

##### Body
> | name          | type         | data type |                                                           
> |---------------|--------------|-----------|
> | "firstName"   | not required | string    |
> | "lastName"    | not required | string    |
> | "phone"       | not required | string    |
> | "pesel"       | not required | string    |
> | "dateOfBirth" | not required | string    |
> | "insurance"   | not required | string    |
> | "firstName"   | not required | string    |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                          |
> |-----------|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "pesel": "string", "dateOfBirth": "string", "insurance": "string", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                          |    
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                          |      
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                          |
</details>

#### Delete your patient account (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>DELETE</code> <code><b>/api/v1/patient</b></code><code>(allows to delete your patient account 🔒[patient policy])</code>
</summary>

##### Responses
> | http code | content-type        | response    |
> |-----------|---------------------|-------------|
> | `204`     | `application/json`  | `NoContent` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`    |    
> | `403`     | `application/json`  | `string`    |      
> | `404`     | `application/json`  | `string`    |
</details>

#### Get all patients (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/patient</b></code><code>(allows to get all patients 🔒[doctor-or-manager policy])</code>
</summary>

##### Parameters
> | name                    | type         | data type |                                                           
> |-------------------------|--------------|-----------|
> | PageNumber              | not required | int32     |
> | PageSize                | not required | int32     |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                                  |
> |-----------|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `204`     | `application/json`  | `{"items": [ { "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "pesel": "string", "dateOfBirth": "string", "insurance": "string", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" } ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                  |    
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                  |      
</details>

#### Get one patient (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/patient{ userId:uuid: }</b></code><code>(allows to get one patient 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                         |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "pesel": "string", "dateOfBirth": "string", "insurance": "string", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                         |    
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                         |      
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                         |
</details>

-----

### Address

*Functionality allows to manage the patients' addresses (entity is created automatically when a patient is registered)*

#### Update patient address (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/address</b></code><code>(allows to update patient address 🔒[patient policy])</code>
</summary>

##### Body
> | name         | type         | data type |                                                           
> |--------------|--------------|-----------|
> | "province"   | not required | string    |
> | "postalCode" | not required | string    |
> | "city"       | not required | string    |
> | "street"     | not required | string    |
> | "house"      | not required | string    |
> | "apartment"  | not required | string    |

##### Responses
> | http code | content-type         | response                                                                                                                         |
> |-----------|----------------------|----------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`   | `{"province": "string", "postalCode": "string", "city": "string", "street": "string", "hose": "string", "apartment": "string" }` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `400`     | `application/json`   | `array`                                                                                                                          |
> | `401`     | `application/json`   | `string`                                                                                                                         |    
> | `403`     | `application/json`   | `string`                                                                                                                         |      
> | `404`     | `application/json`   | `string`                                                                                                                         |
</details>

#### Get patient's address (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/address/{ addressId:uuid }</b></code><code>(allows to get patient's address 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                        |
> |-----------|---------------------|---------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"province": "string", "postalCode": "string", "city": "string", "street": "string", "hose": "string", "apartment": "string"}` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`                                                                                                                        |    
> | `403`     | `application/json`  | `string`                                                                                                                        |      
> | `404`     | `application/json`  | `string`                                                                                                                        |
</details>

-----

### Appointment

*Functionality that allows to manage and interact with appointments*

#### Find doctor's free time (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/appointment/find-time/{ userDoctorId:uuid }/{ date:string }</b></code><code>(allows find doctor's free time 🔒[patient policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                            |
> |-----------|---------------------|---------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "freeHours": [ { "start": "08:00", "end": "09:00" } ] }` |                                                                                                                                                                                                                                                           |                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`                                                                                                            |    
> | `403`     | `application/json`  | `string`                                                                                                            |      
> | `404`     | `application/json`  | `string`                                                                                                            |
> | `422`     | `application/json`  | `string`                                                                                                            |
</details>

#### Create new appointment (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/appointment</b></code><code>(allows to create new appointment 🔒[patient policy])</code>
</summary>

##### Body
> | name           | type       | data type |                                                           
> |----------------|------------|-----------|
> | "userDoctorId" | required   | uuid      |
> | "date"         | required   | string    |
> | "startTime"    | required   | string    |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                             |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "string", "startTime": "string", "endTime": "string", "isCompleted": true, "isCanceled": true }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                                              |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
</details>

#### Get personal appointment list (*Token required*)
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/appointment/{ date:string }</b></code><code>(allows to get your personal an appointment list 🔒[token required]) </code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                  |
> |-----------|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `[ { "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "string", "startTime": "string", "endTime": "string", "isCompleted": true, "isCanceled": true } ]` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                  |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                  |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                  |
> | `422`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                  |
</details>

#### Finalise appointment (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/appointment/finalise/{ appointmentId:uuid }</b></code><code>(allows to finalise an appointment 🔒[doctor policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                             |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "string", "startTime": "string", "endTime": "string", "isCompleted": true, "isCanceled": true }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                                              |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `422`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
</details>

#### Cancel appointment (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/appointment/cancel/{ appointmentId:uuid }</b></code><code>(allows to cancel an appointment 🔒[doctor policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                             |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "date": "string", "startTime": "string", "endTime": "string", "isCompleted": true, "isCanceled": true }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                                              |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
> | `422`     | `application/json`  | `string`                                                                                                                                                                                                                                                                             |
</details>

-----

### Appointment settings

*Functionality for configuring doctors' appointments settings (entity is created automatically when a doctor is created)*

#### Configuration doctor (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/appointment-settings</b></code><code>(allows to config a doctor 🔒[doctor policy])</code>
</summary>

##### Body
> | name        | type       | data type |                                                           
> |-------------|------------|-----------|
> | "startTime" | required   | string    |
> | "endTime"   | required   | string    |
> | "interval"  | required   | string    |
> | "workdays"  | required   | array     |


##### Responses
> | http code | content-type        | response                                                                                         |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "startTime": "10:00", "endTime": "17:00", "interval": "30M", "workdays": [ 1, 2, 3, 4, 5 ] }` |
> | `400`     | `application/json`  | `array`                                                                                          |
> | `401`     | `application/json`  | `string`                                                                                         |
> | `403`     | `application/json`  | `string`                                                                                         |
> | `404`     | `application/json`  | `string`                                                                                         |
> | `422`     | `application/json`  | `string`                                                                                         |
</details>


#### Get doctor configuration (*Token required*)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/appointment-settings/{ settingsId:uuid }</b></code><code>(allows to get a doctor's configuration 🔒[token required])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                   |
> |-----------|---------------------|--------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"startTime": "08:00, "endTime": "15:00", "interval": "1H", "workdays": [ 1, 2, 4, 5 ] }` |
> | `401`     | `application/json`  | `string`                                                                                   |
> | `403`     | `application/json`  | `string`                                                                                   |
> | `404`     | `application/json`  | `string`                                                                                   |
</details>

------

### Specialization

*Functionality that allows to manage and interact with specializations*

#### Create new specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/specialization</b></code><code>(allows to create new specialization 🔒[manager policy])</code>
</summary>

##### Body
> | name          | type         | data type |                                                           
> |---------------|--------------|-----------|
> | "value"       | required     | string    |
> | "description" | not required | string    |

##### Responses
> | http code | content-type        | response                                                                                                                                                                  |
> |-----------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{ "specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "value": "string", "description": "string", "userDoctorIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                   |
> | `401`     | `application/json`  | `string`                                                                                                                                                                  |
> | `403`     | `application/json`  | `string`                                                                                                                                                                  |
> | `409`     | `application/json`  | `string`                                                                                                                                                                  |
</details>

#### Update a specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/specialization</b></code><code>(allows to update specialization 🔒[manager policy])</code>
</summary>

##### Body
> | name               | type         | data type |                                                           
> |--------------------|--------------|-----------|
> | "specializationId" | required     | uuid      |
> | "description"      | not required | string    |
 
> ##### Responses
> | http code | content-type         | response                                                                                                                                                                  |
> |-----------|----------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`   | `{ "specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "value": "string", "description": "string", "userDoctorIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] }` |
> | `400`     | `application/json`   | `array`                                                                                                                                                                   |
> | `401`     | `application/json`   | `string`                                                                                                                                                                  |
> | `403`     | `application/json`   | `string`                                                                                                                                                                  |
> | `404`     | `application/json`   | `string`                                                                                                                                                                  |
</details>

#### Delete a specialization (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>DELETE</code> <code><b>/api/v1/specialization/{ specializationId:uuid }</b></code><code>(allows to delete specialization 🔒[manager policy])</code>
</summary>

> ##### Responses
> | http code | content-type        | response    |
> |-----------|---------------------|-------------|
> | `204`     | `application/json`  | `NoContent` |
> | `401`     | `application/json`  | `string`    |
> | `403`     | `application/json`  | `string`    |
> | `404`     | `application/json`  | `string`    |
</details>

#### Get all specializations
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/specialization</b></code><code>(allows to get all specialization)</code>
</summary>

> ##### Responses
> | http code | content-type        | response                                                                                                                                                                     |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `[{ "specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "value": "string", "description": "string", "userDoctorIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] } ]` |
</details>

#### Include a doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/specialization/include-doctor</b></code><code>(allows to include a doctor 🔒[manager policy])</code>
</summary>

##### Body
> | name                 | type     | data type |                                                           
> |----------------------|----------|-----------|
> | "specializationId"   | required | uuid      |
> | "userDoctorId"       | required | uuid      |

> ##### Responses
> | http code | content-type        | response                                                                                                                                                                    |
> |-----------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "value": "string", "description": "string", "userDoctorIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] } ]` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                    |
> | `403`     | `application/json`  | `string`                                                                                                                                                                    |
> | `404`     | `application/json`  | `string`                                                                                                                                                                    |
> | `409`     | `application/json`  | `string`                                                                                                                                                                    |
</details>

#### Exclude a doctor (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/specialization/exclude-doctor</b></code><code>(allows to exclude a doctor 🔒[manager policy])</code>
</summary>

##### Body
> | name                 | type     | data type |                                                           
> |----------------------|----------|-----------|
> | "specializationId"   | required | uuid      |
> | "userDoctorId"       | required | uuid      |

> ##### Responses
> | http code | content-type        | response                                                                                                                                                                    |
> |-----------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "specializationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "value": "string", "description": "string", "userDoctorIds": [ "3fa85f64-5717-4562-b3fc-2c963f66afa6" ] } ]` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                    |
> | `403`     | `application/json`  | `string`                                                                                                                                                                    |
> | `404`     | `application/json`  | `string`                                                                                                                                                                    |
> | `409`     | `application/json`  | `string`                                                                                                                                                                    |
</details>

------

### Medical record

*Functionality that allows to manage and interact with medical records*

#### Create medical records (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/medical-record</b></code><code>(allows to create medical records 🔒[doctor policy])</code>
</summary>

##### Body
> | name            | type         | data type |                                                           
> |-----------------|--------------|-----------|
> | "appointmentId" | required     | uuid      |
> | "userPatientId" | required     | uuid      |
> | "title"         | required     | string    |
> | "doctorNote"    | not required | string    |

> ##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                    |
> |-----------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{ "medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "created": "2024-04-20T19:46:52.893Z" }` |
> | `400`     | `application/json`  | `array`                                                                                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
> | `409`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
</details>

#### Update medical records (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/medical-record</b></code><code>(allows to update medical records 🔒[doctor policy])</code>
</summary>

##### Body
> | name              | type         | data type |                                                           
> |-------------------|--------------|-----------|
> | "medicalRecordId" | required     | uuid      |
> | "title"           | not required | string    |
> | "doctorNote"      | not required | string    |

> ##### Responses
> | http code | content-type         | response                                                                                                                                                                                                                                                                                                                    |
> |-----------|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`   | `{ "medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "created": "2024-04-20T19:50:45.998Z" }` |
> | `400`     | `application/json`   | `array`                                                                                                                                                                                                                                                                                                                     |
> | `401`     | `application/json`   | `string`                                                                                                                                                                                                                                                                                                                    |
> | `403`     | `application/json`   | `string`                                                                                                                                                                                                                                                                                                                    |
> | `404`     | `application/json`   | `string`                                                                                                                                                                                                                                                                                                                    |
</details>

#### Get one medical record (*Token required*, 🔒doctor-or-patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/medical-record/{ medicalRecordId:uuid }</b></code><code>(allows to get one medical record 🔒[doctor-or-patient policy])</code>
</summary>

> ##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                    |
> |-----------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "created": "2024-04-20T19:53:30.118Z" }` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                    |
</details>

#### Get all medical records for patient (*Token required*, 🔒patient policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/medical-record/for-patient</b></code><code>(allows to get your records 🔒[patient policy])</code>
</summary>

##### Parameters
> | name          | type         | data type |                                                           
> |---------------|--------------|-----------|
> | SortByDate    | not required | boolean   |
> | SortOrderAsc  | not required | boolean   |
> | PageNumber    | not required | int32     |
> | PageSize      | not required | int32     |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                                                                                             |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "items": [ { "medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "created": "2024-04-20T19:55:13.407Z" } ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
</details>

#### Get all medical records for doctor (*Token required*, 🔒doctor policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/medical-record/for-doctor</b></code><code>(allows to get your records 🔒[doctor policy])</code>
</summary>

##### Parameters
> | name          | type         | data type |                                                           
> |---------------|--------------|-----------|
> | UserPatientId | not required | uuid      |
> | SortByDate    | not required | boolean   |
> | SortOrderAsc  | not required | boolean   |
> | PageNumber    | not required | int32     |
> | PageSize      | not required | int32     |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                                                                                                                                                                                                             |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "items": [ { "medicalRecordId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userPatientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "userDoctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "appointmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "doctorNote": "string", "created": "2024-04-20T19:55:13.407Z" } ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                                                                                                                                                                                                             |
</details>

----

### Office

*Functionality that allows to manage and interact with offices*

#### Create offices (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/office</b></code><code>(allows to create new office 🔒[manager policy])</code>
</summary>

##### Body
> | name         | type           | data type |                                                           
> |--------------|----------------|-----------|
> | "name"       | required       | string    |
> | "number"     | required       | int       |

##### Responses
> | http code | content-type        | response                                                                                                       |
> |-----------|---------------------|----------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{ "officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 201, "isAvailable": true }` |
> | `400`     | `application/json`  | `array`                                                                                                        |
> | `401`     | `application/json`  | `string`                                                                                                       |
> | `403`     | `application/json`  | `string`                                                                                                       |
> | `409`     | `application/json`  | `string`                                                                                                       |
</details>

#### Update offices (*Token required*, 🔒manager policy)
<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/office</b></code><code>(allows to update offices 🔒[manager policy])</code>
</summary>

##### Body
> | name       | type           | data type |                                                           
> |------------|----------------|-----------|
> | "officeId" | required       | uuid      |
> | "name"     | required       | string    |

##### Responses
> | http code | content-type        | response                                                                                                       |
> |-----------|---------------------|----------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 201, "isAvailable": true }` |
> | `400`     | `application/json`  | `array`                                                                                                        |
> | `401`     | `application/json`  | `string`                                                                                                       |
> | `403`     | `application/json`  | `string`                                                                                                       |
> | `404`     | `application/json`  | `string`                                                                                                       |
</details>

#### Lock or unlock an office (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>PATCH</code> <code><b>/api/v1/office</b></code><code>(allows to lock or unlock an office 🔒[doctor-or-manager policy])</code>
</summary>

##### Body
> | name       | type           | data type |                                                           
> |------------|----------------|-----------|
> | "officeId" | required       | uuid      |

##### Responses
> | http code | content-type        | response                                                                                                       |
> |-----------|---------------------|----------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{ "officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 201, "isAvailable": true }` |
> | `400`     | `application/json`  | `array`                                                                                                        |
> | `401`     | `application/json`  | `string`                                                                                                       |
> | `403`     | `application/json`  | `string`                                                                                                       |
> | `404`     | `application/json`  | `string`                                                                                                       |
</details>

#### Get all offices (*Token required*, 🔒doctor-or-manager policy)
<details>
<summary>
    <code>GET</code> <code><b>/api/v1/office</b></code><code>(allows to get all offices 🔒[doctor-or-manager policy])</code>
</summary>

##### Responses
> | http code | content-type        | response                                                                                                          |
> |-----------|---------------------|-------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `[ { "officeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "number": 10, "isAvailable": true } ]` |
> | `400`     | `application/json`  | `array`                                                                                                           |
> | `401`     | `application/json`  | `string`                                                                                                          |
> | `403`     | `application/json`  | `string`                                                                                                          |
</details>