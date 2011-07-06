using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.MessagePatterns;

namespace GettingStartedWithRabbitMQ.Example4Routing
{
    public class Consumer : IConnectToRabbitMQ
    {
        protected bool isConsuming;
        protected string QueueName;

        // used to pass messages back to UI for processing
        public delegate void onReceiveMessage(byte[] message);
        public event onReceiveMessage onMessageReceived;

        public Consumer(string server, string exchange, string exchangeType) : base(server, exchange, exchangeType)
        {
        }

        //internal delegate to run the consuming queue on a seperate thread
        private delegate void ConsumeDelegate();

        public void StartConsuming()
        {

                Model.BasicQos(0, 1, false);
                QueueName = Model.QueueDeclare();
               // Model.QueueBind(QueueName, ExchangeName, "");
                if (ExchangeTypeName == ExchangeType.Fanout)
                    AddBinding("");//fanout has default binding
                isConsuming = true;
                ConsumeDelegate c = new ConsumeDelegate(Consume);
                c.BeginInvoke(null, null);
                
             

        }

        public void AddBinding(string routingKey)
        {
            Model.QueueBind(QueueName, ExchangeName, routingKey);
        }

        public void RemoveBinding(string routingKey)
        {
            Model.QueueUnbind(QueueName, ExchangeName, routingKey,null);
        }

        protected Subscription mSubscription { get; set; }

        private void Consume()
        {
            bool autoAck = false;

            //create a subscription
            mSubscription = new Subscription(Model, QueueName, autoAck);
           
            while (isConsuming)
            {
                BasicDeliverEventArgs e = mSubscription.Next();
                byte[] body = e.Body;
                onMessageReceived(body);
                mSubscription.Ack(e);

            }
        }
        
        public void Dispose()
        {
            isConsuming = false;
            base.Dispose();
        }
    }
}
