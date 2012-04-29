using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;
using NUnit.Framework;
using example1.command;
using example1.events;

namespace Tests
{
    [TestFixture]
    public abstract class BaseClass<TCommand> where TCommand : ICommand
    {
        public Guid Id { get; set; }
        protected abstract IEnumerable<IEvent> Given();
        protected abstract TCommand When();
        protected abstract IEnumerable<IEvent> Expect();
        protected Exception Caught;
        protected FakeRepository repository;


        [SetUp]
        public void Setup()
        {
            ICommandHandler commandHandler = new FakeCommandHandler();
            var command = When();
            var expect = Expect();

            try
            {

                commandHandler.Execute(command);
                var actual = repository.CommittedEvents;

                Assert.IsNotNull(actual);
                Assert.IsTrue(expect.SequenceEqual(actual));
            }
            catch (Exception e)
            {
                Caught = e;
            }
        }
    }

    public class FakeRepository : IRepository
    {
        public IEnumerable<IEvent> CommittedEvents { get; set; }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            throw new NotImplementedException();
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            throw new NotImplementedException();
        }

        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            CommittedEvents = aggregate.GetUncommittedEvents();
        }

        
    }


    public class FakeCommandHandler : ICommandHandler
    {
        public void Execute(ICommand command)
        {
            throw new NotImplementedException();
        }
    }


    public class when_creating_a_new_domain_aggregate_root : BaseClass<CreateDomainAggregateRoot>
    {
        private Guid _Id = Guid.NewGuid();
        private string _Name = "test";

        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override CreateDomainAggregateRoot When()
        {
            return new CreateDomainAggregateRoot()
            {
                Id = _Id,
                Name = _Name
            };
        }

        protected override IEnumerable<IEvent> Expect()
        {
            yield return new DomainAggregateRootAdded()
                             {
                                 Id = _Id,
                                 Name = _Name
                             };
        }


        [Test]
        public void Exception()
        {
            Assert.IsNull(Caught);
        }


    }
}
