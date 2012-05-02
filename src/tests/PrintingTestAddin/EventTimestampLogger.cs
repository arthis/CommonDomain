using System;
using NUnit.Core.Extensibility;
using NUnit.Core;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;


namespace NUnitTestLogger
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;

    
    class TestRecord
        {
            public string FullName { get; set; }
            public int ResultState { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string AssemblyName { get; set; }
            public string ComputerName { get; set; }
            public string BuildVersionNumber { get; set; }
            public DateTime RunDateTime { get; set; }
            public int Count { get; set; }
            public double Time { get; set; }


            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(FullName);
                sb.Append(",");
                sb.Append(StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.Append(",");
                sb.Append(EndTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.Append(",");
                sb.Append(Time.ToString());
                sb.Append(",");
                sb.Append(ResultState.ToString());
                return sb.ToString();
            }

            public string GetContainerName()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BuildVersionNumber);
                sb.Append("_");
                sb.Append(AssemblyName);
                sb.Append("_");
                sb.Append(RunDateTime.ToString("yyyyMMddHHmmss"));
                sb.Append("_");
                sb.Append(ComputerName);
                sb.Append("_");
                sb.Append(Count);

                return sb.ToString();
            }

            internal void StartRun(string name, int testCount)
            {
                RunDateTime = DateTime.UtcNow;
                Count = testCount;
                AssemblyName = GetAssemblyNameFromPath(name);
                BuildVersionNumber = GetAssemblyVersionFromPath(name);
                ComputerName = Environment.MachineName;
            }

            private string GetAssemblyVersionFromPath(string name)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                string assemblyNameFromPath = GetAssemblyNameFromPath(name);
                foreach (Assembly a in assemblies)
                {
                    string testAssemblyName = (a.GetName()).Name + ".dll";
                    if (String.Compare(testAssemblyName, assemblyNameFromPath, true) == 0)
                    {
                        return a.GetName().Version.ToString();
                    }
                }
                return "0.0.0000.0";
            }

            private string GetAssemblyNameFromPath(string name)
            {
                if (!String.IsNullOrEmpty(name))
                {
                    FileInfo fi = new FileInfo(name);
                    return fi.Name;
                }
                else
                {
                    return "unknown";
                }
            }

            internal string GetHeader()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("FullName");
                sb.Append(",");
                sb.Append("StartTime");
                sb.Append(",");
                sb.Append("EndTime");
                sb.Append(",");
                sb.Append("Duration");
                sb.Append(",");
                sb.Append("ResultState");
                return sb.ToString();
            }

            internal void StartTest(NUnit.Core.TestName testName)
            {
                StartTime = DateTime.UtcNow;
                FullName = testName.FullName;
            }

            internal void CompleteTest(NUnit.Core.TestResult result)
            {
                EndTime = DateTime.UtcNow;
                Time = result.Time;
                if (result.Executed)
                {
                    if (result.IsSuccess)
                    {
                        ResultState = 0; //Successfull assertions
                    }
                    if (result.IsFailure)
                    {
                        ResultState = 1; //Failed assertion
                    }
                    if (result.IsError)
                    {
                        ResultState = 2; //Error
                    }
                }
                else
                {
                    ResultState = -1; //Not run
                }
            }

            internal void CompleteTestWith(Exception exception)
            {
                EndTime = DateTime.UtcNow;
                Time = 0;
                ResultState = 2; //Error
            }
        }

    public class TestFormatter
    {
        public static void PrintSpec(TestResult result, TextWriter log)
        {
            var passed = result.IsSuccess ? "Passed" : "Failed";
            log.WriteLine(result.FullName.Replace('_', ' ') + " - " + passed);
            result.Test.
            //var on = result.GetOnResult();
            //if (on != null)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("On:");
            //    Console.WriteLine(on.ToString());
            //    Console.WriteLine();
            //}
            //if (result.Result != null)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Results with:");
            //    if (result.Result is Exception)
            //        Console.WriteLine(result.Result.GetType() + "\n" + ((Exception)result.Result).Message);
            //    else
            //        Console.WriteLine(result.Result.ToString());
            //    Console.WriteLine();
            //}

            //Console.WriteLine("Expectations:");
            //foreach (var expecation in result.Expectations)
            //{
            //    if (expecation.Passed)
            //        Console.WriteLine("\t" + expecation.Text + " " + (expecation.Passed ? "Passed" : "Failed"));
            //    else
            //        Console.WriteLine(expecation.Exception.Message);
            //}
            //if (result.Thrown != null)
            //{
            //    Console.WriteLine("Specification failed: " + result.Message);
            //    Console.WriteLine();
            //    Console.WriteLine(result.Thrown);
            //}
            //Console.WriteLine(new string('-', 80));
            //Console.WriteLine();
        }

    }
    

    [NUnitAddin(Description = "Event Timestamp Logger")]
    public class EventTimestampLogger : IAddin, EventListener
    {
        public bool Install(IExtensionHost host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
            if (listeners == null)
                return false;

            listeners.Install(this);
            return true;
        }

        TestRecord testRecord = new TestRecord();
        StringBuilder sb = new StringBuilder();


        #region EventListener Members

        public void RunStarted(string name, int testCount)
        {
            testRecord.StartRun(name, testCount);
            string logFileName = GetLogFileName(testRecord);
            LogHeader(logFileName, testRecord);
        }

        public void RunFinished(TestResult result)
        {
            return;
        }

        public void SuiteStarted(TestName testName)
        {
            return;
        }

        public void TestStarted(TestName testName)
        {
            testRecord.StartTest(testName);
        }

        public void TestFinished(TestResult result)
        {

            string fileName = "resulttest.txt";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + fileName;

            if (File.Exists(filePath))
                File.Delete(filePath);
            using (TextWriter writer = File.CreateText(filePath))
            {
                try
                {
                    ObjectDumper.Write(result, int.MaxValue, writer);
                    writer.WriteLine();
                    writer.WriteLine(new string('*',120));
                    writer.WriteLine();
                    TestFormatter.PrintSpec(result);
                }
                catch (Exception e)
                {
                    writer.Write(e.ToString());
                }
                
            }

            testRecord.CompleteTest(result);
            string logFileName = GetLogFileName(testRecord);
            LogResult(logFileName, testRecord);
        }

        public void SuiteFinished(TestResult result)
        {
            return;
        }

        public void RunFinished(Exception exception)
        {
            testRecord.CompleteTestWith(exception);
            string logFileName = GetLogFileName(testRecord);
            LogResult(logFileName, testRecord);
        }


        private string GetLogFileName(TestRecord testRecord)
        {
            if (testRecord != null)
            {
                string fileName = testRecord.GetContainerName() + ".csv";
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + fileName;
                return filePath;
            }
            else
            {
                return "testresultlog.csv";
            }
        }

        public void TestOutput(TestOutput testOutput)
        {
            return;
        }

        public void UnhandledException(Exception exception)
        {
            Debug.Write("UnhandledException " + exception);
        }

        private void LogHeader(string logFileName, TestRecord testRecord)
        {
            string message = testRecord.GetHeader();
            LogMessage(logFileName, message);
        }

        private void LogResult(string logFileName, TestRecord testRecord)
        {
            string message = testRecord.ToString();
            LogMessage(logFileName, message);
        }

        private void LogMessage(string logFileName, string message)
        {
            using (StreamWriter sw = File.AppendText(logFileName))
            {
                sw.WriteLine(message);
            }
        }
        #endregion
    }
}