using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace CiscoRegistryEditor
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.youtube.com/watch?v=2UBIfAKuTsA");
        }
    }
}
