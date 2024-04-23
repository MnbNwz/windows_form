using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace C__windowsForm_New_
{
    public partial class Form1 : Form
    {
        private bool isInitialLoad = true;
        private const string conString = "Data Source=MUNEEB-PC\\SQLEXPRESS;Initial Catalog=Employee;Persist Security Info=True;User ID=sa;Password=abc123;TrustServerCertificate=True;";
        private string getEmployee = "SELECT empID, empFname, empLname, CONCAT(empID, '. ', LTRIM(RTRIM(empFname)), ' ', LTRIM(RTRIM(empLname))) AS FullName FROM emp_Data";
        private string insertQuery = "INSERT INTO emp_Data (empFname, empLname) VALUES (@FirstName, @LastName)";
        private string updateQuery = "UPDATE emp_Data SET empFname = @FirstName, empLname = @LastName WHERE empID = @EmpID";
        private string deleteQuery = "DELETE FROM emp_Data WHERE empID = @EmpID";
        private string deleteCourseFromEmp = "DELETE FROM Employee_Course WHERE empID = @EmpID";
        private string getCourses = "SELECT course_id, course_name, empId, CONCAT(course_id, '. ', LTRIM(RTRIM(course_name))) AS FullCourseName " +
                             "FROM Employee_Course " +
                             "WHERE empId = @EmpId";

        private string addCourse = "INSERT INTO Employee_Course (empID, course_name) " + "VALUES (@EmpId, @CourseName)";
        string updateCourse = "UPDATE Employee_Course SET course_name = @CourseName WHERE course_id = @CourseId AND empID = @EmpId";
        string deleteCourse = "DELETE FROM Employee_Course WHERE course_id = @CourseId AND empId = @EmpId";

        private bool IDTellerFlag = false;

        public Form1()
        {
            this.Enabled = false;
            InitializeComponent();
            DisablingCourses();
            InitializingAPI();
            comboBox1.DropDownClosed += comboBox1_DropDownClosed;
            comboBox2.DropDownClosed += comboBox2_DropDownClosed;

            comboBox1.SelectedIndex = -1;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            Setting_Index();
            this.Enabled = true;

        }

        private void Setting_Index()
        {
            comboBox1.TabIndex = 0;
            textBox1.TabIndex = 1;
            textBox2.TabIndex = 2;
            button1.TabIndex = 3;
            button4.TabIndex = 4;
            button2.TabIndex = 5;
            button3.TabIndex = 6;
        }

        private int iDTeller(bool flag = true)
        {
            if (flag)
            {
                if (comboBox1.Items.Count > 0 && comboBox1.SelectedIndex != -1)
                {
                    string[] parts = comboBox1.Text.Split('.');
                    if (parts.Length > 0)
                    {
                        if (int.TryParse(parts[0], out int empID))
                        {
                            return empID;
                        }
                    }
                }
            }
            else
            {
                {
                    if (comboBox2.Items.Count > 0 && comboBox2.SelectedIndex != -1)
                    {

                        var row = ((DataRowView)comboBox2.SelectedItem).Row;
                        string courseIdAsString = row["course_id"].ToString();
                        return int.Parse(courseIdAsString);

                    }
                }
            }
            return -1;
        }
        private async void InitializingAPI()
        {
            await FillComponentsData();
        }
        private async Task FillComponentsData()
        {
            //this.Enabled = false;
            try
            {

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(getEmployee, conn))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("empID", typeof(int));
                        dt.Columns.Add("FullName", typeof(string));
                        dt.Columns.Add("empFname", typeof(string));
                        dt.Columns.Add("empLname", typeof(string));
                        dt.Load(reader);

                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "FullName";
                        comboBox1.ValueMember = "empID";
                        comboBox1.SelectedIndex = -1;

                    }
                    conn.Close();
                }
                if (comboBox1.Items.Count > 0)
                {
                    //EnablingCourses();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DisablingCourses()
        {
            comboBox2.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            textBox3.Enabled = false;
        }
        private void EnablingCourses()
        {
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            textBox3.Enabled = true;
            comboBox2.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                button4.Enabled = false;
                if (IDTellerFlag && comboBox1.Items.Count > 0)
                {
                    //DataRowView selectedRow = (DataRowView)comboBox1.SelectedItem;
                    comboBox2.Enabled = true;
                    GettingCourses();
                }
                IDTellerFlag = true;
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var row = ((DataRowView)comboBox2.SelectedItem).Row;
                textBox3.Text = row["course_name"].ToString();
            }
        }
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            assiningVals();
        }

        void assiningVals()
        {
            if (comboBox1.SelectedItem != null)
            {
                var row = ((DataRowView)comboBox1.SelectedItem).Row;
                textBox1.Text = row["empFname"].ToString();
                textBox2.Text = row["empLname"].ToString();
                button4.Enabled = false;
            }
        }
        void settingComboBoxVal()
        {
            if (comboBox1.SelectedIndex != -1)
            {
                //comboBox1.SelectedIndex = -1;
            }
        }
        void settingEmptyTextVal()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty && textBox1.Text != string.Empty)

            {
                button4.Enabled = true;
            }
            settingComboBoxVal();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty && textBox1.Text != string.Empty)
            {
                button4.Enabled = true;
            }
            settingComboBoxVal();
        }



        private void GettingCourses()
        {
            try
            {
                comboBox2.Enabled = false;
                using (SqlConnection conn = new SqlConnection(conString))
                {

                    conn.Open();
                    using (SqlCommand command = new SqlCommand(getCourses, conn))
                    {
                        command.Parameters.AddWithValue("@EmpId", iDTeller());
                        SqlDataReader reader = command.ExecuteReader();
                        DataTable dt = new DataTable();

                        dt.Columns.Add("empId", typeof(int));
                        dt.Columns.Add("FullCourseName", typeof(string));
                        dt.Columns.Add("course_id", typeof(string));
                        dt.Columns.Add("course_name", typeof(string));
                        dt.Load(reader);

                        comboBox2.DataSource = dt;
                        comboBox2.DisplayMember = "course_name";
                        comboBox2.ValueMember = "course_id";

                    }
                    conn.Close();
                }
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0;
                    var row = ((DataRowView)comboBox2.SelectedItem).Row;
                    textBox3.Text = row["course_name"].ToString();


                    EnablingCourses();
                }
                else
                {
                    EnablingCourses();
                    comboBox2.Enabled = false;
                    textBox3.Text = string.Empty;
                    MessageBox.Show("No course available for this user, Please add course first.!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Clear user Button
            button4.Enabled = true;
            settingEmptyTextVal();
            settingComboBoxVal();
            comboBox1.SelectedIndex = -1;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            //Add user Button
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn))
                    {
                        insertCommand.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        insertCommand.Parameters.AddWithValue("@LastName", textBox2.Text);

                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected > 0 ? "Row inserted successfully." : "No rows inserted.");
                    }

                    conn.Close();
                }
                IDTellerFlag = false;
                InitializingAPI();
                settingEmptyTextVal();
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                DisablingCourses();
            }
            else
            {
                MessageBox.Show(textBox1.Text == String.Empty ? "First name can't be empty" : "Last name can't be empty");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Edit User Button
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No user selected.");
                return;
            }
            else if (textBox1.Text == String.Empty || textBox2.Text == String.Empty)
            {
                MessageBox.Show(textBox1.Text == String.Empty ? "First name cannot be empty." : "Last name cannot be empty.");
                return;
            }
            else if (comboBox1.SelectedItem != null)
            {
                DialogResult result = MessageBox.Show($"Do you want to change the name to {textBox1.Text} {textBox2.Text}?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var row = ((DataRowView)comboBox1.SelectedItem).Row;
                    if (row["empFname"].ToString() != textBox1.Text || row["empLname"].ToString() != textBox2.Text)
                    {
                        try
                        {
                            int selectedID = comboBox1.SelectedIndex;
                            using (SqlConnection conn = new SqlConnection(conString))
                            {
                                conn.Open();

                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
                                {
                                    updateCommand.Parameters.AddWithValue("@FirstName", textBox1.Text);
                                    updateCommand.Parameters.AddWithValue("@LastName", textBox2.Text);
                                    updateCommand.Parameters.AddWithValue("@EmpID", iDTeller());
                                    int rowsAffected = updateCommand.ExecuteNonQuery();
                                    MessageBox.Show(rowsAffected > 0 ? "Row updated successfully." : "No rows updated. EmpID not found.");
                                }

                                conn.Close();
                            }
                            IDTellerFlag = false;
                            InitializingAPI();
                            settingEmptyTextVal();
                            comboBox2.SelectedIndex = -1;
                            textBox3.Text = "";
                            DisablingCourses();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while updating the row: " + ex.Message);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Previous First name and Last name can't match with the current names");
                    }
                }
                else
                {
                    MessageBox.Show("Edit request denied!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Delete user Button
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No rows selected. Please select a row to remove the data.");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Do you want to delete the user {textBox1.Text} {textBox2.Text}?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            int rowsAffected1;
                            int rowsAffected2;
                            // Delete from emp_Data table
                            using (SqlCommand command1 = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                command1.Parameters.AddWithValue("@EmpID", iDTeller());
                                rowsAffected1 = command1.ExecuteNonQuery();
                            }

                            // Delete from Employee_Course table
                            using (SqlCommand command2 = new SqlCommand(deleteCourseFromEmp, connection, transaction))
                            {
                                command2.Parameters.AddWithValue("@EmpID", iDTeller());
                                rowsAffected2 = command2.ExecuteNonQuery();
                            }

                            transaction.Commit();   // Commit the transaction

                            int totalRowsAffected = rowsAffected1 + rowsAffected2;  // Check if any rows were deleted and display a message accordingly
                            if (totalRowsAffected > 0)
                            {
                                MessageBox.Show("Employee data and related courses deleted successfully.");
                                comboBox1.SelectedIndex = -1;
                                IDTellerFlag = false;
                                InitializingAPI();
                                settingEmptyTextVal();
                                comboBox2.SelectedIndex = -1;
                                textBox3.Text = "";
                                DisablingCourses();
                            }
                            else
                            {
                                MessageBox.Show("No rows deleted. EmpID not found.");
                            }

                            connection.Close();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error occurred while deleting employee data and related courses: " + ex.Message);
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Delete request denied!");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Add course button
            if (textBox3.Text != String.Empty)
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand insertCommand = new SqlCommand(addCourse, conn))
                    {
                        insertCommand.Parameters.AddWithValue("@EmpId", iDTeller());
                        insertCommand.Parameters.AddWithValue("@CourseName", textBox3.Text);


                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected > 0 ? "Course inserted successfully." : "No Course inserted.");
                    }

                    conn.Close();
                }
                GettingCourses();
                textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("Course field can't be empty");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Edit course button

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("No Course selected.");
                return;
            }
            else if (textBox3.Text == String.Empty)
            {
                MessageBox.Show("Course name cannot be empty.");
                return;
            }
            else if (comboBox2.SelectedItem.ToString() != textBox3.Text)
            {
                DialogResult result = MessageBox.Show($"Do you want to change course name to {textBox3.Text}?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int selectedID = comboBox2.SelectedIndex;
                        using (SqlConnection conn = new SqlConnection(conString))
                        {
                            conn.Open();

                            using (SqlCommand updateCommand = new SqlCommand(updateCourse, conn))
                            {
                                updateCommand.Parameters.AddWithValue("@CourseName", textBox3.Text);
                                updateCommand.Parameters.AddWithValue("@EmpID", iDTeller());
                                updateCommand.Parameters.AddWithValue("@CourseId", iDTeller(false));
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                MessageBox.Show(rowsAffected > 0 ? "Course updated successfully." : "No Course updated.");
                            }

                            conn.Close();
                        }
                        GettingCourses();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while updating the row: " + ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Edit request denied!");
                }
            }
            else
            {
                MessageBox.Show("Previous Course name can't match with the current names");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //delete course
            DialogResult result = MessageBox.Show($"Do you want to delete the course {textBox3.Text}?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                try
                {
                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        conn.Open();

                        using (SqlCommand deleteCommand = new SqlCommand(deleteCourse, conn))
                        {
                            deleteCommand.Parameters.AddWithValue("@EmpID", iDTeller());
                            deleteCommand.Parameters.AddWithValue("@CourseId", iDTeller(false));
                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                            if (rowsAffected > 0) comboBox2.SelectedIndex = -1;

                            MessageBox.Show("Course deleted successfully.");
                        }
                        conn.Close();
                    }
                    GettingCourses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the course: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Delete course request denied!");
            }

            //Delete course button
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            textBox4.Text = "Delay for 5 seconds";
            try
            {
                await Task.Delay(5000);
                MessageBox.Show("Wait delay for background threading completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button8.Enabled = true;
            }
        }

        private void UpdateTextBoxText(TextBox textBox, double value)
        {
            if (textBox.InvokeRequired)     // Marshal the update to textBox.Text back to the UI thread
            {
                textBox.Invoke(new Action(() => textBox.Text = value.ToString()));  //  current thread is not the UI thread, use Invoke to execute the update on the UI thread
            }
            else
            {
                textBox.Text = value.ToString();        // If the current thread is the UI thread, directly update textBox.Text
            }
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }


}
