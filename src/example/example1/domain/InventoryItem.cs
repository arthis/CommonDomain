using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Core;
using example1.events;

namespace example1.domain
{
    public class InventoryItem : AggregateBase
    {

        public InventoryItem(Guid id, string name)
        {
            var evt = new InventoryItemCreatedAdded()
            {
                Id = id,
                Name = name
            };

            RaiseEvent(evt);
        }

        public void Apply(InventoryItemCreatedAdded e)
        {
            Id = e.Id;
        }
    }
}
