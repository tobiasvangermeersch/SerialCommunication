using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        private SerialPort serialPortArduino;

        public Form1()
        {
            InitializeComponent();
            serialPortArduino = new SerialPort();
            serialPortArduino.ReadTimeout = 1000;
            serialPortArduino.WriteTimeout = 1000;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (serialPortArduino.IsOpen)
            {
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
                labelStatus.Text = "Verbroken";
            }
            else
            {
                try
                {
                    serialPortArduino.PortName = comboBoxPoort.SelectedItem.ToString();
                    serialPortArduino.BaudRate = int.Parse(comboBoxBaudrate.SelectedItem.ToString());
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked)
                        serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked)
                        serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityMark.Checked)
                        serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked)
                        serialPortArduino.Parity = Parity.Space;
                    else
                        serialPortArduino.Parity = Parity.None;

                    if (radioButtonStopbitsTwo.Checked)
                        serialPortArduino.StopBits = StopBits.Two;
                    else if (radioButtonStopbitsOnePointFive.Checked)
                        serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsOne.Checked)
                        serialPortArduino.StopBits = StopBits.One;
                    else
                        serialPortArduino.StopBits = StopBits.None;

                    if (radioButtonHandshakeXonXoff.Checked)
                        serialPortArduino.Handshake = Handshake.XOnXOff;
                    else if (radioButtonHandshakeRTSXonXoff.Checked)
                        serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeRTS.Checked)
                        serialPortArduino.Handshake = Handshake.RequestToSend;
                    else
                        serialPortArduino.Handshake = Handshake.None;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    radioButtonVerbonden.Checked = true;
                    buttonConnect.Text = "Disconnect";
                    labelStatus.Text = "Verbonden met " + serialPortArduino.PortName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij verbinding: " + ex.Message, "Verbindingsfout");
                }
            }
        }

        private void tabPageInstellingen_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxDigital2_Checked(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    checkBoxDigital2.Checked = false;
                    return;
                }

                string command = checkBoxDigital2.Checked ? "set d2 high" : "set d2 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
                checkBoxDigital2.Checked = false;
            }
        }

        private void checkBoxDigital3_Checked(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    checkBoxDigital3.Checked = false;
                    return;
                }

                string command = checkBoxDigital3.Checked ? "set d3 high" : "set d3 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
                checkBoxDigital3.Checked = false;
            }
        }

        private void checkBoxDigital4_Checked(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    checkBoxDigital4.Checked = false;
                    return;
                }

                string command = checkBoxDigital4.Checked ? "set d4 high" : "set d4 low";
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
                checkBoxDigital4.Checked = false;
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    return;
                }

                string command = "set pwm9 " + trackBarPWM9.Value.ToString();
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
            }
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    return;
                }

                string command = "set pwm10 " + trackBarPWM10.Value.ToString();
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
            }
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    MessageBox.Show("Geen open seriële verbinding.", "Verbindingsfout");
                    return;
                }

                string command = "set pwm11 " + trackBarPWM11.Value.ToString();
                serialPortArduino.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verzenden commando: " + ex.Message, "Communicatiefout");
            }
        }
    }
}
