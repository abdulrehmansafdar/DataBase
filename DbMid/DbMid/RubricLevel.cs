using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbMid
{
    public partial class RubricLevel : Form
    {
        public RubricLevel()
        {
            InitializeComponent();
            fillRID();
        }

        private void RubricLevel_Load(object sender, EventArgs e)
        {
            showRubric();
        }
        private void showRubric()
        {
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "Select * from RubricLevel;";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(r);
                    RubricRecord.DataSource = dt;
                }
            }
        }

       
        private void fillRID()
        {
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "Select * from Rubric";
            using(SqlConnection conn = new SqlConnection(connection))
            { SqlCommand cmd = new SqlCommand( query,conn);
            conn.Open();
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                {
                    int id = Convert.ToInt32(r["ID"]);
                    comborRubid.Items.Add(id);
                }
            
            
            }
        }




        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int number = getnumber();
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            using( SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into RubricLevel (RubricId,Details,MeasurementLevel) Values (@id ,@details,@level)", conn);
               
               cmd.Parameters.AddWithValue("@id", comborRubid.Text);
                cmd.Parameters.AddWithValue("@details", txtDetails.Text);
                cmd.Parameters.AddWithValue("@level", number);
                cmd.ExecuteNonQuery();
                conn.Close();

              
            
            
            }
            MessageBox.Show("Successfull added Rubric Level","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            showRubric();
        }

        private int getnumber()
        {
            if (combolevel.Text=="Exceptional")
            {
                return 4;
            }
            else if (combolevel.Text == "Excellent")
            {
                return 3;
            }
            else if (combolevel.Text == "Good")
            {
                return 2;
            }
            else if (combolevel.Text == "Unsatisfactory")
            {
                return 1;
            }
            else
                return 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(combolevel.Text))
            {
                int number = getnumber();
                int rID = Convert.ToInt32(RubricRecord.SelectedRows[0].Cells[0].Value);
                string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE rubricLevel SET RubricId = @rid , Details = @Details, MeasurementLevel = @level WHERE Id = @id", conn);
                    cmd.Parameters.AddWithValue("@rid", comborRubid.Text);
                    cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                    cmd.Parameters.AddWithValue("@level", number);
                    cmd.Parameters.AddWithValue("@id", rID);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Successfully updated", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showRubric();
            }
            else
            {
                MessageBox.Show("Please select some rubric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            combolevel.Items.Clear();
            txtDetails.Clear();
            comborRubid.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (RubricRecord.SelectedRows.Count > 0)
            {
                int rID = Convert.ToInt32(RubricRecord.SelectedRows[0].Cells[0].Value);
                string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM RubricLevel WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", rID);

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Close the connection
                    conn.Close();
                }

                MessageBox.Show("Successfully deleted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the data grid view after deletion
                showRubric();
            }
            else
            {
                MessageBox.Show("Please select some rubric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 l = new Form1();
            l.TopLevel = false;
            l.FormBorderStyle = FormBorderStyle.None;
            l.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(l);
            l.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Assessment l = new Assessment();
            l.TopLevel = false;
            l.FormBorderStyle = FormBorderStyle.None;
            l.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(l);
            l.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CLOForm l = new CLOForm();
            l.TopLevel = false;
            l.FormBorderStyle = FormBorderStyle.None;
            l.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(l);
            l.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Attendence l = new Attendence();
            l.TopLevel = false;
            l.FormBorderStyle = FormBorderStyle.None;
            l.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(l);
            l.Show();
        }
    }
}
