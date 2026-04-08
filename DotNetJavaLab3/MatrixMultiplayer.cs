using System.Threading.Tasks;

namespace DotNetJavaLab3;

public class MatrixMultiplayer
{
    public static int[,] SequentialMultiplayer(int[,] matrixA, int[,] matrixB)
    {
        int RowsA = matrixA.GetLength(0);
        int ColsA = matrixA.GetLength(1);
        int ColsB = matrixB.GetLength(1);
        
        int[,] resultMatrix = new int[RowsA, ColsB];

        for (int i = 0; i < RowsA; i++)
        {
            for (int j = 0; j < ColsB; j++)
            {
                int tmp = 0;
                for (int k = 0; k < ColsA; k++)
                {
                    tmp += matrixA[i, k] * matrixB[k, j];
                }
                resultMatrix[i, j] = tmp;
            }
        }
        return resultMatrix;
    }

    public static int[,] ParallelMultiplayer(int[,] matrixA, int[,] matrixB, int maxThreads)
    {
        int RowsA = matrixA.GetLength(0);
        int ColsA = matrixA.GetLength(1);
        int ColsB = matrixB.GetLength(1);
        
        int[,] resultMatrix = new int[RowsA, ColsB];
        
        ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = maxThreads };

        Parallel.For(0, RowsA, parallelOptions, i =>
        {
            for (int j = 0; j < ColsB; j++)
            {
                int tmp = 0;
                for (int k = 0; k < ColsA; k++)
                {
                    tmp += matrixA[i, k] * matrixB[k, j];
                }

                resultMatrix[i, j] = tmp;
            }
        });
        return resultMatrix;
    }
}