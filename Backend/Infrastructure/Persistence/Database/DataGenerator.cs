using Application.Common.Interfaces;
using Bogus;
using Domain.Managers;
using Domain.Offices;
using Domain.Specializations;
using Domain.Users;

namespace Persistence.Database;

internal static class DataGenerator
{
    public static void Seed(DataContext context, IPasswordManager passwordManager)
    {
        if (context.Offices.Any())
            return;
        
        var faker = new Faker();
        passwordManager.CreatePasswordHash("devDev123!", out var hash, out var salt);
        
        string[] officeNames =
        [
            "Orthodontics Suite", 
            "Periodontics Center", 
            "Endodontics Room", 
            "Prosthodontics Studio",
            "Oral Surgery Unit"
        ];
        
        var specializations = new List<Specialization>
        {
            Specialization.Create("Orthodontics", "Orthodontics description"),
            Specialization.Create("Periodontics", "Periodontics description"),
            Specialization.Create("Endodontics", "Endodontics description"),
            Specialization.Create("Prosthodontics", "Prosthodontics description"),
            Specialization.Create("Surgery", "Surgery description"),
        };
        
        var admin = User.Create(
            email: "dev@mail.dev",
            passwordHash: hash,
            passwordSalt: salt,
            role: UserAuthRole.Admin,
            null,
            null,
            null);
            
        var manager = Manager.Create(
            position: "Main manager",  
            User.Create(
                email: "manager@mail.dev", 
                passwordHash: hash, 
                passwordSalt: salt, 
                role: UserAuthRole.Manager, 
                firstName: faker.Person.FirstName, 
                lastName: faker.Person.LastName, 
                phone: faker.Person.Phone));
            
        
        // var doctor = new Faker<User>()
            // .RuleFor(u => u.Email, f => 
                // $"doctor-{f.Lorem.Word()}-{f.Random.Number(1, 9999)}@mail.dev")
            // .RuleFor(u => u.Role, Role.Doctor)
            // .RuleFor(u => u.PasswordHash, hash)
            // .RuleFor(u => u.PasswordSalt, salt)
            // .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            // .RuleFor(u => u.LastName, f => f.Person.LastName)
            // .RuleFor(u => u.Phone, f => f.Person.Phone)
            // .RuleFor(u => u.UserDoctor, _ =>
                // new Faker<UserDoctor>()
                    // .RuleFor(d => d.Status, Status.Active)
                    // .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                    // .RuleFor(d => d.Education, f => f.Company.CompanyName())
                    // .RuleFor(d => d.Specializations, f => 
                        // f.Random.ListItems(specializations, 2))
                    // .RuleFor(d => d.AppointmentSettings, new AppointmentSettings
                    // {
                        // StartTime = new TimeOnly(08,00),
                        // EndTime = new TimeOnly(18, 00),
                        // Interval = Interval.Min60,
                        // Workdays = 
                            // [Workday.Monday, Workday.Tuesday, Workday.Wednesday, Workday.Thursday, Workday.Friday]
                    // })
                // )
            // .Generate(10);

        // var patient = new Faker<User>()
            // .RuleFor(u => u.Email, f =>
                // $"patient-{f.Lorem.Word()}-{f.Random.Number(1, 9999)}@mail.dev")
            // .RuleFor(u => u.Role, Role.Patient)
            // .RuleFor(u => u.PasswordHash, hash)
            // .RuleFor(u => u.PasswordSalt, salt)
            // .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            // .RuleFor(u => u.LastName, f => f.Person.LastName)
            // .RuleFor(u => u.Phone, f => f.Person.Phone)
            // .RuleFor(u => u.UserPatient, _ =>
                // new Faker<UserPatient>()
                    // .RuleFor(p => p.Pesel, f => f.Random.Replace("###########"))
                    // .RuleFor(p => p.DateOfBirth, f =>
                        // f.Date.BetweenDateOnly(new DateOnly(1990, 01, 01),
                            // new DateOnly(2024, 01, 01))
                    // )
                    // .RuleFor(p => p.Insurance, f => f.Lorem.Sentence())
                    // .RuleFor(p => p.Address, _ =>
                        // new Faker<Address>()
                            // .RuleFor(a => a.Province, f => f.Address.State())
                            // .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                            // .RuleFor(a => a.City, f => f.Address.City())
                            // .RuleFor(a => a.Street, f => f.Address.StreetName())
                            // .RuleFor(a => a.Hose, f => f.Address.BuildingNumber())
                            // .RuleFor(a => a.Apartment, f => f.Random.Number(1, 200).ToString())
                    // )
            // )
            // .Generate(20);



            var offices = officeNames
                .Select(name => Office.Create(name, faker.Random.Number(100, 999)));
            
        context.Add(admin);
        context.Add(manager.Value);
        
        // context.AddRange(doctor);
        // context.AddRange(patient);
        context.AddRange(offices);
        
        context.SaveChanges();
    }
}
