using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using MySql.Data.MySqlClient;
using System.Configuration;
using RFID_VendingMachine.Models;
using System.Text.RegularExpressions;

namespace RFID_VendingMachine
{
    public partial class MainForm : Form
    {
        
        MySqlConnection _conn = new MySqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        SettingForm settionForm = new SettingForm();
        Card _currentCard = null;
        string receivedStr = "";
        string VenderStr = "";

        // Receive RFID purpose
        List<char> _barcode = new List<char>(10);
        DateTime _lastKeystroke = new DateTime(0);

        public MainForm()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void settingsSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settionForm == null)
            {
                settionForm = new SettingForm();
            }
            this.Hide();
            settionForm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseForm())
            {
                e.Cancel = true;
            }
            CloseSerialPort();
        }

        private bool CloseForm()
        {
            Boolean cancel = false;
            DialogResult dialog = new DialogResult();

            dialog = MessageBox.Show("確定離開嗎?", "警告!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dialog == DialogResult.Yes)
            {
                System.Environment.Exit(1);
                cancel = true;
            }
            return cancel;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeProductButtons();
            OpenSerialPort(ConfigurationManager.AppSettings["DEVICE_RFID_COM_PORT"]);
            OpenVenderSerialPort(ConfigurationManager.AppSettings["DEVICE_VENDER_COM_PORT"]);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check timing (keystrokes within 100 ms)
            TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
            if (elapsed.TotalMilliseconds > 100)
                _barcode.Clear();

            // record keystroke & timestamp
            _barcode.Add(e.KeyChar);
            _lastKeystroke = DateTime.Now;

            // process barcode
            if (e.KeyChar == 13 && _barcode.Count > 0)
            {
                string msg = new String(_barcode.ToArray());
                Console.WriteLine(msg);
                ActivateProductButtons(msg.Trim('\r'));
                _barcode.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(((TextBox)sender).Text);
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
//            Console.WriteLine("Data Received:");
            Console.Write(indata);

            if (_currentCard != null)
            {
                if (!string.IsNullOrEmpty(indata))
                {
                    VenderStr += indata;
                }

                string pattern = ":(.+):";
                foreach (Match match in Regex.Matches(VenderStr, pattern, RegexOptions.IgnoreCase))
                {
                    if (match.Success)
                    {
                        int pID = int.Parse(match.Groups[1].Value);
                        if (pID != 0)
                        {
                            Product product = GetProduct(pID);
                            DeactivateProductButtons();
                            if (product != null && (_currentCard.Amount - product.Amount) > 0)
                            {
                                MinusUserAmount(_currentCard.CardNo, product.Amount);
                            }
                            serialPort2.Write(pID.ToString());
                            UpdateCardInfo();
                            serialPort2.Write("8");
                            _currentCard = null;
                           
                        }
                        VenderStr = "";
                    }

                }

                /*           if (indata.StartsWith("OK"))
                           {
                               if (indata.Length == 4)
                               {
                                   int pID = int.Parse(indata.Split(',')[1]);

                                   if (pID != 0)
                                   {
                                       Product product = GetProduct(pID);
                                       DeactivateProductButtons();
                                       if (product != null && (_currentCard.Amount - product.Amount) > 0)
                                       {
                                           MinusUserAmount(_currentCard.CardNo, product.Amount);
                                       }
                                       UpdateCardInfo();
                                       _currentCard = null;
                                   }
                               }
                               else
                               {
                                   VenderStr += indata;
                               }  

                           }   else if (indata.StartsWith("FAILED"))
                           {
                               DeactivateProductButtons();
                           }
                           */

            }
            else
            {
                if (!string.IsNullOrEmpty(indata))
                {
                    receivedStr += indata;
                }

                string pattern = ":(.+):";
                foreach (Match match in Regex.Matches(receivedStr, pattern, RegexOptions.IgnoreCase))
                {
                    if (match.Success)
                    {
                        _currentCard = GetCard(match.Groups[1].Value);
                        ActivateProductButtons(_currentCard.CardNo);
                        serialPort2.Write("9");
                        receivedStr = "";
                    }
                }
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            Button btnProduct = (Button)sender;
            Int32 id = 0;
            if (btnProduct == btnCola)
            {
                id = 1;
            }
            else if (btnProduct == btnPotato)
            {
                id = 2;
            }
            else if (btnProduct == btnBlackTea)
            {
                id = 3;
            }

            if (id != 0)
            {
                Product product = GetProduct(id);
                DeactivateProductButtons();
                if (product != null && (_currentCard.Amount - product.Amount) > 0)
                {
                    MinusUserAmount(_currentCard.CardNo, product.Amount);
                    UpdateCardInfo();

                    serialPort2.Write(id.ToString());
                    _currentCard = null;
                    // serialPort1.Write("OK," + id.ToString());
                }
                else
                {
                    serialPort2.Write("FAILED");
                }
            }
        }

        

        private void OpenSerialPort(string portName)
        {
            try
            {
                serialPort1.PortName = portName;
                serialPort1.Open();
            }catch(Exception exp)
            {
                MessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Environment.Exit(1);
            }
        }

        private void OpenVenderSerialPort(string portName)
        {
            try
            {
                serialPort2.PortName = portName;
                serialPort2.Open();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Environment.Exit(1);
            }
        }

        private void CloseSerialPort()
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void CloseVenderSerialPort()
        {
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
            }
        }
        private void DeactivateProductButtons()
        {
            Button[] _productButtons = { btnCola, btnPotato, btnBlackTea };
            foreach (Button btn in _productButtons)
            {
                btn.Invoke(new Action(() => btn.Enabled = false));
            }
        }

        private void ActivateProductButtons(string cardNo)
        {
            _currentCard = GetCard(cardNo);
            if (_currentCard == null)
            {
                return;
            }
            UpdateCardInfo();
            Button[] _productButtons = { btnCola, btnPotato, btnBlackTea };
            using (_conn)
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM products WHERE amount <= @amount", _conn);
                command.Parameters.AddWithValue("@amount", _currentCard.Amount);
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int pid = Int32.Parse(reader["id"].ToString());
                        string amount = reader["amount"].ToString();
                        //_productButtons[pid - 1].Enabled = true;
                        _productButtons[pid - 1].Invoke((MethodInvoker)delegate
                        {
                            _productButtons[pid - 1].Enabled = true;
                        });
                    }
                }
            }
        }

        private Card GetCard(string cardNo)
        {
            Card card = null;
            using (_conn)
            {
                MySqlCommand command = new MySqlCommand(
                    @"SELECT u.*, c.*, u.id user_id FROM users u 
                       INNER JOIN users_cards uc ON uc.user_id = u.id 
                       INNER JOIN cards c ON c.id = uc.card_id
                      WHERE c.card_no = @cardNo", _conn);
                command.Parameters.AddWithValue("@cardNo", cardNo);

                _conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        card = new Card();
                        card.Id = Int32.Parse(reader["id"].ToString());
                        card.CardNo = reader["card_no"].ToString();
                        card.Amount = Decimal.Parse(reader["amount"].ToString());
                        card.User = new User();
                        card.User.Id = Int32.Parse(reader["user_id"].ToString());
                        card.User.Name = reader["name"].ToString();
                        card.User.Email = reader["email"].ToString();

                    }
                }
            }

            return card;
        }

        private void MinusUserAmount(string cardNo, decimal amount)
        {
            using (_conn)
            {
                MySqlCommand command = new MySqlCommand(
                    @"UPDATE cards SET amount = amount - @amount
                      WHERE card_no = @cardNo", _conn);
                command.Parameters.AddWithValue("@cardNo", cardNo);
                command.Parameters.AddWithValue("@amount", amount);
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                int result = command.ExecuteNonQuery();
            }
        }

        private void UpdateCardInfo() {
            lblCardInfo.Invoke(new Action(() =>
                {
                    _currentCard = GetCard(_currentCard.CardNo);
                    lblCardInfo.Text = string.Format("用戶:{0}, 餘額: {1}", _currentCard.User.Name, _currentCard.Amount);
                }
            ));

        }

        private void InitializeProductButtons()
        {
            Button[] _productButtons = { btnCola, btnPotato, btnBlackTea };
            Product[] products = GetProducts();
            for (int i = 0; i < products.Length; i++)
            {
                _productButtons[i].Text = string.Format("{0} 元", products[i].Amount);
            }
        }

        #region DB



        private Product GetProduct(int productID)
        {
            Product product = null;

            using (_conn)
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM products WHERE id = @productID", _conn);
                command.Parameters.AddWithValue("@productID", productID);

                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        product = new Product();
                        product.Id = Int32.Parse(reader["id"].ToString());
                        product.Amount = decimal.Parse(reader["amount"].ToString());
                    }
                }
            }
            return product;
        }

        private Product[] GetProducts()
        {
            ArrayList products = new ArrayList();

            using (_conn)
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM products", _conn);
                
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.Id = Int32.Parse(reader["id"].ToString());
                        product.Name = reader["name"].ToString();
                        product.Amount = decimal.Parse(reader["amount"].ToString());
                        products.Add(product);
                    }
                }
            }

            return (Product[])products.ToArray(typeof(Product));
        }
        #endregion

    }
}
