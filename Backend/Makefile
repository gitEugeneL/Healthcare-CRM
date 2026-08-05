up:
	docker compose -f docker-compose.yml up -d --build

down:
	docker compose -f docker-compose.yml down
	
down-and-clean:
	docker compose down -v


db-migrate:
	# make migrate name=migration_name
	dotnet ef migrations add $(name) --project Infrastructure --startup-project Api --output-dir Persistence/Migrations
	
db-update:
	dotnet ef database update --project Infrastructure --startup-project Api