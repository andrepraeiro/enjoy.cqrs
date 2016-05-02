﻿using System;
using System.Collections.Generic;
using System.Linq;
using EnjoyCQRS.Events;

namespace EnjoyCQRS.EventSource
{
    public abstract class Aggregate
    {
        private readonly List<IDomainEvent> _uncommitedEvents = new List<IDomainEvent>();
        private readonly Route<IDomainEvent> _routeEvents = new Route<IDomainEvent>();

        /// <summary>
        /// Collection of <see cref="IDomainEvent"/> that contains uncommited events.
        /// All events that not persisted yet should be here.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> UncommitedEvents => _uncommitedEvents.AsReadOnly();
        
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Current version of the Aggregate.
        /// </summary>
        public int Version { get; protected set; }

        /// <summary>
        /// This version is calculated based on Uncommited events.
        /// </summary>
        public int EventVersion { get; protected set; }

        protected void On<T>(Action<T> action)
            where T : class 
        {
            _routeEvents.Add(typeof(T), o => action(o as T));
        }

        protected void Raise(IDomainEvent @event)
        {
            ApplyEvent(@event, true);
        }

        /// <summary>
        /// Apply the event in Aggregate.
        /// The last event applied is the current state of the Aggregate.
        /// </summary>
        /// <param name="event"></param>
        private void ApplyEvent(IDomainEvent @event, bool isNewEvent)
        {
            _routeEvents.Handle(@event);

            if (isNewEvent)
            {
                var eventVersion = EventVersion + 1;

                if (@event is DomainEvent)
                {
                    ((DomainEvent) @event).Version = eventVersion;
                }

                _uncommitedEvents.Add(@event);

                EventVersion = eventVersion;
            }
        }

        /// <summary>
        /// Clear the collection of events that uncommited.
        /// </summary>
        public void ClearUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }

        /// <summary>
        /// Load the events in the Aggregate.
        /// </summary>
        /// <param name="events"></param>
        public void LoadFromHistory(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyEvent(@event, false);
            }

            EventVersion = events.LastOrDefault()?.Version ?? 0;

            Version = EventVersion;
        }
    }
}