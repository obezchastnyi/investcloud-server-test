namespace Investcloud_Server_Test.Modals;

public abstract class ApiResponse
{
    public string Cause { get; set; }
    
    public bool Success { get; set; }
}