using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace RFID_VendingMachine
{
    public partial class SettingForm : Form
    {
        MySqlConnection _conn = new MySqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);

        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            (new MainForm()).Show();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            RefreshUsers();
            RefreshProducts();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshUsers();
            RefreshProducts();
        }

        private void RefreshUsers()
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(
                @"SELECT u.id, u.name, u.email, c.amount, c.card_no, u.created_at, u.updated_at FROM users u
                 INNER JOIN users_cards uc ON uc.user_id = u.id
                 INNER JOIN cards c ON c.id = uc.card_id 
            ", _conn);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
            adapter.Fill(dt);
            usersGV.DataSource = dt;
        }

        private void RefreshProducts()
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter("select * from products", _conn);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
            adapter.Fill(dt);
            productDataGV.DataSource = dt;
        }

        private void RefreshCards(int userId)
        {
            DataTable dt = new DataTable();
            MySqlCommand command = new MySqlCommand(@"SELECT cards.* FROM cards 
               INNER JOIN users_cards on users_cards.card_id = cards.id 
               WHERE users_cards.user_id=@user_id", _conn);
            command.Parameters.AddWithValue("@user_id", userId);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
            adapter.Fill(dt);
            
        }

    }
}
