using Application.Common.Interfaces;
using Bogus;
using Domain.Doctors;
using Domain.Managers;
using Domain.Offices;
using Domain.Specializations;
using Domain.Users;
using Domain.WorkSchedules;

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
        
        /*** Doctors ***/
        var doctors = new List<Doctor>();

        var allWorkdays = new[]
        {
            Workday.Monday, Workday.Tuesday, Workday.Wednesday,
            Workday.Thursday, Workday.Friday, Workday.Saturday
        };

        for (var i = 0; i < 10; i++)
        {
            var doctorUser = User.Create(
                email: $"doctor-{faker.Lorem.Word()}-{faker.Random.Number(1, 9999)}@mail.dev",
                passwordHash: hash,
                passwordSalt: salt,
                role: UserAuthRole.Doctor,
                firstName: faker.Person.FirstName,
                lastName: faker.Person.LastName,
                phone: faker.Person.Phone);

            var doctor = Doctor.Create(
                description: faker.Lorem.Sentence(),
                education: faker.Company.CompanyName(),
                user: doctorUser);

            var startHour = faker.Random.Int(7, 10);
            var endHour = faker.Random.Int(16, 20);
            var workdaysCount = faker.Random.Int(3, 6);

            var workScheduleResult = WorkSchedule.Create(
                doctorId: doctor.Id,
                startTime: new TimeOnly(startHour, 0),
                endTime: new TimeOnly(endHour, 0),
                appointmentDuration: faker.PickRandom<AppointmentDuration>(),
                workdays: faker.Random.ListItems(allWorkdays, workdaysCount).ToList());

            doctor.AssignWorkSchedule(workScheduleResult.Value);

            foreach (var specialization in faker.Random.ListItems(specializations, 2))
                specialization.IncludeDoctor(doctor);

            doctors.Add(doctor);
        }
        
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
        
        context.AddRange(doctors);
        // context.AddRange(patient);
        context.AddRange(offices);
        
        context.SaveChanges();
    }
}
