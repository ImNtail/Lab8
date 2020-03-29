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
            string text = String.Empty;
            using (StreamReader readText = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "text.txt")))
                text = readText.ReadToEnd();
            Console.Write("Enter a substring to search for: ");
            string substring = Console.ReadLine();
            int index;
            KMP(text, substring, out index);
            if (index != 0)
                Console.WriteLine("\nIndex of substring: " + index);
            else
                Console.WriteLine("\nThere is no such substring");
        }
        public static void simpleSerach()
        {

        }
        public static int[] createPrefix(string s)
        {
            int[] pi = new int[s.Length];
            int j = 0;
            pi[0] = 0;
            Console.Write(pi[0] + " ");
            for (int i = 1; i < s.Length; i++)
            {
                while (j > 0 && s[j] != s[i])
                    j = pi[j-1];
                if (s[j] == s[i])
                    j++;
                pi[i] = j;
                Console.Write(pi[i] + " ");
            }
            return pi;
        }
        public static void KMP(string text, string substring, out int index)
        {
            int[] prefix = createPrefix(substring);
            int j = 0;
            index = -1;
            for (int i = 1; i <= text.Length; i++)
            {
                while (j > 0 && substring[j] != text[i - 1])
                    j = prefix[j - 1];
                if (substring[j] == text[i - 1])
                    j++;
                if (j == substring.Length)
                    index = i - substring.Length;
            }
        }
        public static void BM()
        {

        }
    }
}
