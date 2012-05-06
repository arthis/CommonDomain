using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;
using example1.command;
using example1.handlers;

namespace RunExample1
{
    public class CommandHandlerService
    {
        private readonly Dictionary<Type, Action<Object>> _handlers = new Dictionary<Type, Action<Object>>();



        public void InitHandlers(IRepository repositoryEvent)
        {
            _handlers.Add(typeof(CreateInventoryItem),
                          (cmd) => new CreateInventoryItemHandler(repositoryEvent).Execute((CreateInventoryItem)cmd));
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
