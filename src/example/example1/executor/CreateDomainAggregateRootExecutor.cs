using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using example1.command;
using CommonDomain.Persistence;
using System.Diagnostics.Contracts;
using example1.domain;

namespace example1.executor
{
    public class CreateDomainAggregateRootExecutor
    {
        public static void Execute(IRepository repository, CreateDomainAggregateRoot cmd)
        {
            Contract.Requires<ArgumentNullException>(repository != null);
            Contract.Requires<ArgumentNullException>(cmd != null);

            Guid commitId = Guid.NewGuid();

            var entity = new MyDomainAggregateRoot(cmd.Id, cmd.Name);

            repository.Save(entity,commitId);
        }
    }
}
