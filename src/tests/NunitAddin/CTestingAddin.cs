using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NunitAddin
{
     [NUnitAddin(Type = ExtensionType.Core, Name = "My Add In", Description = "A Demo for Run Setup Code")]
    public class CTestingAddin : IAddin, EventListener
    {
        #region IAddin Members

        public bool Install(IExtensionHost host)
        {
            IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
            if (listeners == null)
                return false;

            listeners.Install(this);
            return true;
        }

        #endregion

        #region EventListener Members

        public void RunStarted(
                string name, int testCount)
        {
            //Do Set up logic Here
        }
        public void RunFinished(Exception exception)
        { }

        public void RunFinished(TestResult result)
        {
            
        }

        public void SuiteFinished(TestResult result)
        { }

        public void SuiteStarted(TestName testName)
        { }

        public void TestFinished(TestResult result)
        { }

        public void TestOutput(TestOutput testOutput)
        { }

        public void TestStarted(TestName testName)
        { }

        public void UnhandledException(Exception exception)
        { }

        #endregion
    }


}
