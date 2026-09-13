namespace Application.Abstractions.Security;

public interface IPasswordService
{
    void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
   
    bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
}
