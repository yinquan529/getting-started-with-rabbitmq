using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace GettingStartedWithRabbitMQ.Example3PubSub
{
    public class Producer : IConnectToRabbitMQ
    {
        public Producer(string server, string exchange, string exchangeType) : base(server, exchange, exchangeType)
        {
        }

        public void SendMessage(byte[] message)
        {
            IBasicProperties basicProperties = Model.CreateBasicProperties();
            basicProperties.SetPersistent(true);
            Model.BasicPublish(ExchangeName, "", basicProperties, message);
        }
    }
}
