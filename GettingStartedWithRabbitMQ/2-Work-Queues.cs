using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GettingStartedWithRabbitMQ.Example2WorkQueues;

namespace GettingStartedWithRabbitMQ
{
    public partial class _2_Work_Queues : Form
    {

        public string HOST_NAME = "localhost";
        public string QUEUE_NAME = "workQueues";

        private Consumer consumer;
        private Consumer consumer2;
        private Producer producer;

        public _2_Work_Queues()
        {
            InitializeComponent();

            //create the producer
            producer = new Producer(HOST_NAME, QUEUE_NAME);

            //**CONSUMER 1 **

            //create the consumer
            consumer = new Consumer(HOST_NAME, QUEUE_NAME);

            //this from will handle messages
            consumer.onMessageReceived += handleMessage;

            //start consuming
            consumer.StartConsuming();

            //**CONSUMER 2 **

            //create the second consumer
            consumer2 = new Consumer(HOST_NAME, QUEUE_NAME);

            //this from will handle messages
            consumer2.onMessageReceived += handleMessage2;

            //start consuming
            consumer2.StartConsuming();
        }

        private int count = 0;
        private void button1_Click_1(object sender, EventArgs e)
        {
            string message = String.Format("{0} - {1}", count++, textBox1.Text);
            producer.SendMessage(System.Text.Encoding.UTF8.GetBytes(message));
        }

        //delegate to post to UI thread
        private delegate void showMessageDelegate(string message);

        //Callback for message receive
        public void handleMessage(byte[] message)
        {
            Thread.Sleep(1000);
            showMessageDelegate s = new showMessageDelegate(richTextBox1.AppendText);

            this.Invoke(s,  System.Text.Encoding.UTF8.GetString(message) + Environment.NewLine);
        }

        //Callback for message receive
        public void handleMessage2(byte[] message)
        {
            showMessageDelegate s = new showMessageDelegate(richTextBox2.AppendText);

            this.Invoke(s, System.Text.Encoding.UTF8.GetString(message) + Environment.NewLine);
        }


    }
}
