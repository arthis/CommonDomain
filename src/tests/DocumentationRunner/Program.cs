using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonDomain;
using NUnit.Framework;
using Tests;

namespace DocumentationRunner
{


    class Program
    {

        public static string GetDocumentation(Type type)
        {
            var sb = new StringBuilder();
            var index = 0;

            var instance = Activator.CreateInstance(type);

            MethodInfo[] methodInfos = type.GetMethods();

            var given = (IEnumerable<IEvent>)methodInfos.Single(x => x.Name == "Given").Invoke(instance, null);

            if (given.Any())
            {
                sb.AppendLine("Given that :");
                foreach (var evt in given)
                {
                    if (index == 0)
                        sb.AppendLine("   " + evt.ToDescription());
                    else
                        sb.AppendLine("   and " + evt.ToDescription());
                    index++;
                }
            }

            

            var when = (ICommand)methodInfos.Single(x => x.Name == "When").Invoke(instance, null);
            sb.AppendLine("When " + when.ToDescription());


            var expect = (IEnumerable<IEvent>)methodInfos.Single(x => x.Name == "Expect").Invoke(instance, null);
            var expectedTest = methodInfos.Where(x => Attribute.GetCustomAttribute(x, typeof(TestAttribute), false) is TestAttribute);

            index = 0;
            if (expect.Any() || expectedTest.Any())
            {
                sb.AppendLine("Then");
                foreach (var evt in expect)
                {
                    if (index == 0)
                        sb.AppendLine("   " + evt.ToDescription());
                    else
                        sb.AppendLine("   and " + evt.ToDescription());
                    index++;
                }

                foreach (var mtd in expectedTest)
                {
                    var nameTest = mtd.Name.Replace('_', ' ');
                    if (index == 0)
                        sb.AppendLine("   " + nameTest);
                    else
                        sb.AppendLine("   and " + nameTest);
                }
            }
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            Assembly assembly = typeof(Creation_of_a_new_inventory_item).Assembly;
            var typeBaseClass = typeof(BaseClass<ICommand>);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType.Name == typeBaseClass.Name)
                {
                    Console.Write(GetDocumentation(type));
                }
            }

            Console.Read();
        }
    }
}
