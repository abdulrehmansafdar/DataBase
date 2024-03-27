using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbMid
{
    public partial class Assessment : Form
    {
        private SqlConnection conn;
        private TabControl tabControl1; // Correct type declaration
        public int AssId;
        public Assessment()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;");
            tabControl1 = new TabControl();
            tabControl1.SuspendLayout();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 c = new Form1();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            Rubric c = new Rubric();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();
        }

        private void Assessment_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }
        private void GetStudentsRecord()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (conn.State == ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Assessment", conn))
                {
                    DataTable dt = new DataTable();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }

                    StudentRecord.DataSource = null;
                    StudentRecord.DataSource = dt;
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Assessment VALUES (@Title,GetDate(),@TotalMarks, @TotalWeightage)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text);

                        cmd.Parameters.AddWithValue("@TotalMarks", txtTotal.Text);
                        cmd.Parameters.AddWithValue("@TotalWeightage", txtWeight.Text);


                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Student added successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        }
        private bool isValid()
        {
            if (txtTitle.Text == string.Empty)
            {
                MessageBox.Show("RubricID  is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (AssId > 0) // Checking if AssId is greater than 0
            {
                int Id = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
                DateTime Date = Convert.ToDateTime(StudentRecord.SelectedRows[0].Cells[2].Value);
                SqlCommand cmd = new SqlCommand("Update Assessment SET Title=@RubricID, DateCreated=@Date, TotalMarks=@TotalMarks, TotalWeightage=@TotalWeightage WHERE ID=@ID", conn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RubricID", txtTitle.Text);

                cmd.Parameters.AddWithValue("@TotalMarks", txtTotal.Text);
                cmd.Parameters.AddWithValue("@TotalWeightage", txtWeight.Text);
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@ID", Id); // Use AssId directly as the parameter value

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Rubric updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
            }
            else
            {
                MessageBox.Show("Please select Rubric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            int Id = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
            SqlCommand cmd = new SqlCommand("Delete From Assessment  Where ID=@ID", conn);

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@ID", Id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Rubric Deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GetStudentsRecord();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();

            txtTotal.Clear();
            txtWeight.Clear();
        }

        private void StudentDataR(object sender, DataGridViewCellEventArgs e)
        {
            AssId = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
            //txtTitle.Text = StudentRecord.SelectedRows[0].Cells[1].Value.ToString();

            //txtTotal.Text = StudentRecord.SelectedRows[0].Cells[2].Value.ToString();
            //txtWeight.Text = StudentRecord.SelectedRows[0].Cells[3].Value.ToString();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AssessmentComponent a =new AssessmentComponent();
            a.TopLevel=false;
            a.FormBorderStyle = FormBorderStyle.None;
            a.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(a);
            a.Show();
        }
    }
}
