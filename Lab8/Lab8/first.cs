using System;
using System.Linq;
using System.IO;

namespace Lab8
{
    public class First
    {
        /*
        Реализовать алгоритмы поиска: линейный, бинарный, интерполяционный.
        Данные взять из файла sorted.dat, созданного в лабораторной работе №7.
        Для каждого алгоритма должны выводиться на консоль следующие данные:
        позиция найденного элемента (или сообщение «Не найдено»), время работы
        алгоритма ( «секунды : миллисекунды» ), количество сравнений.
        */
        static void shellSort(int[] array, int length, out ulong countOfTranspositions)
        {
            countOfTranspositions = 0;
            DateTime startTime = DateTime.Now;
            int[] steps = { 57, 23, 10, 4, 1 };
            foreach (int step in steps)
                for (int i = step; i < length; i++)
                {
                    int j = i;
                    int temp = array[i];
                    while (j >= step && temp > array[j - step])
                    {
                        array[j] = array[j - step];
                        j -= step;
                    }
                    array[j] = temp;
                    if (j != i)
                        countOfTranspositions++;
            }
            if (countOfTranspositions > 0)
                countOfTranspositions--;
        }
        public static void Execute()
        {
            ulong countOfComparisons = 0, countOfTranspositions;
            TimeSpan workTime = new TimeSpan();
            string indexOfNumber = String.Empty;
            int length = 100000;
            int[] array = new int[length];
            string path = @"C:\Lab7_2\sorted.dat";
            using (StreamReader readArray = new StreamReader(new FileStream(path, FileMode.OpenOrCreate)))
            {
                try
                {
                    try
                    {
                        Console.WriteLine("Reading array from file...");
                        for (int i = 0; i < array.Length; i++)
                            array[i] = int.Parse(readArray.ReadLine());
                        Console.WriteLine("Checking array...");
                        shellSort(array, length, out countOfTranspositions);
                        if (countOfTranspositions == 0)
                            Console.WriteLine("Array is sorted\n");
                        else
                            Console.WriteLine("Array is not sorted\n");
                    }
                    catch (ArgumentNullException)
                    {
                        bool isEmpty = true;
                        foreach (int number in array)
                            if (number != 0)
                                isEmpty = false;
                        if (isEmpty)
                            Console.WriteLine("File is empty!\n");
                        else
                            Console.WriteLine("Array is not full\n");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Array is broken\n");
                }
            }
            int select = 0;
            while (true)
            {
                Console.Write("Choose linear(1), binary(2) or interp(3) search or type 4 to quit: ");
                select = int.Parse(Console.ReadLine());
                if (select == 4)
                    break;
                if (select != 1 && select != 2 && select != 3)
                    continue;
                Console.Write("Type what number you want to find: ");
                int numberToSearch = int.Parse(Console.ReadLine());
                switch (select)
                {
                    case 1:
                        Console.WriteLine("\nLinear search:\n");
                        linearSearch(array, numberToSearch, indexOfNumber, countOfComparisons, workTime);
                        break;
                    case 2:
                        Console.WriteLine("\nBinary search:\n");
                        binarySearch(array, numberToSearch, indexOfNumber, countOfComparisons, workTime);
                        break;
                    case 3:
                        Console.WriteLine("\nInterp search:\n");
                        interpSearch(array, numberToSearch, indexOfNumber, countOfComparisons, workTime);
                        break;
                    default:
                        break;
                }
            }
        }
        public static void linearSearch(int[] array, int numberToSearch, string indexOfNumber, ulong countOfComparisons, TimeSpan workTime)
        {
            DateTime startTime = DateTime.Now;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == numberToSearch)
                    indexOfNumber += i + " ";
                countOfComparisons++;
            }
            DateTime endTime = DateTime.Now;
            workTime = endTime - startTime;
            if (indexOfNumber != String.Empty)
                Console.WriteLine("Indexes of number: {0}", indexOfNumber);
            else
                Console.WriteLine("There are no such number in array");
            Console.WriteLine("Count of comparisons: {0}", countOfComparisons);
            Console.WriteLine("Work time: {0}\n", workTime);
        }
        public static void binarySearch(int[] array, int numberToSearch, string indexOfNumber, ulong countOfComparisons, TimeSpan workTime)
        {
            int left = 0, right = array.Length - 1;
            DateTime startTime = DateTime.Now;
            while (right >= left)
            {
                int mid = (left + right) / 2;
                if (array[mid] == numberToSearch)
                    indexOfNumber += mid + " ";
                if (numberToSearch > array[mid])
                    right = mid - 1;
                else
                    left = mid + 1;
                countOfComparisons++;
            }
            DateTime endTime = DateTime.Now;
            workTime = endTime - startTime;
            if (indexOfNumber != String.Empty)
                Console.WriteLine("Indexes of number: {0}", indexOfNumber);
            else
                Console.WriteLine("There are no such number in array");
            Console.WriteLine("Count of comparisons: {0}", countOfComparisons);
            Console.WriteLine("Work time: {0}\n", workTime);
        }
        public static void interpSearch(int[] array, int numberToSearch, string indexOfNumber, ulong countOfComparisons, TimeSpan workTime)
        {
            int left = 0, right = array.Length - 1;
            DateTime startTime = DateTime.Now;
            while (right >= left)
            {
                int mid = left + (right - left) * (numberToSearch - array[left]) / (array[right] - array[left]);
                if (numberToSearch > array[mid])
                {
                    right = mid - 1;
                    countOfComparisons++;
                }
                else if (numberToSearch < array[mid])
                {
                    left = mid + 1;
                    countOfComparisons++;
                }
                else
                {
                    indexOfNumber += mid + " ";
                    break;
                }
            }
            DateTime endTime = DateTime.Now;
            workTime = endTime - startTime;
            if (indexOfNumber != String.Empty)
                Console.WriteLine("Indexes of number: {0}", indexOfNumber);
            else
                Console.WriteLine("There are no such number in array");
            Console.WriteLine("Count of comparisons: {0}", countOfComparisons);
            Console.WriteLine("Work time: {0}\n", workTime);
        }
    }
}
