using Domain.Abstractions.Errors;

namespace Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error UnavailableSlot = Error.Problem(
        "Appointment.UnavailableSlotStart",
        $"Slot is unavailable");

    public static readonly Error EndTimeBeforeStartTime = Error.Problem(
        "Appointment.EndTimeBeforeStartTime",
        "End time cannot be before start time");

    public static Error AlreadyBooked => Error.Problem(
        "Appointment.AlreadyBooked",
        "Patient already has an active appointment with this doctor on this date.");
    
    public static Error InvalidStatus(string status) => Error.Problem(
        "Appointment.InvalidStatus",
        $"'{status}' is not a valid appointment status.");
    
    public static Error NotFound(Guid appointmentId) => Error.NotFound(
        "Appointment.AppointmentNotFound", 
        $"The appointment with the identifierMa {appointmentId} was not found");
    
    public static readonly Error AlreadyCanceled = Error.Problem(
        "Appointment.AlreadyCanceled",
        "The appointment is already canceled");
    
    public static readonly Error AlreadyCompleted = Error.Problem(
        "Appointment.AlreadyCompleted",
        "The appointment is already completed");
    
    public static readonly Error AlreadyConfirmed = Error.Problem(
        "Appointment.AlreadyConfirmed",
        "The appointment is already confirmed");
    
    public static readonly Error AlreadyStarted = Error.Problem(
        "Appointment.AlreadyStarted",
        "The appointment is already started");
    
    public static readonly Error StartTimeTooEarly = Error.Problem(
        "Appointment.StartTimeTooEarly",
        "It is too early");
}