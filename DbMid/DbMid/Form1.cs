using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace DbMid
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        private TabControl tabControl1; // Correct type declaration
        public int StudentID;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;");
            tabControl1 = new TabControl();
            tabControl1.SuspendLayout();
            panel1.Dock = DockStyle.Fill;


        }



        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();

        }
        private void GetStudentsRecord()
        {
            string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT * FROM student";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                StudentRecord.DataSource = dataTable;
                reader.Close();
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (isValid())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("insert into Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status) values (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status) ", conn);
                    cmd.Parameters.AddWithValue("@FirstName", txtStudentName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLast.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Email", txtAdress.Text);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", txtRegno.Text);
                    if (comboBox2.Text == "Active")
                    {
                        cmd.Parameters.AddWithValue("@Status", 5);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Status", 6);
                    }
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Student added successfully", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetStudentsRecord();
                }
            }
            else
            {
                MessageBox.Show("Failed to open database connection.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    

        private bool isValid()
        {
            if (txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

       




       
        

       
        private void StudentDataR(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
            txtStudentName.Text = StudentRecord.SelectedRows[0].Cells[1].Value.ToString();
            txtLast.Text = StudentRecord.SelectedRows[0].Cells[2].Value.ToString();
            txtRegno.Text = StudentRecord.SelectedRows[0].Cells[5].Value.ToString();
            txtAdress.Text = StudentRecord.SelectedRows[0].Cells[4].Value.ToString();
            txtPhone.Text = StudentRecord.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void StudentRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Your button2_Click logic
            tabControl1.SelectedIndex = 1;
            if (StudentID > 0)
            {
                string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Update Student SET FirstName=@FirstName,LastName = @LastName , Contact =@Contact, Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status Where ID=@ID", con);
                cmd.Parameters.AddWithValue("@FirstName", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLast.Text);
                cmd.Parameters.AddWithValue("@Contact", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtAdress.Text);
                cmd.Parameters.AddWithValue("@RegistrationNumber", txtRegno.Text);
                if (comboBox2.Text == "Active")
                {
                    cmd.Parameters.AddWithValue("@Status", 5);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Status", 6);
                }
                cmd.Parameters.AddWithValue("@ID", StudentID);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Student updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();

            }
            else
            {
                MessageBox.Show("Please select student", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                string deleteQuery = "DELETE FROM student WHERE id = @StudentId";
                using (conn) // Open and close connection automatically
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@StudentId", StudentID);

                        try
                        {
                            conn.Open(); // Open the connection within the using block
                            command.ExecuteNonQuery();

                            MessageBox.Show("Successfully Deleted!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }

                GetStudentsRecord();
            }
            else
            {
                MessageBox.Show("Please select some student", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            txtStudentName.Clear();

            txtRegno.Clear();
            txtAdress.Clear();
            txtPhone.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CLOForm c = new CLOForm();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel
           
            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Rubric r = new Rubric();
            r.TopLevel = false;
            r.FormBorderStyle = FormBorderStyle.None;
            r.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(r);
            r.Show();
        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void panelside_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtLast_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Assessment c = new Assessment();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            Attendence a = new Attendence();
            a.TopLevel = false;
            a.FormBorderStyle = FormBorderStyle.None;
            a.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(a);
            a.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtStudentName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRegno_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAdress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Result c = new Result();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();
        }
    }
}
