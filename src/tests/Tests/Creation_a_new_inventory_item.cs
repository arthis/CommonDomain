using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;
using NUnit.Framework;
using example1;
using example1.command;
using example1.events;
using example1.handlers;

namespace Tests
{
    public class Creation_of_a_new_inventory_item : BaseClass<CreateInventoryItem>
    {
        private Guid _Id = Guid.NewGuid();
        private string _Name = "test";

        protected override CommandHandler<CreateInventoryItem> OnHandle(IRepository repository)
        {
            return  new CreateInventoryItemHandler(repository);
        }

        public override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        public override CreateInventoryItem When()
        {
            return new CreateInventoryItem()
            {
                Id = _Id,
                Name = _Name
            };
        }

        public override IEnumerable<IEvent> Expect()
        {
            yield return new InventoryItemCreatedAdded()
                             {
                                 Id = _Id,
                                 Name = _Name
                             };
        }

        [Test]
        public void It_does_not_throw_an_Exception()
        {
            Assert.IsNull(Caught);
        }


    }



}
