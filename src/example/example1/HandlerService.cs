using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using CommonDomain;
using example1.handlers;
using example1.command;
using CommonDomain.Persistence.EventStore;
using CommonDomain.Core;
using CommonDomain.Persistence;

namespace example1
{
    public class HandlerService 
    {
        private readonly Dictionary<Type, Action<Object>> _handlers = new Dictionary<Type, Action<Object>>();

        

        public void InitHandlers(IRepository repositoryEvent)
        {
            //_handlers.Add(typeof(CreateInventoryItem),
            //              (cmd) => new CreateInventoryItemHandler().Execute(repositoryEvent, (CreateInventoryItem)cmd));
        }

        public void Execute(ICommand command)
        {
            Contract.Requires<ArgumentNullException>(command != null);

            var type = command.GetType();
            if (_handlers.ContainsKey(type))
                _handlers[type](command);

        }

       

        
    }

    
}
