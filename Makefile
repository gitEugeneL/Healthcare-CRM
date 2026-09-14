up:
	docker compose -f docker-compose.yml up -d --build

down:
	docker compose -f docker-compose.yml down
	
down-and-clean:
	docker compose down -v

db-migrate:
	@cd Backend && dotnet ef migrations add $(name) --project Infrastructure/Persistence --startup-project Presentation/Api --output-dir Database/Migrations

db-update:
	@cd Backend && dotnet ef database update --project Infrastructure/Persistence --startup-project Presentation/Api