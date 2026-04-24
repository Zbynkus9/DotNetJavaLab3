# Obliczenia wielowątkowe w .NET

Projekt zrealizowany w ramach laboratorium z przedmiotu Platformy Programistyczne .NET i Java.



## Opis projektu
1. **Matrix Multiplication (Console App):** Testy wydajnościowe mnożenia macierzy (sekwencyje, Paraller.For, Thread)
2. **Image Processing (WinForms App):**  Aplikacja okienkowa pozwalająca na równoległe nakładanie różnych filtrów graficznych na wybrane zdjęcie przy wykorzystaniu asynchroniczności

## 🏗 Struktura Projektu i Kluczowe Klasy

<img width="405" height="399" alt="image" src="https://github.com/user-attachments/assets/bc35e924-86a6-49e7-aa77-43e4997ba701" />

### Moduł macierzy
* **`MatrixMultiplier`**: Główna klasa logiczna.
    * `MultiplySequential`: Implementacja jednowątkowa (baseline).
    * `MultiplyParallel`: Wykorzystuje `Parallel.For` do automatycznego rozdzielenia pracy na rdzenie procesora.
    * `MultiplyThreads`: Implementacja niskopoziomowa. Ręcznie dzieli zakres wierszy macierzy na tablicę obiektów `Thread`.
* **`MatrixOperations`**: Klasa narzędziowa do generowania losowych macierzy o zadanym rozmiarze.

### Moduł przetwarzania obrazu
* **`ImageFilters`**: Statyczna klasa zawierająca algorytmy filtrów (Grayscale, Negative, Threshold, RedFilter). Każda metoda operuje na kopii obiektu `Bitmap` dla zachowania bezpieczeństwa wątkowego.
* **`Form1`**: Obsługa interfejsu użytkownika. Wykorzystuje `Task.Run` oraz `Task.WhenAll` do równoległego uruchamiania filtrów bez blokowania głównego wątku UI.

## Kluczowe fragmenty kodu

1. Zad 1 (Mnożenie macierzy sekwencyjnie i z użyciem Paraller)

```cs
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
```

2. Zad 2 (Mnożenie macierzy z użyciem Thread)

```cs
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
```

3. Zad 3 (Wielowątkowe przetwarzanie obrazów)

```cs
private async void BtnProcess_Click(object sender, EventArgs e)
{
    if (currentImage == null) return;

    btnProcess.Enabled = false;
    btnLoad.Enabled = false;

    // Tasks (z puli wątków). 
    // Każde zadanie wykonuje się równolegle w osobnym wątku roboczym
    Task<Bitmap> task1 = Task.Run(() => ImageFilters.ApplyGrayscale(currentImage));
    Task<Bitmap> task2 = Task.Run(() => ImageFilters.ApplyNegative(currentImage));
    Task<Bitmap> task3 = Task.Run(() => ImageFilters.ApplyThreshold(currentImage));
    Task<Bitmap> task4 = Task.Run(() => ImageFilters.ApplyRedFilter(currentImage));

    // Czekamy asynchronicznie, aż wszystkie 4 wątki zakończą pracę
    await Task.WhenAll(task1, task2, task3, task4);

    // Zwrócenie obrazów do GUI (bezpieczne, bo await wraca do wątku głównego)
    pbFilter1.Image = task1.Result;
    pbFilter2.Image = task2.Result;
    pbFilter3.Image = task3.Result;
    pbFilter4.Image = task4.Result;

    btnProcess.Enabled = true;
    btnLoad.Enabled = true;
}
```
