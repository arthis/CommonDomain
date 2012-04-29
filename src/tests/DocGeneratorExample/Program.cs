using System;
using Simple.Testing.Framework;
using System.Diagnostics;

namespace DocGeneratorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleRunner.RunAllInAssembly(typeof(Program).Assembly).ForEach(PrintSpecOutput);
        }

        private static void PrintSpecConsole(RunResult result)
        {
            var passed = result.Passed ? "Passed" : "Failed";
            Console.WriteLine(result.Name.Replace('_', ' ') + " - " +passed);
            var on = result.GetOnResult();
                if(on != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("On:");
                    Console.WriteLine(on.ToString());
                    Console.WriteLine();
                }
                if (result.Result != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Results with:");
                    if(result.Result is Exception)
                        Console.WriteLine(result.Result.GetType() + "\n" + ((Exception) result.Result).Message );
                    else
                        Console.WriteLine(result.Result.ToString());
                    Console.WriteLine();
                }
            
            Console.WriteLine("Expectations:");
            foreach(var expecation in result.Expectations)
            {
                if(expecation.Passed)
                    Console.WriteLine("\t" + expecation.Text + " " + (expecation.Passed ? "Passed" : "Failed"));
                else
                    Console.WriteLine(expecation.Exception.Message);
            }
            if(result.Thrown != null)
            {
                Console.WriteLine("Specification failed: " + result.Message);
                Console.WriteLine();
                Console.WriteLine(result.Thrown);
            }
            Console.WriteLine(new string('-', 80));
            Console.WriteLine();
        }

        private static void PrintSpecOutput(RunResult result)
        {
            var passed = result.Passed ? "Passed" : "Failed";
            Debug.WriteLine(result.Name.Replace('_', ' ') + " - " + passed);
            var on = result.GetOnResult();
            if (on != null)
            {
                Debug.WriteLine("");
                Debug.WriteLine("On:");
                Debug.WriteLine(on.ToString());
                Debug.WriteLine("");
            }
            if (result.Result != null)
            {
                Debug.WriteLine("");
                Debug.WriteLine("Results with:");
                if (result.Result is Exception)
                    Debug.WriteLine(result.Result.GetType() + "\n" + ((Exception)result.Result).Message);
                else
                    Debug.WriteLine(result.Result.ToString());
                Debug.WriteLine("");
            }

            Debug.WriteLine("Expectations:");
            foreach (var expecation in result.Expectations)
            {
                if (expecation.Passed)
                    Debug.WriteLine("\t" + expecation.Text + " " + (expecation.Passed ? "Passed" : "Failed"));
                else
                    Debug.WriteLine(expecation.Exception.Message);
            }
            if (result.Thrown != null)
            {
                Debug.WriteLine("Specification failed: " + result.Message);
                Debug.WriteLine("");
                Debug.WriteLine(result.Thrown);
            }
            Debug.WriteLine(new string('-', 80));
            Debug.WriteLine("");
        }
    }
}
