using System.Diagnostics;
using Investcloud_Server_Test.Interfaces;
using Microsoft.Extensions.Logging;

namespace Investcloud_Server_Test.Services;

public class ProcessService (
    IApiOperationalService apiOperationalService,
    IHashingService hashingService,
    IMatrixMultiplicationService matrixMultiplicationService,
    ILogger<ProcessService> logger) : IProcessService
{
    public async Task ProcessMatrixMultiplication()
    {
        var apiInitResult = await apiOperationalService.InitializeMatrices();
        if (!apiInitResult)
        {
            return;
        }
        
        var totalTime = Stopwatch.StartNew();

        var fetchMatrixATask = apiOperationalService.FetchMatrix("A");
        var fetchMatrixBTask = apiOperationalService.FetchMatrix("B");
        await Task.WhenAll(fetchMatrixATask, fetchMatrixBTask);

        var matrixA = fetchMatrixATask.Result;
        var matrixB = fetchMatrixBTask.Result;
        
        if (matrixA == null || matrixB == null)
        {
            return;
        }
        
        var resultMatrix = matrixMultiplicationService.MultiplyMatrices(matrixA, matrixB);
        var concatenatedResult = matrixMultiplicationService.ConcatenateMatrix(resultMatrix);
        
        var md5Hash = hashingService.ComputeMd5Hash(concatenatedResult);

        totalTime.Stop();
        
        logger.LogInformation($"Total Processing time: {totalTime.ElapsedMilliseconds} ms");

        await apiOperationalService.ValidateResult(md5Hash);
    }

    private void DebugProcess(int [][] matrixA, int [][] matrixB, int [][] resultMatrix, string concatenatedResult)
    {
        // Debug: Print the first 3x3 section of matrices A and B for verification
        matrixMultiplicationService.PrintMatrixToConsole(matrixA, "Matrix A");
        matrixMultiplicationService.PrintMatrixToConsole(matrixB, "Matrix B");

        // Debug: Print the first 3x3 section of the result matrix for verification
        matrixMultiplicationService.PrintMatrixToConsole(resultMatrix, "Result Matrix");
        
        // Debug: Print the first 50 characters of the concatenated string
        logger.LogDebug($"Concatenated string sample (first 50 chars): {concatenatedResult[..Math.Min(50, concatenatedResult.Length)]}");
    }
}