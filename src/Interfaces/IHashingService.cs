namespace Investcloud_Server_Test.Interfaces;

public interface IHashingService
{
    string ComputeMd5Hash(string input);
}