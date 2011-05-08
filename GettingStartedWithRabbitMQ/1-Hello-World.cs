using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GettingStartedWithRabbitMQ.Example1HelloWorld;

namespace GettingStartedWithRabbitMQ
{
    public partial class Form1 : Form
    {
        public string HOST_NAME = "localhost";
        public string QUEUE_NAME = "helloWorld";

        private Consumer consumer;
        private Producer producer;

        public Form1()
        {
            InitializeComponent();

            //create the producer
            producer = new Producer(HOST_NAME, QUEUE_NAME);

            //create the consumer
            consumer = new Consumer(HOST_NAME, QUEUE_NAME);

            //this from will handle messages
            consumer.onMessageReceived += handleMessage;

            //start consuming
            consumer.StartConsuming();
        }

        //Send the message on click
        private void button1_Click(object sender, EventArgs e)
        {
            producer.SendMessage(System.Text.Encoding.UTF8.GetBytes(textBox1.Text));
        }
        
        //delegate to post to UI thread
        private delegate void showMessageDelegate(string message);

        //Callback for message receive
        public void handleMessage(byte[] message)
        {
            showMessageDelegate s = new showMessageDelegate(richTextBox1.AppendText);

            this.Invoke(s,  System.Text.Encoding.UTF8.GetString(message) + Environment.NewLine);
        }
    }
}
