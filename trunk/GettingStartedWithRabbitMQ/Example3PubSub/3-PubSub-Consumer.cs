using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GettingStartedWithRabbitMQ.Example3PubSub;
using RabbitMQ.Client;

namespace GettingStartedWithRabbitMQ
{
    public partial class PubSub_Consumer : Form
    {
        public string HOST_NAME = "localhost";
        public string EXCHANGE_NAME = "logs";

        private Consumer consumer;
        public PubSub_Consumer()
        {
            InitializeComponent();
            //create the consumer
            consumer = new Consumer(HOST_NAME, EXCHANGE_NAME, ExchangeType.Fanout);

            //connect to RabbitMQ
            if (!consumer.ConnectToRabbitMQ())
            {
                //Show a basic error if we fail
                MessageBox.Show("Could not connect to Broker");
            }

            //Register for message event
            consumer.onMessageReceived += handleMessage;

            //Start consuming
            consumer.StartConsuming();
        }

        //delegate to post to UI thread
        private delegate void showMessageDelegate(string message);

        //Callback for message receive
        public void handleMessage(byte[] message)
        {
      
            showMessageDelegate s = new showMessageDelegate(richTextBox1.AppendText);

            this.Invoke(s, System.Text.Encoding.UTF8.GetString(message) + Environment.NewLine);
        }
    }
}
