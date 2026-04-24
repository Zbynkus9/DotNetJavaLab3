using System.Threading;
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

    public static int[,] ThreadMultiplayer(int[,] matrixA, int[,] matrixB, int threadCount)
    {
        int RowsA = matrixA.GetLength(0);
        int ColsA = matrixA.GetLength(1);
        int ColsB = matrixB.GetLength(1);
        
        int[,] resultMatrix = new int[RowsA, ColsB];
        
       Thread[] threads = new Thread[threadCount]; 
       int rowsPerThread = RowsA / threadCount; 
        
       for (int t = 0; t < threadCount; t++)
       {
           int threadId = t; // Zmienna lokalna dla domknięcia (closure) w wyrażeniu lambda
            
           threads[t] = new Thread(() =>
           {
               int startRow = threadId * rowsPerThread;
               int endRow = (threadId == threadCount - 1) ? RowsA : startRow + rowsPerThread;

               for (int i = startRow; i < endRow; i++)
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
           });
           threads[t].Start();
       }

       // Czekamy na zakończenie wszystkich wątków
       foreach (var thread in threads)
       {
           thread.Join();
       }

        return resultMatrix;
    }
}