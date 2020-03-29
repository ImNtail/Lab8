using System;
using System.IO;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            int select;
            Console.Write("Choose the task from 1 to 2: ");
            select = int.Parse(Console.ReadLine());
            Console.WriteLine();
            switch (select)
            {
                case 1:
                    Console.WriteLine("The first task: \n");
                    First.Execute();
                    break;
                case 2:
                    Console.WriteLine("The second task: \n");
                    Second.Execute();
                    break;
                default:
                    break;
            }
        }
    }
}