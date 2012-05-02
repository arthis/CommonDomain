//using System;
//using System.Collections.Generic;
//using System.Text;
//using NUnit.Core;
//using NUnit.Core.Extensibility;
//using System.IO;

//namespace PrintingTestAddin
//{
//    [NUnitAddin(Type = ExtensionType.Core, Name = "Printing Test Addin", Description = "print test results")]
//    public class PrintingTestAddin : IAddin, EventListener
//    {
//        #region IAddin Members

//        public bool Install(IExtensionHost host)
//        {
//            IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
//            listeners.Install(this);
//            return true;
//        }

//        #endregion

//        #region EventListener Members

//        public void RunStarted(
//                string name, int testCount)
//        {
//            //Do Set up logic Here
//        }
//        public void RunFinished(Exception exception)
//        { }

//        public void RunFinished(TestResult result)
//        {
//        }

//        public void SuiteFinished(TestResult result)
//        { }

//        public void SuiteStarted(TestName testName)
//        { }

//        public void TestFinished(TestResult result)
//        {
//            string file = "c:\resulttest.txt";

//            File.CreateText(file);

//            //if (File.Exists(file))
//            //    File.Delete(file);
//            //using (var writer = File.CreateText(file))
//            //{
//            //    ObjectDumper.Write(result, 0, writer);
//            //}

//        }

//        public void TestOutput(TestOutput testOutput)
//        {
//        }

//        public void TestStarted(TestName testName)
//        { }

//        public void UnhandledException(Exception exception)
//        { }

//        #endregion
//    }


//}
