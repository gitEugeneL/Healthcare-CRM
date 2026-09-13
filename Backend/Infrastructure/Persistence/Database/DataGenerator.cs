using Application.Abstractions.Security;
using Bogus;
using Domain.Addresses;
using Domain.Appointments;
using Domain.Doctors;
using Domain.Managers;
using Domain.MedicalRecords;
using Domain.Offices;
using Domain.Patients;
using Domain.Specializations;
using Domain.Users;
using Domain.WorkSchedules;

namespace Persistence.Database;

internal static class DataGenerator
{
    public static void Seed(DataContext context, IPasswordService passwordService)
    {
        if (context.Offices.Any())
            return;

        var faker = new Faker();
        passwordService.CreatePasswordHash("devDev123!", out var hash, out var salt);

        string[] officeNames =
        [
            "Orthodontics Suite",
            "Periodontics Center",
            "Endodontics Room",
            "Prosthodontics Studio",
            "Oral Surgery Unit",
            "Pediatric Dentistry Room",
            "Radiology Cabinet"
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
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday
        };

        for (var i = 0; i < 12; i++)
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

            doctor.ChangeStatus(faker.Random.Bool(0.85f) ? DoctorStatus.Active : DoctorStatus.Disable);

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

            foreach (var specialization in faker.Random.ListItems(specializations, faker.Random.Int(1, 2)))
                specialization.IncludeDoctor(doctor);

            doctors.Add(doctor);
        }

        /*** Patients ***/
        var patients = new List<Patient>();

        for (var i = 0; i < 25; i++)
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
                insurance: faker.Random.Bool(0.8f) ? faker.Company.CompanyName() + " Insurance" : null,
                user: patientUser,
                address: address);

            // немного разнообразим статусы пациентов
            if (faker.Random.Bool(0.05f))
                patient.Deactivate();

            patients.Add(patient);
        }

        var offices = officeNames
            .Select(name => Office.Create(name, faker.Random.Number(100, 999)))
            .ToList();

        // часть кабинетов временно недоступна (на ремонте/занята)
        foreach (var office in faker.Random.ListItems(offices, faker.Random.Int(1, 2)))
            office.ChangeAvailability();

        /*** Appointments & Medical Records ***/
        var appointments = new List<Appointment>();
        var medicalRecords = new List<MedicalRecord>();

        string[] diagnoses =
        [
            "Dental caries, unspecified",
            "Chronic periodontitis",
            "Malocclusion, unspecified",
            "Pulpitis",
            "Gingivitis, chronic",
            "Impacted wisdom tooth",
            "Dental abscess",
            "Bruxism",
            "Tooth wear",
            "Dentofacial anomaly",
            "Periapical abscess without sinus",
            "Dental plaque accumulation"
        ];

        string[] icdCodes =
        [
            "K02.9", "K05.3", "K07.4", "K04.0", "K05.1",
            "K01.1", "K04.7", "F45.8", "K03.0", "K07.9",
            "K04.6", "K03.6"
        ];

        string[] doctorNotes =
        [
            "Patient reports mild discomfort, no acute symptoms observed.",
            "Routine checkup completed, no abnormalities found.",
            "Follow-up recommended to monitor healing progress.",
            "Procedure completed without complications.",
            "Patient tolerated the procedure well.",
            "Further imaging required for accurate diagnosis.",
            "Patient advised on oral hygiene improvements.",
            "Symptoms consistent with initial diagnosis, treatment plan adjusted.",
            "Local anesthesia administered prior to procedure.",
            "Mild inflammation noted, prescribed anti-inflammatory course."
        ];

        string[] recommendations =
        [
            "Maintain regular brushing and flossing routine.",
            "Avoid hard or sticky foods for the next few days.",
            "Schedule a follow-up appointment in two weeks.",
            "Use prescribed mouthwash twice daily.",
            "Apply cold compress if swelling occurs.",
            "Take prescribed pain medication as needed.",
            "Avoid hot or cold beverages for 48 hours.",
            "Return immediately if bleeding does not subside."
        ];

        string[] recordTitles =
        [
            "Initial Consultation", "Follow-up Visit", "Routine Checkup",
            "Treatment Session", "Post-op Review", "Emergency Visit"
        ];

        foreach (var doctor in doctors)
        {
            var schedule = doctor.WorkSchedule;
            if (schedule is null)
                continue;

            var duration = (int)schedule.AppointmentDuration;
            var latestPossibleStartHour = Math.Max(schedule.StartTime.Hour, schedule.EndTime.Hour - 1);
            var appointmentsForDoctor = faker.Random.Int(5, 9);

            for (var i = 0; i < appointmentsForDoctor; i++)
            {
                var isPast = faker.Random.Bool(0.6f);
                var date = isPast
                    ? faker.Date.Between(DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-1))
                    : faker.Date.Between(DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));

                var dateOnly = DateOnly.FromDateTime(date);
                var startHour = faker.Random.Int(schedule.StartTime.Hour, latestPossibleStartHour);
                var startTime = new TimeOnly(startHour, faker.PickRandom(0, 15, 30, 45));
                var endTime = startTime.AddMinutes(duration);

                var patient = faker.PickRandom(patients);

                var appointmentResult = Appointment.Create(dateOnly, startTime, endTime, doctor.Id, patient.Id);
                if (appointmentResult.IsFailure)
                    continue;

                var appointment = appointmentResult.Value;
                appointment.Initialize();

                if (isPast)
                {
                    if (faker.Random.Bool(0.15f))
                    {
                        appointment.Cancel();
                    }
                    else
                    {
                        appointment.Confirm();
                        appointment.Complete();

                        if (appointment.Status == AppointmentStatus.Completed && faker.Random.Bool(0.75f))
                        {
                            var hasFollowUp = faker.Random.Bool(0.5f);

                            var medicalRecordResult = MedicalRecord.Create(
                                appointmentId: appointment.Id,
                                title: faker.PickRandom(recordTitles),
                                doctorNote: faker.PickRandom(doctorNotes),
                                recommendationForPatient: faker.PickRandom(recommendations),
                                diagnosis: faker.PickRandom(diagnoses),
                                icdCode: faker.PickRandom(icdCodes),
                                followUpDate: hasFollowUp
                                    ? DateOnly.FromDateTime(DateTime.Now.AddDays(faker.Random.Int(7, 30)))
                                    : null);

                            if (medicalRecordResult.IsSuccess)
                                medicalRecords.Add(medicalRecordResult.Value);
                        }
                    }
                }
                else
                {
                    if (faker.Random.Bool(0.15f))
                        appointment.Cancel();
                    else if (faker.Random.Bool(0.5f))
                        appointment.Confirm();
                }

                appointments.Add(appointment);
            }
        }

        context.Add(admin);
        context.Add(manager.Value);

        context.AddRange(doctors);
        context.AddRange(patients);
        context.AddRange(offices);
        context.AddRange(specializations);
        context.AddRange(appointments);
        context.AddRange(medicalRecords);

        context.SaveChanges();
    }
}