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
        private System.Windows.Forms.Timer timerOefening4;

        public Form1()
        {
            InitializeComponent();
            serialPortArduino = new SerialPort();
            serialPortArduino.ReadTimeout = 1000;
            serialPortArduino.WriteTimeout = 1000;

            // Initialiseer timerOefening4
            timerOefening4 = new System.Windows.Forms.Timer();
            timerOefening4.Interval = 1000;
            timerOefening4.Tick += timerOefening4_Tick;
            timerOefening4.Enabled = false;
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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Enable timer only when tabPageOefening3 (index 3) is selected
                if (tabControl.SelectedIndex == 3)
                {
                    timerOefening3.Enabled = true;
                    this.Text = "BZL seriële communicatie Tobias Vangermeersch [Timer ON - Tab 3]";
                }
                else
                {
                    timerOefening3.Enabled = false;
                    this.Text = "BZL seriële communicatie Tobias Vangermeersch [Timer OFF - Tab " + tabControl.SelectedIndex + "]";
                }

                // Enable/disable timerOefening4 based on tabPageOefening4 selection
                if (tabControl.SelectedIndex == 4)
                {
                    timerOefening4.Enabled = true;
                }
                else
                {
                    timerOefening4.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij tab selectie: " + ex.Message, "Fout");
            }
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    return;
                }

                // Clear previous responses
                serialPortArduino.ReadExisting();
                System.Threading.Thread.Sleep(50);

                // Query and update digital pin 5
                serialPortArduino.WriteLine("get d5");
                System.Threading.Thread.Sleep(150);
                string response5 = serialPortArduino.ReadExisting().Trim();
                if (response5.Length > 0)
                {
                    char lastChar = response5[response5.Length - 1];
                    radioButtonDigital5.Checked = (lastChar == '1');
                }
                else
                {
                    // No response received
                    radioButtonDigital5.Checked = false;
                }

                System.Threading.Thread.Sleep(50);

                // Query and update digital pin 6
                serialPortArduino.WriteLine("get d6");
                System.Threading.Thread.Sleep(150);
                string response6 = serialPortArduino.ReadExisting().Trim();
                if (response6.Length > 0)
                {
                    char lastChar = response6[response6.Length - 1];
                    radioButtonDigital6.Checked = (lastChar == '1');
                }
                else
                {
                    // No response received
                    radioButtonDigital6.Checked = false;
                }

                System.Threading.Thread.Sleep(50);

                // Query and update digital pin 7
                serialPortArduino.WriteLine("get d7");
                System.Threading.Thread.Sleep(150);
                string response7 = serialPortArduino.ReadExisting().Trim();
                if (response7.Length > 0)
                {
                    char lastChar = response7[response7.Length - 1];
                    radioButtonDigital7.Checked = (lastChar == '1');
                }
                else
                {
                    // No response received
                    radioButtonDigital7.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout in timer: " + ex.Message, "Communicatiefout");
            }
        }

        private void timerOefening4_Tick(object sender, EventArgs e)
        {
            try
            {
                // Controleer of er een seriële verbinding aanwezig is
                if (!serialPortArduino.IsOpen)
                {
                    return;
                }

                // Verwijder alle voorgaande antwoorden van de Arduino
                serialPortArduino.ReadExisting();
                System.Threading.Thread.Sleep(50);

                // Verstuur het commando om de waarde van analog0 op te vragen
                serialPortArduino.WriteLine("get a0");
                System.Threading.Thread.Sleep(150);

                // Lees het antwoord uit en trim tot je enkel de waarde overhoudt
                string response = serialPortArduino.ReadExisting().Trim();

                if (response.Length > 0)
                {
                    // Haal alleen het getal uit het antwoord (laatste deel)
                    string[] parts = response.Split(' ');
                    string analogValue = parts[parts.Length - 1];

                    // Schrijf dit antwoord naar de property Text van labelAnalog0
                    labelAnalog0.Text = analogValue;
                }
                else
                {
                    labelAnalog0.Text = "Geen antwoord";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout in timerOefening4: " + ex.Message, "Communicatiefout");
            }
        }
    }
}