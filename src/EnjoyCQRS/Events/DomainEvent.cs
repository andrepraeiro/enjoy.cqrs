﻿// The MIT License (MIT)
// 
// Copyright (c) 2016 Nelson Corrêa V. Júnior
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;

namespace EnjoyCQRS.Events
{
    /// <summary>
    /// Used to represent an Domain event.
    /// The domain event are things that have value for your domain.
    /// They are raised when occur changes on the aggregate root.
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Domain Event Unique identifier.
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// Aggregate Unique identifier.
        /// </summary>
        public Guid AggregateId { get; }

        /// <summary>
        /// Event version.
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        /// Construct the domain event.
        /// </summary>
        /// <param name="aggregateId"></param>
        protected DomainEvent(Guid aggregateId)
        {
            Id = Guid.NewGuid();

            AggregateId = aggregateId;
        }
    }
}