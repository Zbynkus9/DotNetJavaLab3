using System;

namespace DotNetJavaLab3;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter number of threads");
        int numThreads = int.Parse(Console.ReadLine());
        int matrixSize = 1000;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        int[,] matrixA = MatrixOperations.GenerateRandomMatrix(matrixSize, matrixSize, numThreads);
        int[,] matrixB = MatrixOperations.GenerateRandomMatrix(matrixSize, matrixSize, numThreads);
        watch.Stop();
        Console.WriteLine($"Elapsed Matrix Generation time: {watch.ElapsedMilliseconds} ms");
        watch = System.Diagnostics.Stopwatch.StartNew();
        int[,] matrixC = MatrixMultiplayer.SequentialMultiplayer(matrixA, matrixB);
        //int[,] matrixC = MatrixMultiplayer.ParallelMultiplayer(matrixA, matrixB, 1);
        watch.Stop();
        Console.WriteLine($"Elapsed Matrix Sequential Multiplayer time: {watch.ElapsedMilliseconds} ms");
        watch = System.Diagnostics.Stopwatch.StartNew();
        int[,] matrixD = MatrixMultiplayer.ParallelMultiplayer(matrixA, matrixB, numThreads);
        watch.Stop();
        Console.WriteLine($"Elapsed Matrix Parallel Multiplayer time: {watch.ElapsedMilliseconds} ms");
        watch = System.Diagnostics.Stopwatch.StartNew();
        int[,] matrixE = MatrixMultiplayer.ThreadMultiplayer(matrixA, matrixB, numThreads);
        watch.Stop();
        Console.WriteLine($"Elapsed Matrix Thread Multiplayer time: {watch.ElapsedMilliseconds} ms");
        
        
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