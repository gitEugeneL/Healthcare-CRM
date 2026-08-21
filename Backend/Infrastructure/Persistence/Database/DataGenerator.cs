using Application.Abstractions;
using Bogus;
using Domain.Addresses;
using Domain.Doctors;
using Domain.Managers;
using Domain.Offices;
using Domain.Patients;
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
                firstName: faker.Name.FirstName(), 
                lastName: faker.Name.LastName(), 
                phone: faker.Phone.PhoneNumber()));
        
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
                firstName: faker.Name.FirstName(),
                lastName: faker.Name.LastName(),
                phone: faker.Phone.PhoneNumber());

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
        
        /*** Patients ***/
        var patients = new List<Patient>();

        for (var i = 0; i < 20; i++)
        {
            var patientUser = User.Create(
                email: $"patient-{faker.Lorem.Word()}-{faker.Random.Number(1, 9999)}@mail.dev",
                passwordHash: hash,
                passwordSalt: salt,
                role: UserAuthRole.Patient,
                firstName: faker.Name.FirstName(),
                lastName: faker.Name.LastName(),
                phone: faker.Phone.PhoneNumber());

            var address = Address.Create(
                province: faker.Address.State(),
                postalCode: faker.Address.ZipCode(),
                city: faker.Address.City(),
                street: faker.Address.StreetName(),
                hose: faker.Address.BuildingNumber(),
                apartment: faker.Random.Bool()
                    ? faker.Random.Number(1, 200).ToString()
                    : null);

            var patient = Patient.Create(
                dateOfBirth: faker.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2024, 1, 1)),
                pesel: faker.Random.Replace("###########"),
                insurance: faker.Lorem.Sentence(),
                user: patientUser,
                address: address);

            patients.Add(patient);
        }

        var offices = officeNames
                .Select(name => Office.Create(name, faker.Random.Number(100, 999)));
            
        context.Add(admin);
        context.Add(manager.Value);
        
        context.AddRange(doctors);
        context.AddRange(patients);
        context.AddRange(offices);

        context.AddRange(specializations);

        context.SaveChanges();
    }
}