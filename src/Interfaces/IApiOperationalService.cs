namespace Investcloud_Server_Test.Interfaces;

public interface IApiOperationalService
{
    Task<bool> InitializeMatrices();

    Task<int[][]?> FetchMatrix(string dataset);

    Task ValidateResult(string md5Hash);
}