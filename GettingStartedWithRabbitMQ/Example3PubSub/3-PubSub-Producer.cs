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
    public partial class PubSub_Producer : Form
    {
        public string HOST_NAME = "localhost";
        public string EXCHANGE_NAME = "logs";

        private Producer producer;

        //delegate to show messages on the UI thread
        private delegate void showMessageDelegate(string message);

        public PubSub_Producer()
        {
            InitializeComponent();

            //Declare the producer
            producer = new Producer(HOST_NAME, EXCHANGE_NAME, ExchangeType.Fanout);

            //connect to RabbitMQ
            if(!producer.ConnectToRabbitMQ())
            {
                //Show a basic error if we fail
                MessageBox.Show("Could not connect to Broker");
            }
            
        }

        private int count = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string message = String.Format("{0} - {1}", count++, textBox1.Text);
            producer.SendMessage(System.Text.Encoding.UTF8.GetBytes(message));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Open a new Consumer Form
            PubSub_Consumer consumer = new PubSub_Consumer();
            consumer.Show();
        }



    }
}
