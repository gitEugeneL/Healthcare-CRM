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