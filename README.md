# Healthcare-CRM

## Migration to .NET in process..!
---

Final project on the subject of **MVC-web-application-development**.

Сlinic management system. Supports the management of staff, offices, appointments, patients and medical records.

<i>Docker, PostgreSQL, Php-8, Symfony-6, PHPUnit, RestAPI, JWT-authentication with refresh token, Vue.js client</i>

## How to run the server

1. Build Docker images based on the configuration defined in the docker-compose.yml.

         make build

2. Start containers and run composition for all services defined in the docker-compose.yml.

        make up

3. Stop and remove containers.

        make down

## How to configure the database

1.  Execute a migration in database.

        make migrate_db

2. Upload data into the database.

        make upload_data_db


## How to configure the server

1. Executes an interactive Bash shell within a PHP container.

        make app_bush

2. Generate a key pair for JWT validation.

        make generate_auth_keys

3. Revoke all invalid (datetime expired) refresh tokens.

        make clear_refresh_tokens

## How to run tests

1. Create a database for tests.

        make create_test_db

2. Test database migration.

       make migrate_test_db

3. Upload testing data into the test database.

        make upload_data_test_db

4. Run tests.

        make start_tests

## API documentation

### Swagger documentation 

    1. /api/doc
    2. /api/doc.json


## Database diagram

![Database diagram](https://github.com/gitEugeneL/Healthcare-CRM/blob/main/diagram.png?raw=true)
