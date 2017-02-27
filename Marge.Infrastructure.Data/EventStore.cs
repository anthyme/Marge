using System;
using System.Collections.Generic;
using Marge.Common;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;

namespace Marge.Infrastructure.Data
{
    public class EventStore : IEventStore
    {
        private readonly IStoreEvents store =
            Wireup.Init()
               .LogToOutputWindow()
               .UsingInMemoryPersistence()
               .UsingSqlPersistence("MargeDb")
               .WithDialect(new MsSqlDialect())               
               .InitializeStorageEngine()
               .UsingJsonSerialization()
               .LogToOutputWindow()
               .Build();


        public IEventStoreStream CreateStream(Guid id)
        {
            return new EventStoreStream(id, store.CreateStream(id));
        }

        public IEventStoreStream OpenStream(Guid id)
        {
            return new EventStoreStream(id, store.OpenStream(id));
        }
    }
}
