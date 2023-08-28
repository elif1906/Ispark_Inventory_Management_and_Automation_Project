using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace StoreTracking
{
    public partial class elemanEkleSil : Form
    {

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HS2O2T3\\SQLEXPRESS;Initial Catalog=ispark;Integrated Security=True");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader read;

        void createDataTable(string query)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    da = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata Oluştu: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }



        void readData(string sorgu)
        {
            connection.Open();
            command = new SqlCommand(sorgu, connection);
            read = command.ExecuteReader();

            while (read.Read())
            {
                comboBox2.Items.Add(read["Record_Number"]);
            }
            read.Close();
            connection.Close();
        }

        void goster()
        {
            string sorgu2;

            if (comboBox1.Text == "Müdür")
            {    
                comboBox2.Items.Clear();
                sorgu2 = "SELECT Gm_Record_Number as Record_Number FROM General_Manager";
                readData(sorgu2);
                comboBox3.Visible = true;
                label9.Visible= true;
                comboBox2.Visible = true;
                label8.Visible = true;
            }
            else if (comboBox1.Text == "Şef")
            {
                comboBox2.Visible = true;
                label8.Visible = true;
                comboBox3.Visible = true;
                label9.Visible = true;
                comboBox2.Items.Clear();
                sorgu2 = "SELECT M_Record_Number as Record_Number FROM Manager";
                readData(sorgu2);
            }
            else if (comboBox1.Text == "Personel")
            {
                comboBox2.Visible = true;
                label8.Visible = true;
                comboBox3.Visible = true;
                label9.Visible = true;
                comboBox2.Items.Clear();
                sorgu2 = "SELECT C_Record_Number as Record_Number FROM Chef";
                readData(sorgu2);
            }
            else if (comboBox1.Text == "Genel Müdür")
            {
                comboBox2.Visible = false;
                label8.Visible = false;

                comboBox3.Visible = true;
                label9.Visible = true;

                comboBox2.Items.Clear();
                sorgu2 = "SELECT C_Record_Number as Record_Number FROM Chef";
                readData(sorgu2);
            }
        }

        public elemanEkleSil()
        {
            InitializeComponent();

            string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
            createDataTable(sorgu4);

            goster();
            comboBox1.Items.Add("Genel Müdür");
            comboBox1.Items.Add("Müdür");
            comboBox1.Items.Add("Şef");
            comboBox1.Items.Add("Personel");
            groupBox1.Hide();
            comboBox3.Visible= false;
            label9.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int olusturulanId;
            if (int.TryParse(label11.Text, out olusturulanId))
            {
                string connectionString = "Data Source=DESKTOP-HS2O2T3\\SQLEXPRESS;Initial Catalog=ispark;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        if (comboBox1.Text == "Personel")
                        {
                            string sorgu2 = "INSERT INTO Employee(E_Record_Number, C_Record_Number) VALUES(@E_Record_Number,@C_Record_Number)";
                            using (SqlCommand command = new SqlCommand(sorgu2, connection))
                            {
                                command.Parameters.AddWithValue("@E_Record_Number", olusturulanId);
                                command.Parameters.AddWithValue("@C_Record_Number", int.Parse(comboBox2.Text));
                                command.ExecuteNonQuery();
                            }
                            MessageBox.Show("Personel Görevi Verilmiştir");
                        }
                        else if (comboBox1.Text == "Şef")
                        {
                            string sorgu2 = "INSERT INTO Chef(C_Record_Number, M_Record_Number) VALUES(@C_Record_Number,@M_Record_Number)";
                            using (SqlCommand command = new SqlCommand(sorgu2, connection))
                            {
                                command.Parameters.AddWithValue("@C_Record_Number", olusturulanId);
                                command.Parameters.AddWithValue("@M_Record_Number", int.Parse(comboBox2.Text));
                                command.ExecuteNonQuery();
                            }
                            MessageBox.Show("Şef Görevi Verilmiştir");
                        }
                        else if (comboBox1.Text == "Müdür")
                        {
                            string sorgu2 = "INSERT INTO Manager(M_Record_Number, Gm_Record_Number, Department_Id) VALUES(@M_Record_Number, @Gm_Record_Number, @Department_Id)";
                            using (SqlCommand command = new SqlCommand(sorgu2, connection))
                            {
                                command.Parameters.AddWithValue("@M_Record_Number", olusturulanId);
                                command.Parameters.AddWithValue("@Gm_Record_Number", int.Parse(comboBox2.Text));

                                Dictionary<string, int> departmentIdMapping = new Dictionary<string, int>
                        {
                            { "Insan Kaynaklari", 1 },
                            { "Bilgi Sistemler", 2 },
                            { "Muhasebe", 3 },
                            { "Teknik Servis", 4 }
                        };

                                if (departmentIdMapping.TryGetValue(comboBox3.Text, out int mappedDepartmentId))
                                {
                                    command.Parameters.AddWithValue("@Department_Id", mappedDepartmentId);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Müdür Görevi Başarıyla Verildi");
                                }
                                else
                                {
                                    MessageBox.Show("Geçersiz Departman Seçimi");
                                }

                            }
                        }
                        else if (comboBox1.Text == "Genel Müdür")
                        {
                            string sorgu2 = "INSERT INTO General_Manager(Gm_Record_Number) VALUES(@Gm_Record_Number)";
                            using (SqlCommand command = new SqlCommand(sorgu2, connection))
                            {
                                command.Parameters.AddWithValue("@Gm_Record_Number", olusturulanId);
                                command.ExecuteNonQuery();
                            }
                            MessageBox.Show("Genel Müdür Görevi Verilmiştir");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir hata oluştu: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID değeri");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SirketElemanIslemleri viewOne = new SirketElemanIslemleri();
            viewOne.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            goster();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("İsim Ve Soyisim Alanları Doldurulmalıdır");

            }

            else
            {
                string sorgu = "INSERT INTO Users(Employee_Name, Employee_Surname,Password)VALUES(@Employee_Name, @Employee_Surname,'1234')";
                command = new SqlCommand(sorgu, connection);
                command.Parameters.AddWithValue("@Employee_Name", textBox1.Text);
                command.Parameters.AddWithValue("@Employee_Surname", textBox2.Text);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Yeni Personel Ekleme İşlemi Başarılı");
                connection.Close();
                groupBox1.Show();
            }

            connection.Open();
            string sorgu3 = "SELECT TOP 1 * FROM Users ORDER BY Record_Number DESC";
            command = new SqlCommand(sorgu3, connection);
            read = command.ExecuteReader();
            while (read.Read())
            {
                string sonVeri = read["Record_Number"].ToString();
                label11.Text = sonVeri;
            }
            read.Close();
            connection.Close();

            string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
            createDataTable(sorgu4);

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void RefreshDataGridView()
        {
            string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
            createDataTable(sorgu4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int silinecekElemanId = int.Parse(label6.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HS2O2T3\\SQLEXPRESS;Initial Catalog=ispark;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand commandEmployee = new SqlCommand())
                    {
                        commandEmployee.Connection = connection;
                        commandEmployee.CommandText = "DELETE FROM Users WHERE Record_Number = @Record_Number";
                        commandEmployee.Parameters.AddWithValue("@Record_Number", silinecekElemanId);

                        commandEmployee.ExecuteNonQuery();
                    }

                    RefreshDataGridView();

                    MessageBox.Show("Personel Silme Başarılı");

                   

                    string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
                    createDataTable(sorgu4);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("HATA Çalışana Ait Zimmetli Ürün Ve Görev Alınmalıdır");
            }
        }





        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void elemanEkleSil_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoginPage viewOne = new LoginPage();
            viewOne.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
