﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;

namespace example1
{
    public abstract class CommandHandler<TCommand> where TCommand:class ,ICommand
    {
        protected IRepository Repository;
        public abstract void Execute(TCommand command);

        public CommandHandler(IRepository repository)
        {
            Contract.Requires<ArgumentNullException>(repository!=null);

            Repository = repository;
        }
    }


}
