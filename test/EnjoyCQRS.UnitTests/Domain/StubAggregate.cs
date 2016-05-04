﻿using System;
using EnjoyCQRS.EventSource;
using EnjoyCQRS.UnitTests.Domain.Events;

namespace EnjoyCQRS.UnitTests.Domain
{
    public class StubAggregate : Aggregate
    {
        public string Name { get; private set; }
        
        private StubAggregate(Guid newGuid)
        {
            Emit(new TestAggregateCreatedEvent(newGuid));
        }

        public StubAggregate()
        {
        }

        public static StubAggregate Create()
        {
            return new StubAggregate(Guid.NewGuid());
        }
        
        public void DoSomething(string name)
        {
            Emit(new SomeEvent(Id, name));
        }

        public void DoSomethingWithoutEvent()
        {
            Emit(new NotRegisteredEvent(Id));
        }

        protected override void RegisterEvents()
        {
            SubscribeTo<SomeEvent>(x => { Name = x.Name; });
            SubscribeTo<TestAggregateCreatedEvent>(x => { Id = x.AggregateId; });
        }
    }
}