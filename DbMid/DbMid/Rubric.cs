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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DbMid
{
    public partial class Rubric : Form
    {
        private SqlConnection conn;
        private TabControl tabControl1; // Correct type declaration
        public int RId;
        public Rubric()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;");
            fillClo();
        }

        private void Rubric_Load(object sender, EventArgs e)
        {
            GetRubric();
        }
        private void GetRubric()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (conn.State == ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Rubric", conn))
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

        private void button1_Click(object sender, EventArgs e)
        {
            int number = getCLOid();
            if (isValid())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO rubric (Id, Details, CLoId) VALUES (@id, @Details, @CLoId)", conn))
                    {

                        cmd.Parameters.AddWithValue("@id", txtRubricId.Text);
                        cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                        if (number != -1)
                        {
                            cmd.Parameters.AddWithValue("@CLoId", number);
                        }
                        else
                        {
                            return;
                        }
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Rubric added successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetRubric();
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
        private int getCLOid()
        {
            int CLOId = -1;
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from Clo where name = @name", con);
            cmd.Parameters.AddWithValue("@name", comboCLO.Text);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                CLOId = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return CLOId;
        }


        private void fillClo()
        {
            string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT * FROM CLO";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboCLO.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }

        private bool isValid()
        {
            if (txtRubricId.Text == string.Empty)
            {
                MessageBox.Show("RubricID  is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rID = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);

            if (rID > 0) // Checking if rID is greater than 0
            {
                int number = getCLOid();

                if (isValid())
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE rubric SET Details = @Details, CLoId = @CLoId WHERE Id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", rID);
                        cmd.Parameters.AddWithValue("@Details", txtDetails.Text);

                        if (number != -1)
                        {
                            cmd.Parameters.AddWithValue("@CLoId", number);
                        }
                        else
                        {
                            return;
                        }

                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Updated!");

                        GetRubric();

                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a Rubric to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (RId > 0)
            {
                int rowIndex = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
                string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
                string deleteQuery = "DELETE FROM rubric WHERE id = @RId";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@RId", RId);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Successfully Deleted!");
                            GetRubric();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a Rubric to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtRubricId.Clear();

            txtDetails.Clear();
           
        }

        private void StudentDataR(object sender, DataGridViewCellEventArgs e)
        {
            RId = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells[0].Value);
            txtRubricId.Text = StudentRecord.SelectedRows[0].Cells[2].Value.ToString();
            txtDetails.Text = StudentRecord.SelectedRows[0].Cells[1].Value.ToString();


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

        private void button7_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RubricLevel l = new RubricLevel();
            l.TopLevel = false;
            l.FormBorderStyle = FormBorderStyle.None;
            l.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(l);
            l.Show();
        }
    }
}
