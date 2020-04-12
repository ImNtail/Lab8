using System;
using System.IO;

namespace Lab8 {
    public class Second {
        /*
        Реализовать алгоритмы КМП, БМ, простого поиска подстроки и проверить
        на различных тестах. Тестовые строки (искомые подстроки) должны быть
        представлены произвольными строками и строками с повторяющимися
        фрагментами. Сравнить эффективность одних тех же алгоритмов для
        разных подстрок. Для каждого алгоритма должны выводиться на консоль
        следующие данные: позиция найденного элемента (или сообщение «Не найдено»),
        время работы алгоритма ( «секунды : миллисекунды» ), количество сравнений.
        */
        public static void Execute()
        {
            ulong countOfComparisons = 0;
            TimeSpan workTime = new TimeSpan();
            string text = String.Empty;
            using (StreamReader readText = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "text.txt")))
                text = readText.ReadToEnd();
            int index = 0;
            string select = String.Empty;
            while (true)
            {
                Console.Write("Write the search method (1 - simple, 2 - KMP, 3 - BM) or write 4 to exit: ");
                select = Console.ReadLine();
                if (select == "4")
                    break;
                if (select == "1" || select == "2" || select == "3")
                {
                    Console.Write("Enter a substring to search for: ");
                    string substring = Console.ReadLine();
                    if (select == "1")
                        simpleSerach(text, substring, out index, out countOfComparisons, out workTime);
                    if (select == "2")
                        KMP(text, substring, out index, out countOfComparisons, out workTime);
                    if (select == "3")
                        BM(text, substring, out index, out countOfComparisons, out workTime);
                    if (index != -1)
                        Console.WriteLine("Index of substring: " + index);
                    else
                        Console.WriteLine("There is no such substring.");
                    Console.WriteLine("Count of comparisons: " + countOfComparisons);
                    Console.WriteLine("Work time: " + workTime.TotalMilliseconds + " milliseconds\n");
                    countOfComparisons = 0;
                    workTime = TimeSpan.Parse("0");
                    index = 0;
                }
            }
        }
        public static void simpleSerach(string text, string substring, out int index, out ulong countOfComparisons, out TimeSpan workTime)
        {
            index = -1;
            countOfComparisons = 0;
            DateTime startTime = DateTime.Now;
            Console.WriteLine("\nSimple search");
            for (int i = 0; i < text.Length - substring.Length; i++)
            {
                for (int j = 0; substring[j] == text[i + j]; j++, countOfComparisons++)
                    if (j == substring.Length - 1)
                    {
                        index = i;
                        break;
                    }
                countOfComparisons++;
                if (index == i)
                    break;
            }
            DateTime endTime = DateTime.Now;
            workTime = endTime - startTime;
        }
        public static int[] createPrefix(string s)
        {
            int[] pi = new int[s.Length];
            int j = 0;
            pi[0] = 0;
            for (int i = 1; i < s.Length; i++)
            {
                while (j > 0 && s[j] != s[i])
                    j = pi[j - 1];
                if (s[j] == s[i])
                    j++;
                pi[i] = j;
            }
            return pi;
        }
        public static void KMP(string text, string substring, out int index, out ulong countOfComparisons, out TimeSpan workTime)
        {
            countOfComparisons = 0;
            DateTime startTime = DateTime.Now;
            Console.WriteLine("\nKMP search");
            int[] prefix = createPrefix(substring);
            int j = 0;
            index = -1;
            for (int i = 1; i <= text.Length; i++)
            {
                while (j > 0 && substring[j] != text[i - 1])
                {
                    j = prefix[j - 1];
                    countOfComparisons++;
                }
                    if (substring[j] == text[i - 1])
                {
                    j++;
                    countOfComparisons++;
                }
                if (j == substring.Length)
                {
                    index = i - substring.Length;
                    break;
                }
            }
            DateTime endTime = DateTime.Now;
            workTime = endTime - startTime;
        }
        public static int[] badCharactersTable(string substring)
        {
            int[] badShift = new int[256];
            for (int i = 0; i < 256; i++)
                badShift[i] = -1;
            for (int i = 0; i < substring.Length - 1; i++)
                badShift[(int)substring[i]] = i;
            return badShift;
        }
        public static int[] suffixesCreate(string substring)
        {
            int[] suffixes = new int[substring.Length];
            suffixes[substring.Length - 1] = substring.Length;
            int right = substring.Length - 1, left = 0;
            for (int i = right - 1; i >= 0; --i)
            {
                if (i > right && suffixes[i + substring.Length - 1 - left] < i - right)
                    suffixes[i] = suffixes[i + substring.Length - 1 - left];
                else if (i < right)
                    right = i;
                left = i;
                while (right >= 0 && substring[right] == substring[right + substring.Length - 1 - left])
                    right--;
                suffixes[i] = left - right;
            }
            return suffixes;
        }
        public static int[] goodSuffixTable(string substring)
        {
            int m = substring.Length;
            int[] suffixes = suffixesCreate(substring);
            int[] goodSuffixes = new int[substring.Length];
            for (int i = 0; i < substring.Length; i++)
                goodSuffixes[i] = substring.Length;
            for (int i = substring.Length - 1; i >= 0; i--)
                if (suffixes[i] == i + 1)
                    for (int j = 0; j < substring.Length - i - 1; j++)
                        if (goodSuffixes[j] == substring.Length)
                            goodSuffixes[j] = substring.Length - i - 1;
            for (int i = 0; i < substring.Length - 2; i++)
                goodSuffixes[substring.Length - 1 - suffixes[i]] = substring.Length - i - 1;
            return goodSuffixes;
        }
        public static void BM(string text, string substring, out int shift, out ulong countOfComparisons, out TimeSpan workTime)
        {
            countOfComparisons = 0;
            DateTime startTime = DateTime.Now, endTime;
            workTime = TimeSpan.MinValue;
            Console.WriteLine("\nBM search");
            if (substring.Length > text.Length)
                shift = -1;
            else
            {
                int[] badShift = badCharactersTable(substring);
                int[] goodSuffix = goodSuffixTable(substring);
                shift = 0;
                while (shift <= text.Length - substring.Length)
                {
                    int i;
                    for (i = substring.Length - 1; i >= 0 && substring[i] == text[i + shift]; i--, countOfComparisons++) ;
                    if (i < 0)
                    {
                        endTime = DateTime.Now;
                        workTime = endTime - startTime;
                        return;
                    }
                    shift += Math.Max(i - badShift[(int)text[shift + i]], goodSuffix[i]);
                }
                shift = -1;
                endTime = DateTime.Now;
                workTime = endTime - startTime;
            }
        }
    }
}
