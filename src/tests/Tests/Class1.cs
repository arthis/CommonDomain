using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;
using NUnit.Framework;
using example1;
using example1.command;
using example1.events;
using example1.handlers;

namespace Tests
{
    [TestFixture]
    public abstract class BaseClass<TCommand> where TCommand : class, ICommand
    {
        
        public Guid Id { get; set; }
        protected abstract IEnumerable<IEvent> Given();
        protected abstract TCommand When();
        protected abstract IEnumerable<IEvent> Expect();
        protected Exception Caught;
        public abstract CommandHandler<TCommand> OnHandle(IRepository repository);
        FakeRepository fakeRepository = new FakeRepository();


        [SetUp]
        public void Setup()
        {
            var commandHandler = OnHandle(fakeRepository);
            fakeRepository.CommittedEvents = Given();
            var command = When();
            var expect = Expect();

            try
            {
                commandHandler.Execute(command);
                var actual = fakeRepository.CommittedEvents.ToList();

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
        private readonly IConstructAggregates _factory = new AggregateFactory();
        public IEnumerable<IEvent> CommittedEvents { get; set; }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            var ar = this._factory.Build(typeof(TAggregate), id, null) as TAggregate;
            if (ar!=null && CommittedEvents.Any())
            {
                foreach (var committedEvent in CommittedEvents)
                {
                    ar.ApplyEvent(committedEvent);   
                }
            }
            return ar;
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


    public class when_creating_a_new_inventory_item : BaseClass<CreateInventoryItem>
    {
        private Guid _Id = Guid.NewGuid();
        private string _Name = "test";

        public override CommandHandler<CreateInventoryItem> OnHandle(IRepository repository)
        {
            return  new CreateInventoryItemHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override CreateInventoryItem When()
        {
            return new CreateInventoryItem()
            {
                Id = _Id,
                Name = _Name
            };
        }

        protected override IEnumerable<IEvent> Expect()
        {
            yield return new InventoryItemCreatedAdded()
                             {
                                 Id = _Id,
                                 Name = _Name
                             };
        }

        [Test]
        public void does_not_throw_an_Exception()
        {
            Assert.IsNull(Caught);
        }


    }



}
