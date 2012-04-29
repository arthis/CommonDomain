using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Testing.ClientFramework;

namespace DocGeneratorExample
{
    public class Example1Specs
    {

        public Specification when_constructing_an_account = new ConstructorSpecification<Account>()
        {
            When = () => new Account("Jane Smith", 17),
            Expect =
                {
                    account => account.AccountHolderName == "Jane Smith",
                    account => account.UniqueIdentifier == 17,
                    account => account.CurrentBalance == new Money(0m),
                    account => account.Transactions.Count() == 0
                }
        };
    }
}
