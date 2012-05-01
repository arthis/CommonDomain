using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        [TearDown]
        public void Cleanup()
        {
            TestFormatter.PrintSpec();

        }
    }

    public class RunResult
    {
        public bool Passed;
        public string Message;
        public Exception Thrown;
        public string SpecificationName;
        public List<ExpectationResult> Expectations = new List<ExpectationResult>();
        public MemberInfo FoundOnMemberInfo;
        public Delegate On;
        public object Result;

        public object GetOnResult()
        {
            return On.DynamicInvoke();
        }

        public string Name
        {
            get { return SpecificationName ?? FoundOnMemberInfo.Name; }
        }

        internal void MarkFailure(string message, Exception thrown)
        {
            Thrown = thrown;
            Message = message;
            Passed = false;
        }
    }


    public class ExpectationResult
    {
        public bool Passed;
        public string Text;
        public Exception Exception;
        public Expression OriginalExpression;
    }

    public class TestFormatter
    {
        public static void PrintSpec(RunResult result)
        {
            var passed = result.Passed ? "Passed" : "Failed";
            Console.WriteLine(result.Name.Replace('_', ' ') + " - " + passed);
            var on = result.GetOnResult();
            if (on != null)
            {
                Console.WriteLine();
                Console.WriteLine("On:");
                Console.WriteLine(on.ToString());
                Console.WriteLine();
            }
            if (result.Result != null)
            {
                Console.WriteLine();
                Console.WriteLine("Results with:");
                if (result.Result is Exception)
                    Console.WriteLine(result.Result.GetType() + "\n" + ((Exception)result.Result).Message);
                else
                    Console.WriteLine(result.Result.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("Expectations:");
            foreach (var expecation in result.Expectations)
            {
                if (expecation.Passed)
                    Console.WriteLine("\t" + expecation.Text + " " + (expecation.Passed ? "Passed" : "Failed"));
                else
                    Console.WriteLine(expecation.Exception.Message);
            }
            if (result.Thrown != null)
            {
                Console.WriteLine("Specification failed: " + result.Message);
                Console.WriteLine();
                Console.WriteLine(result.Thrown);
            }
            Console.WriteLine(new string('-', 80));
            Console.WriteLine();
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
        public void Exception()
        {
            Assert.IsNull(Caught);
        }


    }
}
