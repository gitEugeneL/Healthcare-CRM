namespace Api.Utils;

internal abstract class AuthTags
{
    internal const string BasePolicy = "base-policy";
    
    internal const string PatientPolicy = "patient-policy";
    
    internal const string DoctorPolicy = "doctor-policy";
    
    internal const string ManagerPolicy = "manager-policy";
    
    internal const string AdminPolicy = "admin-policy";
    
    internal const string DoctorOrPatientPolicy = "doctor-or-patient-policy";
    
    internal const string ManagerOrPatientPolicy = "manager-or-patient-policy";
    
    internal const string DoctorOrManagerPolicy = "doctor-or-manager-policy";
}
