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
    public partial class Result : Form
    {
        private string connectionString = "Data Source=DESKTOP-54IBTRP\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True;";

        ErrorProvider errorProvider1= new ErrorProvider();
        ErrorProvider errorProvider2 = new ErrorProvider();
        ErrorProvider errorProvider3 = new ErrorProvider();
        ErrorProvider errorProvider4 = new ErrorProvider();
        ErrorProvider errorProvider5 = new ErrorProvider();
        ErrorProvider errorProvider6 = new ErrorProvider();
        public Result()
        {
            InitializeComponent();
            fillStudentCombo();
            fillAssessmentCombo();
            fillAssessmentComponent();
            fillRubricDetail();
            fillRubricID();
            fillRubricLID();
        }


        private void fillStudentCombo()
        {

            string query = "SELECT * FROM Student";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboStudent.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }

        private void fillAssessmentCombo()
        {
            string query = "SELECT * FROM Assessment";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboAssessment.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }

        private void fillAssessmentComponent()
        {
            string query = "SELECT * FROM AssessmentComponent";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboComponenet.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }


        private void fillRubricDetail()
        {
            string query = "SELECT * FROM Rubric";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(1);
                    comboRD.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }


        private void fillRubricID()
        {
            string query = "SELECT * FROM RubricLevel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string reg = reader.GetString(2);
                    comboRL.Items.Add(reg);

                }


                reader.Close();
                connection.Close();
            }
        }

        private int getStudentId()
        {
            int studentId = -1;
            try
            {


                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select id from Student where FirstName = @name", con);
                cmd.Parameters.AddWithValue("@name", comboStudent.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    studentId = Convert.ToInt32(reader["id"]);
                }

                con.Close();

                return studentId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return studentId;
            }



        }

        private int getComponentMarks()
        {
            int Marks = -1;
            try
            {


                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select totalMarks from AssessmentComponent where Name = @name", con);
                cmd.Parameters.AddWithValue("@name", comboComponenet.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Marks = Convert.ToInt32(reader["totalMarks"]);
                }

                con.Close();

                return Marks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return Marks;
            }
        }

        private int getRubricValue()
        {
            if (comboRL.Text == "Unsatisfactory")
            {
                return 1;
            }
            else if (comboRL.Text == "Fair")
            {
                return 2;
            }
            else if (comboRL.Text == "Good")
            {
                return 3;
            }
            else { return 4; }
        }

        private int getMaxRubric()
        {
            int max = -1;
            try
            {


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT MAX(measurementLevel) FROM rubriclevel WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", comboRubricLID.Text);

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)  // Check for DBNull in case no result is returned
                    {
                        max = Convert.ToInt32(result);
                    }
                    con.Close();
                }

                return max;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return max;
            }
        }


        private int calculateObtainedMarks()
        {
            int rvalue = getRubricValue();
            int marks = getComponentMarks();
            int maxrubricValue = getMaxRubric();

            int result = (rvalue / maxrubricValue) * marks;
            return result;
        }

        private void saveEvaluation()
        {
            try
            {
                int stdID = getStudentId();
                int assessmentComponentID = getComponentId();
                int marks = calculateObtainedMarks();

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate, obtained_marks) VALUES (@stdID, @ACID, @rubicLevelId, GetDate(), @marks)", con);
                    cmd.Parameters.AddWithValue("@stdID", stdID);
                    cmd.Parameters.AddWithValue("@ACID", assessmentComponentID);
                    cmd.Parameters.AddWithValue("@rubicLevelId", comboRubricLID.Text);
                    cmd.Parameters.AddWithValue("@marks", marks);
                    if (!checkResult())
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Successfully Saved!");

                        displayResult();
                    }
                    else
                    {
                        con.Close();
                        MessageBox.Show("This Result Already Exist!");

                        displayResult();
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }




        }


        private int getComponentId()
        {
            int compId = -1;

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select id from AssessmentComponent where name = @name", con);
            cmd.Parameters.AddWithValue("@name", comboComponenet.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                compId = Convert.ToInt32(reader["id"]);
            }

            con.Close();

            return compId;
        }


        private void displayResult()
        {
            try
            {
                string query = "SELECT StudentResult.*, Student.FirstName AS StudentName FROM StudentResult INNER JOIN Student ON StudentResult.StudentId = Student.Id WHERE StudentResult.StudentId = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", getStudentId());
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    StudentRecord.DataSource = dataTable;
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private bool checkResult()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM StudentResult WHERE StudentId = @id AND AssessmentComponentId = @ACID AND RubricMeasurementId= @rid", con);
                cmd.Parameters.AddWithValue("@id", getStudentId());
                cmd.Parameters.AddWithValue("@ACID", getComponentId());
                cmd.Parameters.AddWithValue("@rid", comboRubricLID.Text);
                int count = (int)(cmd.ExecuteScalar());

                con.Close();

                // If count is greater than 0, it means the date exists
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return true;
            }
        }
        private void Marks()
        {

            int marks = getComponentMarks();
        }


        private void fillRubricLID()
        {
            string query = "SELECT * FROM RubricLevel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    comboRubricLID.Items.Add(id);

                }


                reader.Close();
                connection.Close();
            }
        }

        private bool ValidateStudent()
        {
            if (comboStudent.SelectedItem == null || comboStudent.SelectedItem.ToString() == "")
            {
                errorProvider1.SetError(comboStudent, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider1.SetError(comboStudent, string.Empty);

            }
            return true;
        }

        private bool ValidateAssessment()
        {
            if (comboAssessment.SelectedItem == null || comboAssessment.SelectedItem.ToString() == "")
            {
                errorProvider2.SetError(comboAssessment, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider2.SetError(comboAssessment, string.Empty);

            }
            return true;
        }

        private bool ValidateComponent()
        {
            if (comboComponenet.SelectedItem == null || comboComponenet.SelectedItem.ToString() == "")
            {
                errorProvider3.SetError(comboComponenet, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider3.SetError(comboComponenet, string.Empty);

            }
            return true;
        }

        private bool ValidateRDetail()
        {
            if (comboRD.SelectedItem == null || comboRD.SelectedItem.ToString() == "")
            {
                errorProvider4.SetError(comboRD, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider4.SetError(comboRD, string.Empty);

            }
            return true;
        }

        private bool ValidateRLevel()
        {
            if (comboRL.SelectedItem == null || comboRL.SelectedItem.ToString() == "")
            {
                errorProvider5.SetError(comboRL, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider5.SetError(comboRL, string.Empty);

            }
            return true;
        }

        private bool ValidateRLevelId()
        {
            if (comboRubricLID.SelectedItem == null || comboRubricLID.SelectedItem.ToString() == "")
            {
                errorProvider6.SetError(comboRD, "It cannot be empty.");
                return false;
            }
            else
            {
                errorProvider6.SetError(comboRD, string.Empty);

            }
            return true;
        }

        private void Result_Load(object sender, EventArgs e)
        {

        }

        private void txtStudentName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateStudent())
            {
                return;
            }
            if (!ValidateAssessment())
            {
                return;
            }
            if (!ValidateRLevel())
            {
                return;
            }
            if (!ValidateRLevelId())
            {
                return;
            }
            if (!ValidateComponent())
            {
                return;
            }
            if (!ValidateRDetail())
            {
                return;
            }
            saveEvaluation();
        }

        private void comboRL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int StudentId = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells["StudentId"].Value);
                int ACId = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells["AssessmentComponentId"].Value);
                int rId = Convert.ToInt32(StudentRecord.SelectedRows[0].Cells["RubricMeasurementId"].Value);
                string deleteQuery = "DELETE FROM StudentResult WHERE StudentId = @id AND AssessmentComponentId = @ACID AND RubricMeasurementId= @rid";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", StudentId);
                        command.Parameters.AddWithValue("@ACID", ACId);
                        command.Parameters.AddWithValue("@rid", rId);

                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully Deleted!");
                        displayResult();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!ValidateStudent())
            {
                return;
            }
            displayResult();
        }
    }
}
