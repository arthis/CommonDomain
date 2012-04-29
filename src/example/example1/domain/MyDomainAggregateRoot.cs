using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Core;
using example1.events;

namespace example1.domain
{
    public class MyDomainAggregateRoot : AggregateBase
    {

        public MyDomainAggregateRoot(Guid id, string name)
        {
            var evt = new DomainAggregateRootAdded()
            {
                Id = id,
                Name = name
            };

            RaiseEvent(evt);
        }

        public void Apply(DomainAggregateRootAdded e)
        {
            Id = e.Id;
        }
    }
}
