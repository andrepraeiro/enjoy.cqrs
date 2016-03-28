﻿using System;
using System.Collections.Generic;
using MyCQRS.Events;

namespace MyCQRS.Domain
{
    public class Aggregate
    {
        private readonly List<IEvent> _uncommitedEvents = new List<IEvent>();
        private readonly Route<IEvent> _routeEvents = new Route<IEvent>();

        public IReadOnlyCollection<IEvent> UncommitedEvents => _uncommitedEvents.AsReadOnly();
        public int Version { get; protected set; } = -1;

        protected void On<T>(Action<T> action)
            where T : class 
        {
            _routeEvents.Add(typeof(T), o => action(o as T));
        }

        protected void Raise(IEvent @event)
        {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

        public void ApplyEvent(IEvent @event)
        {
            _routeEvents.Handle(@event);

            Version++;
        }

        public void ClearUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }
    }
}