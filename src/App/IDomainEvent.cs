using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public interface IDomainEvent
    {
    }

    public interface IBus
    {
        void Send(string message);
    }

    public class Bus : IBus
    {
        public void Send(string message)
        {
            Console.WriteLine($"Message Sent: '{message}'");
        }
    }

    public class MessageBus
    {
        private readonly IBus _bus;

        public MessageBus(IBus bus)
        {
            _bus = bus;
        }

        public void SendEmailChangedMessage(long studentId, string newEmail)
        {
            _bus.Send("Type: STUDENT_EMAIL_CHANGED; " +
               $"Id: {studentId}; " +
               $"NewEmail: {newEmail}");
        }
    }

    public sealed class EventDispatcher
    {
        private readonly MessageBus _messageBus;

        public EventDispatcher(MessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Dispatch(IEnumerable<IDomainEvent> events)
        {
            foreach (IDomainEvent ev in events)
            {
                Dispatch(ev);
            }
        }

        private void Dispatch(IDomainEvent ev)
        {
            switch (ev)
            {
                case StudentEmailChangedEvent emailChangedEvent:
                    _messageBus.SendEmailChangedMessage(
                        emailChangedEvent.StudentId,
                        emailChangedEvent.NewEmail);
                    break;

                // new domain events go here

                default:
                    throw new Exception($"Unknown event type: '{ev.GetType()}'");
            }
        }
    }
}
