using System;
using System.Collections;
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
    public partial class AssessmentComponent : Form
    {
        public AssessmentComponent()
        {
            InitializeComponent();
            fillAssessment();
            fillRubric();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void AssessmentComponent_Load(object sender, EventArgs e)
        {
            showComponent();
        }
        private void fillAssessment()
        {
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT * FROM Assessment";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboAssessment.Items.Add(reg);

                }


                reader.Close();
                conn.Close();
            }
        }
        private void fillRubric()
        {
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "Select * from Rubric";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string id = r.GetString(1);
                    comboRubric.Items.Add(id);
                }


            }
        }
        private void showComponent()
        {
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "Select * From AssessmentComponent";

            using(SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(r);
                    AssessmentRecord.DataSource = dt;
                   
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int aID = getAsssessmentId();
            int rID = getRubricId();
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open(); // Open the connection before executing the command
                SqlCommand cmd = new SqlCommand("INSERT INTO assessmentComponent (Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentId) VALUES (@name, @rid, @totalmarks, GetDate(), GetDate(), @aId)", conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@rid", rID);
                cmd.Parameters.AddWithValue("@totalmarks", txtTotal.Text);
                cmd.Parameters.AddWithValue("@aId", aID);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Successfully added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            showComponent();


        }


        private int getAsssessmentId()
        {
            int Id = -1;
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from assessment where title = @title", con);
            cmd.Parameters.AddWithValue("@title", comboAssessment.Text);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Id = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return Id;
        }

        private int getRubricId()
        {
            int Id = -1;
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from rubric where details = @details", con);
            cmd.Parameters.AddWithValue("@details", comboRubric.Text);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Id = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return Id;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int ACID = Convert.ToInt32(AssessmentRecord.SelectedRows[0].Cells[0].Value); // gets the id of selected row
            DateTime Date = Convert.ToDateTime(AssessmentRecord.SelectedRows[0].Cells[4].Value); // gets the date from selected row
            int aID = Convert.ToInt32(AssessmentRecord.SelectedRows[0].Cells[6].Value);
            int rID = Convert.ToInt32(AssessmentRecord.SelectedRows[0].Cells[2].Value);
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE assessmentComponent SET Name = @name, RubricId = @rId, TotalMarks = @totalmarks,DateCreated = @date,  DateUpdated = GetDate(), AssessmentId = @aId WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@rId", rID);
            cmd.Parameters.AddWithValue("@totalmarks", txtTotal.Text);
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.Parameters.AddWithValue("@aId", aID);
            cmd.Parameters.AddWithValue("@id", ACID);

            cmd.ExecuteNonQuery();
            showComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(AssessmentRecord.SelectedRows[0].Cells[0].Value);
            string connection = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("Delete from AssessmentComponent where Id = @id", con);
                cmd.Parameters.AddWithValue("@id", index);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showComponent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            RubricLevel c = new RubricLevel();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(c);
            c.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
