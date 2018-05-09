using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Form_client
{
    public partial class Form1 : Form
    {
        static int recv;
        static byte[] data = new byte[1024];
        static Socket sock;
        static IPEndPoint ipe;
        static String stringData;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Parse(txtIP.Text), 9050);
            sock.Connect(ipe);
            listBox1.Items.Add("Connected to server");
            recv = sock.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            listBox1.Items.Add("Server:" + stringData);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMS.Text != "")
            {
                listBox1.Items.Add("Client: " + txtMS.Text);
                data = Encoding.ASCII.GetBytes(txtMS.Text);
                sock.Send(data, data.Length, SocketFlags.None);
                data = new byte[1024];
                recv = sock.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                listBox1.Items.Add("Server:" + stringData);
                txtMS.Text = "";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sock.Close();            
        }
    }
}
