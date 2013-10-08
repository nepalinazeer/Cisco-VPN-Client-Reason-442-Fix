using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CiscoRegistryEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblRKeyLoc.Text = RegistrySettings.Default.RLocBaseKey + RegistrySettings.Default.RLocSubKey;

            string regVal;
             regVal = ReadCurrentRegistryVal(BaseKeys.LocalMachine.ToString(), RegistrySettings.Default.RLocSubKey, "Display Name");

             if (!string.IsNullOrEmpty(regVal))
             {
                 lblCurVal.Text = regVal;
                 CheckIfRegistryKeyisValid();
             }
             else
             {
                 lblCurVal.Text = "**NOT FOUND!**";
                 toolStripStatusLabel1.Text = "Could not fine the registry key. Pease make sure Cisco VPN Client is installed properly";
                 btnMakeChange.Enabled = false;
             }
        }

        public enum BaseKeys{ClassesRoot, CurrentConfig, CurrentUse, LocalMachine} 
        private string ReadCurrentRegistryVal(string baseKey, string keyLoc, string key)
        {
            string rVal = string.Empty;

            

            if(baseKey.Equals("LocalMachine"))
            {
            RegistryKey localMachinex64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey rKey = localMachinex64.OpenSubKey(keyLoc);
            if (rKey != null)
            {
                rVal = rKey.GetValue(key).ToString();
                rKey.Close();
            }
           
            }

            return rVal;
            
        }

        private bool WriteToCurrenRegistryVal(string baseKey, string keyLoc, string key, string value)
        {
            try
            {
                if (baseKey.Equals("LocalMachine"))
                {
                    RegistryKey localMachinex64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey rKey = localMachinex64.OpenSubKey(keyLoc, true);
                    if (rKey != null)
                    {
                        rKey.SetValue(key, value);
                        rKey.Close();
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            

            return false;

        }

        private void CheckIfRegistryKeyisValid()
        {
            if (lblCurVal.Text.Equals(RegistrySettings.Default.CorrectValue))
            {
                toolStripStatusLabel1.Text = "You already have the correct value for this key. The source of your error could be something else than invalid registry value for this key";
                btnMakeChange.Enabled = false;
            }
            else
            {

            }
        }

        private void btnMakeChange_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Applying Fix..";
                this.Cursor = Cursors.WaitCursor;

                bool status = WriteToCurrenRegistryVal(BaseKeys.LocalMachine.ToString(), RegistrySettings.Default.RLocSubKey, "Display Name", RegistrySettings.Default.CorrectValue);

                if (status)
                {
                    toolStripStatusLabel1.Text = "Value successfully changed to " + RegistrySettings.Default.CorrectValue + ". Restart may be required.";
                    btnMakeChange.Text = "Fixed!";
                    btnMakeChange.Enabled = false;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Failed While attempting to change value";
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
