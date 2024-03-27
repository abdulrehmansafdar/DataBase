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
    public partial class Attendence : Form
    {
        ErrorProvider errorProvider1 = new ErrorProvider();
        ErrorProvider errorProvider2 = new ErrorProvider();
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;");

        public Attendence()
        {
            InitializeComponent();
            //showstud();
            showstatus();
            GetStudentsRecord();
            fillComboRegistration();
        }
        private void fillComboRegistration()
        {
            string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT * FROM Student";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(5);
                    txtReg.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }

        private void Attendence_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.TopLevel = false;
            f1.FormBorderStyle = FormBorderStyle.None;
            f1.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(f1);
            f1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Assessment a = new Assessment();
            a.TopLevel = false;
            a.FormBorderStyle = FormBorderStyle.None;
            a.Dock = DockStyle.Fill;

            // Clear existing controls from the panel

            panel1.Controls.Clear();

            // Add CLOForm to the panel
            panel1.Controls.Add(a);
            a.Show();
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

        private void button7_Click(object sender, EventArgs e)
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

        private void txtReg_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void showstud()
        {
            conn.Open();
            string query = "Select * FROM Student";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string reg = reader.GetString(5);
                txtReg.Items.Add(reg);
            }
            conn.Close();

        }
        private void showstatus()
        {
            conn.Open();
            string query = "Select * FROM Lookup";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string reg = reader.GetString(1);
                txtStatus.Items.Add(reg);
            }
            conn.Close();

        }
        private void GetStudentsRecord()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (conn.State == ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand("Select * from StudentAttendance", conn))
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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (isValid())
            //{
            //    try
            //    {
            //        conn.Open(); // Open the connection once here

            //        // Initiate a transaction
            //        SqlTransaction transaction = conn.BeginTransaction();

            //        try
            //        {
            //            // Get the latest ClassAttendance Id
            //            int classAttendanceId;
            //            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM ClassAttendance", conn, transaction))
            //            {
            //                classAttendanceId = Convert.ToInt32(cmd.ExecuteScalar());
            //            }

            //            // Insert student attendance record with the retrieved Id
            //            using (SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance(StudentId, AttendanceStatus, AttendanceId) VALUES (@StudentId, @AttendanceStatus, @AttendanceId)", conn, transaction))
            //            {
            //                cmd.CommandType = CommandType.Text;
            //                cmd.Parameters.AddWithValue("@StudentId", txtReg.Text);
            //                cmd.Parameters.AddWithValue("@AttendanceStatus", encrypt(txtStatus.Text));
            //                cmd.Parameters.AddWithValue("@AttendanceId", classAttendanceId);

            //                cmd.ExecuteNonQuery();
            //            }

            //            // Commit the transaction if successful
            //            transaction.Commit();
            //            MessageBox.Show("Attendance Marked successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //            GetStudentsRecord();
            //        }
            //        catch (Exception ex)
            //        {
            //            // Rollback the transaction if any exception occurs
            //            transaction.Rollback();
            //            MessageBox.Show("Failed to mark attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //        finally
            //        {
            //            conn.Close(); // Close the connection in the finally block
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Failed to open database connection: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            if (txtReg.SelectedItem == null)
            {

                errorProvider1.SetError(txtReg, "It cannot be empty.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtReg, string.Empty);

            }


            if (txtStatus.SelectedItem == null)
            {
                errorProvider2.SetError(txtStatus, "It cannot be empty.");
                return;
            }
            else
            {
                errorProvider2.SetError(txtStatus, string.Empty);

            }
            saveAttendanceDate();
            GetStudentsRecord();
        }


        
        private bool isValid()
        {
            if (txtReg.Text == string.Empty)
            {
                MessageBox.Show("Registration Number  is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void txtStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private int encrypt(string  st)
        {
            if (st.ToLower() == "present")
                return 1;
            else if (st.ToLower() == "absent")
                return 2;
            else if (st.ToLower() == "leave")
                return 3;
            else if (st.ToLower() == "active")
                return 4;
            else if (st.ToLower() == "inactive")
                return 5;
            return 0;
        }
        //private int takeclass()
        //{
        //    string q= "Select Id From ClassAttendance";
        //    SqlCommand cmd = new SqlCommand(,conn);
        //    int Id;
        //    cmd.Parameters.AddWithValue()

        //}


        private void saveAttendanceDate()
        {
            ADDDate();
            int dateId = FindDateID();
            int studenId = FindStudentId();
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance (AttendanceId, StudentId, AttendanceStatus) VALUES (@attendanceId, @studentId, @status)", con);
                if (studenId != -1)
                {
                    cmd.Parameters.AddWithValue("@studentId", studenId);
                }
                else
                {
                    return;
                }
                if (dateId != -1)
                {
                    cmd.Parameters.AddWithValue("@attendanceId", dateId);
                }
                else
                {
                    return;
                }
                int number = giveNumber();
                cmd.Parameters.AddWithValue("@status", number);
                cmd.ExecuteNonQuery();
                con.Close();

            }

            MessageBox.Show("Attendance added successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);




        }

        private void resetData()
        {
            txtReg.Items.Clear();
            txtStatus.Items.Clear();
            

        }


        private int giveNumber()
        {
            if (txtStatus.Text == "Present")
            {
                return 1;
            }
            else if (txtStatus.Text == "Absent")
            {
                return 2;
            }
            else if (txtStatus.Text == "Leave")
            {
                return 3;
            }
            else { return 4; }
        }




        private void ADDDate()
        {
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            if (!CheckDateExist())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ClassAttendance (AttendanceDate) VALUES (@date)", con);
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtDate.Value));
                cmd.ExecuteNonQuery();
                con.Close();

            }

        }

        private int FindDateID()
        {
            int dateId = -1;
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from ClassAttendance where AttendanceDate = @date", con);
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtDate.Value));

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                dateId = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return dateId;

        }

        private bool CheckDateExist()
        {
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select count(*)from ClassAttendance where AttendanceDate = @date", con);
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtDate.Value));
            int count = (int)(cmd.ExecuteScalar());

            con.Close();

            // If count is greater than 0, it means the date exists
            return count > 0;



        }

        private int FindStudentId()
        {
            int studentId = -1;
            string constr = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from Student where RegistrationNumber = @reg", con);
            cmd.Parameters.AddWithValue("@reg", txtReg.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                studentId = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return studentId;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (StudentRecord.SelectedRows.Count > 0)
            {
                // Get the selected row index
                int rowIndex = StudentRecord.SelectedRows[0].Index;

                // Get the Id of the selected attendance record
                int attendanceId = Convert.ToInt32(StudentRecord.Rows[rowIndex].Cells["AttendanceId"].Value);

                // Get the updated status
                int updatedStatus = giveNumber();

                // Update the attendance record in the database
                UpdateAttendanceStatus(attendanceId, updatedStatus);

                // Refresh the DataGridView
                GetStudentsRecord();

                MessageBox.Show("Attendance updated successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a record to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAttendanceStatus(int attendanceId, int updatedStatus)
        {
            try
            {
                conn.Open();

                string query = "UPDATE StudentAttendance SET AttendanceStatus = @status WHERE AttendanceId = @attendanceId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", updatedStatus);
                    cmd.Parameters.AddWithValue("@attendanceId", attendanceId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
    }


