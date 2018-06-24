using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


public class ListCount
{
    public int phonenumberPosition;
    public int possibleMovePosition;
    public string priorNumberCombination;

}

public class Program
{
    public const int NumberofDigitsInPhone =7 ;
    public static int[,] possibleMoves = new int[20, 2]
    {
        {1, 8}, {1, 6}, {2, 7}, {2, 9}, {3, 4}, {3, 8}, {4, 9}, {4, 3}, {4, 0}, {6, 1}, {6, 7}, {6, 0}, {7, 2}, {7, 6},
        {8, 3}, {8, 1}, {9, 4}, {9, 2}, {0, 4}, {0, 6}
    };

    private static int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();
        return listOfInts.ToArray();
    }


    private static bool ValidateNumber(int possibleNumber)
    {

        var numberArray = GetIntArray(possibleNumber);
        for (int arrayCounter = 0; arrayCounter < numberArray.Length - 2; arrayCounter++)
        {
            bool foundmatch = false;
            for (int x = 0; x < possibleMoves.GetLength(0)-1; x++)
            {
                if (possibleMoves[x, 0] == numberArray[arrayCounter] && possibleMoves[x, 1] == numberArray[arrayCounter + 1])
                {
                    foundmatch = true;
                    break;
                }
            }
            if (!foundmatch)
            {
                return false;
            }
        }

        return true;
    }
    private static List<string> ProcessList
        (int numberCounter)
    {
        var _listOfNumbers = new List<string>();
        var usedcombinations = new List<ListCount>();
        string Number = numberCounter.ToString();
        int lastNumber = numberCounter;
        do
        {
            // We can make each starting digit parrallel
            for (int x = 0; x < possibleMoves.GetLength(0); x++)
            {
                // has to be a valid move that we haven't done before
                if (possibleMoves[x, 0] == lastNumber
                    && !usedcombinations.Exists(
                        listc => listc.phonenumberPosition == Number.Length
                                 && listc.possibleMovePosition == x
                                 && listc.priorNumberCombination == Number.Substring(0, listc.priorNumberCombination.Length)
                    ))

                {
                    usedcombinations.Add(new ListCount()
                    {
                        phonenumberPosition = Number.Length,
                        priorNumberCombination = Number,
                        possibleMovePosition = x
                    });
                    Number += possibleMoves[x, 1].ToString();
                    lastNumber = possibleMoves[x, 1];
                    x = 0;
                }
                if (Number.Length == NumberofDigitsInPhone)
                {
                    break;
                }
            }

            if (Number.Length == NumberofDigitsInPhone)
            {
                _listOfNumbers.Add((Number));
            }
            Number = Number.TrimEnd(Number[Number.Length - 1]);
            if (Number.Length > 0)
            {
                lastNumber = Int32.Parse(Number.Substring(Number.Length - 1, 1));
            }
        } while (Number.Length > 0);

        return _listOfNumbers;
    }
    public static void Main()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        var listOfNumbers = new List<string>();
        Parallel.For(2,10, new ParallelOptions { MaxDegreeOfParallelism = 1 }, numCounter => listOfNumbers.AddRange(ProcessList(numCounter)));
        Console.WriteLine($"The knight has a total of {listOfNumbers.Count} possible Phone numbers, file ouput to program location ");
        TextWriter tw = new StreamWriter("KnightMoves-indexTree.txt");
        foreach (var item in listOfNumbers)
        {
            tw.WriteLine(item);
        }
        tw.Close();
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        Console.WriteLine("B Tree RunTime " + elapsedTime);
        stopWatch.Start();


        //listOfNumbers = new List<string>();
        //for (int i = 2000000; i < 9999999; i++)
        //{
        //    if (ValidateNumber(i))
        //    {
        //        listOfNumbers.Add(i.ToString());
        //        }
        //}
        //Console.WriteLine($"The knight has a total of {listOfNumbers.Count} possible Phone numbers, file ouput to program location ");
        //tw = new StreamWriter("KnightMoves-BruteForce.txt");
        //foreach (var item in listOfNumbers)
        //{
        //    tw.WriteLine(item);
        //}
        //tw.Close();
        //stopWatch.Stop();
        //ts = stopWatch.Elapsed;
        //elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        //Console.WriteLine("Brute Force RunTime " + elapsedTime);
        //Console.WriteLine("RunTime " + elapsedTime);

        Console.ReadLine();
    }
}