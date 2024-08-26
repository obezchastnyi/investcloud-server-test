namespace Investcloud_Server_Test.Interfaces;

public interface IMatrixMultiplicationService
{
    int[][] MultiplyMatrices(int[][] matrixA, int[][] matrixB);

    string ConcatenateMatrix(int[][] matrix);

    void PrintMatrixToConsole(int[][] matrix, string matrixName);
}