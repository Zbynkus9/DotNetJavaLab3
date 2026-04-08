using System;
using System.Threading.Tasks;

namespace DotNetJavaLab3;

public class MatrixOperations
{
    public static int[,] GenerateRandomMatrix(int rows, int cols, int numThreads)
    {
        int[,] matrix = new int[rows, cols];
        ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = numThreads };
        Random rnd = new Random();
        Parallel.For(0, rows, parallelOptions, x =>
            Parallel.For(0, cols, parallelOptions, y =>
            {
                matrix[x, y] = rnd.Next(1, 10);
            }));
        return matrix;
    }
    
    public static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}