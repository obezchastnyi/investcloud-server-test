using System.Text;
using Investcloud_Server_Test.Exceptions;
using Investcloud_Server_Test.Interfaces;
using Investcloud_Server_Test.Modals;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Investcloud_Server_Test.Services;

public class ApiOperationalService(
    IHttpClientFactory httpClientFactory,
    ILogger<ApiOperationalService> logger) : IApiOperationalService
{
    public async Task<bool> InitializeMatrices()
    {
        var url = $"{Const.InvestCloudApiBase}/{string.Format(Const.InvestCloudInitMatrixApi, Const.MatrixSize)}";

        using var client = httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            ProcessApiResponse<ApiInitMatrixResponse>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                string.Format(Const.ApiErrorLog, nameof(InitializeMatrices), nameof(HttpRequestException), ex.Message), ex);
            return false;
        }
        catch (ApiUnsuccessfulResponse ex)
        {
            logger.LogError(
                string.Format(Const.ApiErrorLog, nameof(InitializeMatrices), nameof(ApiUnsuccessfulResponse), ex.Message), ex);
            return false;
        }

        logger.LogInformation("Matrix Initialization is successful");
        return true;
    }

    public async Task<int[][]?> FetchMatrix(string dataset)
    {
        using var client = httpClientFactory.CreateClient();

        const int size = Const.MatrixSize;
        var matrix = new int[size][];

        var fetchTasks = new Task<string>[size];
        for (var i = 0; i < size; i++)
        {
            var url = $"{Const.InvestCloudApiBase}/{string.Format(Const.InvestCloudFetchMatrixApi, dataset, i)}";
            fetchTasks[i] = client.GetStringAsync(url);
        }

        string[] results = [];
        
        try
        {
            results = await Task.WhenAll(fetchTasks);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                string.Format(Const.ApiErrorLog, nameof(FetchMatrix), nameof(HttpRequestException), ex.Message), ex);
            return null;
        }
        
        for (var i = 0; i < size; i++)
        {
            var apiResult = JsonConvert.DeserializeObject<ApiFetchDatasetResponse>(results[i]);
            if (apiResult != null)
            {
                matrix[i] = apiResult.Value;
            }
        }

        return matrix;
    }

    public async Task ValidateResult(string md5Hash)
    {
        using var client = httpClientFactory.CreateClient();

        const string url = $"{Const.InvestCloudApiBase}/{Const.InvestCloudValidateApi}";
        var content = new StringContent(md5Hash, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            ProcessApiResponse<ApiValidateMatrixResponse>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError($"Matrix validation error: {ex.Message}");
        }
        catch (ApiUnsuccessfulResponse ex)
        {
            logger.LogError(
                string.Format(Const.ApiErrorLog, nameof(ValidateResult), nameof(ApiUnsuccessfulResponse), ex.Message), ex);
        }

        logger.LogInformation(Const.ApiSuccessfulValidationLog);
    }

    private static void ProcessApiResponse<T>(string responseContent)
        where T : ApiResponse
    {
        var apiResult = JsonConvert.DeserializeObject<T>(responseContent);
        if (apiResult is not { Success: true })
        {
            throw new ApiUnsuccessfulResponse
            {
                Error = apiResult.Cause
            };
        }
    }
}