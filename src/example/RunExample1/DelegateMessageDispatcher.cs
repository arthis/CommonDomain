﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventStore;
using EventStore.Dispatcher;


namespace RunExample1
{
    public class DelegateMessageDispatcher : IDispatchCommits
    {
        private readonly Action<Commit> dispatch;

        public DelegateMessageDispatcher(Action<Commit> dispatch)
        {
            this.dispatch = dispatch;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // no op
        }

        public virtual void Dispatch(Commit commit)
        {
            this.dispatch(commit);
        }
    }
}
