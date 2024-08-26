using System.Text;
using Investcloud_Server_Test.Interfaces;
using Microsoft.Extensions.Logging;

namespace Investcloud_Server_Test.Services;

public class MatrixMultiplicationService(ILogger<MatrixMultiplicationService> logger) : IMatrixMultiplicationService
{
    public int[][] MultiplyMatrices(int[][] matrixA, int[][] matrixB)
    {
        var size = matrixA.Length;
        var resultMatrix = new int[size][];
        for (var i = 0; i < size; i++)
        {
            resultMatrix[i] = new int[size];
        }

        Parallel.For(0, size, i =>
        {
            for (var j = 0; j < size; j++)
            {
                var sum = 0;
                for (var k = 0; k < size; k++)
                {
                    sum += matrixA[i][k] * matrixB[k][j];
                }

                resultMatrix[i][j] = sum;
            }
        });

        return resultMatrix;
    }

    public string ConcatenateMatrix(int[][] matrix)
    {
        var sb = new StringBuilder();
        foreach (var row in matrix)
        {
            foreach (var element in row)
            {
                sb.Append(element);
            }
        }

        return sb.ToString();
    }

    public void PrintMatrixToConsole(int[][] matrix, string matrixName)
    {
        Console.WriteLine($"{matrixName} sample (top-left 3x3):");
        for (var i = 0; i < Math.Min(Const.MatrixSize, matrix.Length); i++)
        {
            for (var j = 0; j < Math.Min(Const.MatrixSize, matrix[i].Length); j++)
            {
                Console.WriteLine(matrix[i][j] + "\t");
            }

            Console.WriteLine();
        }
    }
}