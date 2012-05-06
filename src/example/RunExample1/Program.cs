using System;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence.EventStore;
using EventStore;
using EventStore.Serialization;
using example1.command;
using example1;


namespace RunExample1
{
    public class Program
    {
        private static CommandHandlerService _commandHandlerService;

        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        private static IStoreEvents WireupEventStore()
        {
            return Wireup.Init()
               .LogToOutputWindow()
               .UsingMongoPersistence("EventStore", new DocumentObjectSerializer())
                   .InitializeStorageEngine()
                   .UsingJsonSerialization()
                       .Compress()
                       .EncryptWith(EncryptionKey)
               .HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
               .UsingSynchronousDispatchScheduler()
                   .DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
               .Build();

        }

        public static void Init()
        {
            var storeEvents = WireupEventStore();
            var aggregateFactory = new AggregateFactory();
            var conflictDetector = new ConflictDetector();
            var eventRepository = new EventStoreRepository(storeEvents, aggregateFactory, conflictDetector);

            _commandHandlerService = new CommandHandlerService();
            _commandHandlerService.InitHandlers(eventRepository);
        }

        static void Main(string[] args)
        {
            Init();

            Guid id = Guid.NewGuid();

            var command = new CreateInventoryItem()
            {
                Id = id,
                Name = "testname"
            };


            _commandHandlerService.Execute(command);
        }

        private static void DispatchCommit(Commit commit)
        {
            // This is where we'd hook into our messaging infrastructure, such as NServiceBus,
            // MassTransit, WCF, or some other communications infrastructure.
            // This can be a class as well--just implement IDispatchCommits.
            try
            {
                foreach (var @event in commit.Events)
                    Console.WriteLine("*************************");
            }
            catch (Exception)
            {
                Console.WriteLine("*************************");
            }
        }
    }
}
