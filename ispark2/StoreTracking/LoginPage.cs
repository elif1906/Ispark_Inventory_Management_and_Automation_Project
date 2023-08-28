using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace StoreTracking
{


    public partial class LoginPage : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HS2O2T3\\SQLEXPRESS;Initial Catalog=ispark;Integrated Security=True");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader rd;

        public LoginPage()
        {
            InitializeComponent();
        }

        public int sicilNo;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox1.Text == "admin" && textBox2.Text == "1906")
                {
                    Menu viewOne = new Menu();
                    viewOne.Show();
                    this.Hide();
                }

                else
                {
                    string sorgu = "SELECT * FROM Users WHERE Record_Number = @Record_Number AND Password = @Password";
                    command = new SqlCommand(sorgu, connection);
                    command.Parameters.AddWithValue("@Record_Number", int.Parse(textBox1.Text));
                    command.Parameters.AddWithValue("@Password", textBox2.Text);
                    connection.Open();
                    rd = command.ExecuteReader();



                    if (rd.Read())
                    {
                        connection.Close();
                        Kullanici viewOne = new Kullanici();
                        viewOne.sicilNo = int.Parse(textBox1.Text);
                        viewOne.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bilgilerinizi Kontrol Edin");
                    }
                }
            }

            catch(System.FormatException ex)
            {
                MessageBox.Show("Kullanıcı Bulunamadı");
            }
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
