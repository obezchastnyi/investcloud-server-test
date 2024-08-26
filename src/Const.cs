namespace Investcloud_Server_Test;

public static class Const
{
    #region Parameters

    public const int MatrixSize = 1000;

    #endregion
    
    #region Api

    public const string InvestCloudApiBase = "https://recruitment-test.investcloud.com/api/numbers";
    
    public const string InvestCloudInitMatrixApi = "init/{0}";
    
    public const string InvestCloudFetchMatrixApi = "{0}/row/{1}";

    public const string InvestCloudValidateApi = "validate";

    #endregion

    #region Logs

    public const string ApiSuccessfulValidationLog = "Validation is successful";
    
    public const string ApiErrorLog = "{0} error. '{1}' : {2}";

    #endregion
}