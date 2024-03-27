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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DbMid
{
    public partial class CLOForm : Form
    {
        public CLOForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into CLo values(@CLOName, GetDate(), GetDate())", con);
            cmd.Parameters.AddWithValue("@CLOName", CLOName.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Successfully Inserted!");
            printCLO();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int CLoID = Convert.ToInt32(CLOGrid.SelectedRows[0].Cells[0].Value); // gets the id of selected row
            DateTime Date = Convert.ToDateTime(CLOGrid.SelectedRows[0].Cells[2].Value); // gets the date from selected row

            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Update CLo SET Name=@Name,DateCreated = @dateCreated ,DateUpdated = GetDate() Where ID=@ID", con);
            cmd.Parameters.AddWithValue("@Name", CLOName.Text);
            cmd.Parameters.AddWithValue("@dateCreated", Date);
            cmd.Parameters.AddWithValue("@ID", CLoID);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("CLO updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            printCLO();


        }
        private void printCLO()
        {
            string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT * FROM CLo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                CLOGrid.DataSource = dataTable;
                reader.Close();
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(CLOGrid.SelectedRows[0].Cells[0].Value);
            string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string deleteQuery = "DELETE FROM CLo WHERE id = @CLoId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CloId", rowIndex);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Successfully Deleted!");
                        printCLO();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

        }

        private void CLOForm_Load(object sender, EventArgs e)
        {
            
                printCLO();
            
        }

        private void CLOName_TextChanged(object sender, EventArgs e)
        {

        }

        private void CLOGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 c = new Form1();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel2.Controls.Clear();

            // Add CLOForm to the panel
            panel2.Controls.Add(c);
            c.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Rubric c = new Rubric();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel2.Controls.Clear();

            // Add CLOForm to the panel
            panel2.Controls.Add(c);
            c.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Assessment c = new Assessment();
            c.TopLevel = false;
            c.FormBorderStyle = FormBorderStyle.None;
            c.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel2.Controls.Clear();

            // Add CLOForm to the panel
            panel2.Controls.Add(c);
            c.Show();
        }
    }
}
