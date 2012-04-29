using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDomain
{
    public interface ICommandHandler
    {
        void Execute(ICommand command);
    }
}
