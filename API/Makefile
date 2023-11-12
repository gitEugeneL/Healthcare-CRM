#------------------------------------#
#           Docker compose			 #
#------------------------------------#

build:
	docker-compose -f ./docker/docker-compose.yml build

up:
	docker-compose -f ./docker/docker-compose.yml up

down:
	docker-compose -f ./docker/docker-compose.yml down


#------------------------------------#
#                APP                 #
#------------------------------------#

app_bash:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bash

# TESTS --------------------------------------------------------------------------------------------------------------

create_test_db:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console --env=test doctrine:database:create

migrate_test_db:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console --env=test doctrine:schema:create

upload_data_test_db:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console --env=test doctrine:fixtures:load

start_tests:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/phpunit

# DATABASE -----------------------------------------------------------------------------------------------------------

migrate_db:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console doctrine:migrations:migrate

upload_data_db:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console doctrine:fixtures:load

# AUTH ---------------------------------------------------------------------------------------------------------------

generate_auth_keys:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console lexik:jwt:generate-keypair

clear_refresh_tokens:
	docker-compose -f ./docker/docker-compose.yml exec -u www-data php bin/console gesdinet:jwt:clear
