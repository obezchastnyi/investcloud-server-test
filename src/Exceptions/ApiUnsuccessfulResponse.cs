namespace Investcloud_Server_Test.Exceptions;

public class ApiUnsuccessfulResponse : Exception
{
    public string Error { get; set; }
}