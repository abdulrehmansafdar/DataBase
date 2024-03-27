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

namespace CLOMisyafa
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        // ADDing function
        private void button2_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=GREY\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into CLo values(@CLOName, GetDate(), GetDate())", con);
            cmd.Parameters.AddWithValue("@CLOName", CLOName.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Successfully Inserted!");
            printCLO();
        }

        // deletion
        private void button4_Click(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(CLOGrid.SelectedRows[0].Cells[0].Value);
            string connectionString = "Data Source=Grey\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True";
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

        
        // function used for printing clo
        private void printCLO()
        {
            string connectionString = "Data Source=GREY\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True";
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

        private void Form1_Load(object sender, EventArgs e)
        {
                printCLO();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // modification
        private void button3_Click(object sender, EventArgs e)
        {
            int CLoID = Convert.ToInt32(CLOGrid.SelectedRows[0].Cells[0].Value); // gets the id of selected row
            DateTime Date = Convert.ToDateTime(CLOGrid.SelectedRows[0].Cells[2].Value); // gets the date from selected row

            string constr = "Data Source=GREY\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True";
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

        private void CLOName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
