using System.Security.Cryptography;
using System.Text;
using Investcloud_Server_Test.Interfaces;

namespace Investcloud_Server_Test.Services;

public class HashingService : IHashingService
{
    public string ComputeMd5Hash(string input)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(data);
    }
}