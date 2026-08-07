using Bogus;
using Domain.Managers;
using Domain.Offices;
using Domain.Users;
using Infrastructure.Security;

namespace Infrastructure.Persistence;

internal static class DataGenerator
{
    public static void Seed(DataContext context)
    {
        if (context.Managers.Any())
            return;
        
        var faker = new Faker();
        
        var passwordManager = new PasswordManager();
        passwordManager.CreatePasswordHash("devDev123!", out var hash, out var salt);
       
        string[] officeNames =
        [
            "Orthodontics Suite", 
            "Periodontics Center", 
            "Endodontics Room", 
            "Prosthodontics Studio",
            "Oral Surgery Unit"
        ];
        
        // var specializations = new List<Specialization>
        // {
            // new() { Value = "Orthodontics" },
            // new() { Value = "Periodontics" },
            // new() { Value = "Endodontics" },
            // new() { Value = "Prosthodontics" },
            // new() { Value = "Surgery" }
        // };
        
        // var admin = new Faker<User>()
            // .RuleFor(u => u.Email, "admin@mail.dev")
            // .RuleFor(u => u.Role, Role.Admin)
            // .RuleFor(u => u.PasswordHash, hash)
            // .RuleFor(u => u.PasswordSalt, salt)
            // .RuleFor(u => u.FirstName, "Admin")
            // .RuleFor(u => u.LastName, "Admin")
            // .Generate();
        
            
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
            
            
            
        //     
        // var manager = new Faker<Manager>()
        //     .RuleFor(m => m.Position, "Main manager")
        //     .RuleFor(m => m.User.Email, "manager@mail.dev")
        //     .RuleFor(m => m.User.Role, UserAuthRole.Manager)
        //     .RuleFor(m => m.User.PasswordHash, hash)
        //     .RuleFor(m => m.User.PasswordSalt, salt)
        //     .RuleFor(m => m.User.FirstName, f => f.Person.FirstName)
        //     .RuleFor(m => m.User.LastName, f => f.Person.LastName)
        //     .RuleFor(m => m.User.Phone, f => f.Person.Phone)
        //     .Generate();
        //
        
        
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
            
        // context.AddRange(admin);
        context.AddRange(manager.Value);
        // context.AddRange(doctor);
        // context.AddRange(patient);
        context.AddRange(offices);
        
        context.SaveChanges();
    }
}
