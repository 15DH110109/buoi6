using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Form_server
{
    
    public partial class Form1 : Form
    {
        static Socket Listen;
        static Socket client;
        static IPEndPoint ipe;
        static int connections = 0;
        static Thread langnghe;
        public Form1()
        {
            InitializeComponent();
            StartServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            listBox1.Items.Add("Waiting for clients ....");            
        }
        private void StartServer()
        {
            langnghe = new Thread(new ThreadStart(ListenClient));
            langnghe.Start();
        }
        private void ListenClient()
        {
            Listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Any, 9050);
            Listen.Bind(ipe);
            Listen.Listen(10);
            while (true)
            {               
                if (Listen.Poll(1000000, SelectMode.SelectRead))
                {
                    client = Listen.Accept();
                    Thread newthread = new Thread(new ThreadStart(HandleConnection));
                    newthread.Start();
                }
            }
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            
        }

        public void HandleConnection()
        {
            int recv;
            byte[] data = new byte[1024];
            NetworkStream ns = new NetworkStream(client);
            connections++;
            listBox1.Items.Add("Client ket noi: co " + connections+ " Client ket noi");
            string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);
            while (true)
            {
                data = new byte[1024];
                recv = ns.Read(data, 0, data.Length);
                if (recv == 0)
                    break;
                ns.Write(data, 0, recv);
            }
            ns.Close();
            client.Close();
            connections--;
            listBox1.Items.Add("client ket thuc: con "+connections+" Client ket noi");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Close();
            Listen.Close();
        }
    }
}
