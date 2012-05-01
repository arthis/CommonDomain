using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using example1.command;
using CommonDomain.Persistence;
using System.Diagnostics.Contracts;
using example1.domain;

namespace example1.handlers
{
    public class CreateInventoryItemHandler : CommandHandler<CreateInventoryItem>
    {
        public CreateInventoryItemHandler(IRepository repository)
            : base(repository)
        {
        }

        public override void Execute(CreateInventoryItem cmd)
        {
            Contract.Requires<ArgumentNullException>(cmd != null);

            var commitId = Guid.NewGuid();

            var entity = new InventoryItem(cmd.Id, cmd.Name);

            Repository.Save(entity, commitId);
        }
    }
}
