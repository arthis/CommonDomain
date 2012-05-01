using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;

namespace example1.command
{
    public class CreateInventoryItem :ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
