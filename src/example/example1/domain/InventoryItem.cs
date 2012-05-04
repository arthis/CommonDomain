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
        private string _name;

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

        public string ToDescription()
        {
            return string.Format("Inventory Item {0}, {1}", Id, _name);
        }
    }
}
