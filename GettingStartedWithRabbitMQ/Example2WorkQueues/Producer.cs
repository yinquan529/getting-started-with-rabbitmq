using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace GettingStartedWithRabbitMQ.Example2WorkQueues
{
    public class Producer : IDisposable
    {

        protected IModel Model;
        protected IConnection Connection;
        protected string QueueName;

        public Producer(string hostName, string queueName)
        {
            QueueName = queueName;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            bool durable = true;
            Model.QueueDeclare(QueueName, durable, false, false, null);
        }

        public void SendMessage(byte[] message)
        {
            IBasicProperties basicProperties = Model.CreateBasicProperties();
            basicProperties.SetPersistent(true);
            Model.BasicPublish("", QueueName, basicProperties, message);
        }

        public void Dispose()
        {
            if (Connection != null)
                Connection.Close();
            if (Model != null)
                Model.Abort();
        }
    }
}
