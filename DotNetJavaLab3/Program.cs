using System;

namespace DotNetJavaLab3;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter number of threads");
        int numThreads = int.Parse(Console.ReadLine());
        int matrixSize = 1000;
        Console.WriteLine("Enter number of iterations");
        int iterations = int.Parse(Console.ReadLine());
        long[,] times = new long[3, iterations];
        for (int i = 0; i < iterations; i++)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int[,] matrixA = MatrixOperations.GenerateRandomMatrix(matrixSize, matrixSize, numThreads);
            int[,] matrixB = MatrixOperations.GenerateRandomMatrix(matrixSize, matrixSize, numThreads);
            watch.Stop();
            // Console.WriteLine($"Elapsed Matrix Generation time: {watch.ElapsedMilliseconds} ms");

            watch = System.Diagnostics.Stopwatch.StartNew();
            int[,] matrixC = MatrixMultiplayer.SequentialMultiplayer(matrixA, matrixB);
            //int[,] matrixC = MatrixMultiplayer.ParallelMultiplayer(matrixA, matrixB, 1);
            times[0, i] = watch.ElapsedMilliseconds;
            watch.Stop();
            // Console.WriteLine($"Elapsed Matrix Sequential Multiplayer time: {watch.ElapsedMilliseconds} ms");
            watch = System.Diagnostics.Stopwatch.StartNew();
            int[,] matrixD = MatrixMultiplayer.ParallelMultiplayer(matrixA, matrixB, numThreads);
            watch.Stop();
            times[1, i] = watch.ElapsedMilliseconds;
            // Console.WriteLine($"Elapsed Matrix Parallel Multiplayer time: {watch.ElapsedMilliseconds} ms");
            watch = System.Diagnostics.Stopwatch.StartNew();
            int[,] matrixE = MatrixMultiplayer.ThreadMultiplayer(matrixA, matrixB, numThreads);
            watch.Stop();
            // Console.WriteLine($"Elapsed Matrix Thread Multiplayer time: {watch.ElapsedMilliseconds} ms");
            times[2, i] = watch.ElapsedMilliseconds;
        }
        
        long[] avgTimes = new long[3];

        for (int i = 0; i < iterations; i++)
        {
            avgTimes[0]  += times[0, i];
            avgTimes[1] += times[1, i];
            avgTimes[2] += times[2, i];
        }
        
        avgTimes[0] = avgTimes[0]/iterations;
        avgTimes[1] = avgTimes[1]/iterations;
        avgTimes[2] = avgTimes[2]/iterations;
        
        
        Console.WriteLine($"Avg Matrix Sequential Multiplayer time: {avgTimes[0]} ms");
        Console.WriteLine($"Avg Matrix Sequential Multiplayer time: {avgTimes[1]} ms");
        Console.WriteLine($"Avg Matrix Sequential Multiplayer time: {avgTimes[2]} ms");
        
        
        // Console.WriteLine("Matrix A:");
        // MatrixOperations.PrintMatrix(matrixA);
        // Console.WriteLine("Matrix B:");
        // MatrixOperations.PrintMatrix(matrixB);
        // Console.WriteLine("Matrix C (A x B):");
        // MatrixOperations.PrintMatrix(matrixC);
        // Console.WriteLine("Matrix D (A x B):");
        // MatrixOperations.PrintMatrix(matrixD);
        // Console.WriteLine("Matrix E (A x B):");
        // MatrixOperations.PrintMatrix(matrixE);
 
    }
    
}