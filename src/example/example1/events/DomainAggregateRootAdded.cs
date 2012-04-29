﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;

namespace example1.events
{
	public class DomainAggregateRootAdded :IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
	
}