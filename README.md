# Healthcare-CRM

## [PHP Symfony 6 version](https://github.com/gitEugeneL/Healthcare-CRM/tree/PHP-Symfony-6)

---

Clinic management system. Supports the management of staff, offices, appointments, patients and medical records.

The project implements a clean architecture, CQRS pattern, Repository pattern, custom authorization.

## Main technologies

* [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
* [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core)
* [MediatR](https://github.com/jbogard/MediatR)
* [SQL-Server 2022](https://www.microsoft.com/pl-pl/sql-server/sql-server-2022)
* [XUnit](https://xunit.net)
* [Docker](https://www.docker.com)


## List of containers

* **database** - MsSQL database container.

* **app** - container for all application layers.


## How to run the server

The first time the containers are launched, random data generation will be performed to check the functionality (Bogus package).

1. Build and start Docker images based on the configuration defined in the docker-compose.yml.

        make up     // docker-compose up --build

2. Stop and remove containers.

        make down   // docker-compose down


## API documentation

1. Swagger documentation

        http://localhost:8080/swagger/index.html


## Database diagram

![Database diagram](https://github.com/gitEugeneL/Healthcare-CRM/blob/main/diagram.png?raw=true)
