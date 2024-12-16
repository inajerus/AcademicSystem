using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AcademicSystem
{
    public partial class LecturerDashboard : Form

    {
        private int loggedInUserID;
        public LecturerDashboard(int userID)
        {
            InitializeComponent();
            this.loggedInUserID = userID;
        }

        private void LecturerDashboard_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to the Lecturer Dashboard!");
            LoadCoursesForLecturer(loggedInUserID);
        }
        private void LoadCoursesForLecturer(int lecturerID)
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
            SELECT c.CourseID, c.CourseName
            FROM LecturerCourses lc
            JOIN Courses c ON lc.CourseID = c.CourseID
            WHERE lc.LecturerID = @lecturerID";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@lecturerID", lecturerID);
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbCourses.Items.Clear();
                while (reader.Read())
                {
                    cmbCourses.Items.Add(new { Text = reader["CourseName"].ToString(), Value = reader["CourseID"] });
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message);
            }
        }
        private void cmbCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCourse = (dynamic)cmbCourses.SelectedItem;
            int courseID = selectedCourse.Value;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
            SELECT u.UserID, u.Username, g.Grade
            FROM GroupStudents gs
            JOIN Users u ON gs.StudentID = u.UserID
            LEFT JOIN Grades g ON g.StudentID = u.UserID AND g.CourseID = @courseID
            WHERE gs.GroupID IN (
                SELECT GroupID FROM GroupCourses WHERE CourseID = @courseID
            )";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                adapter.SelectCommand.Parameters.AddWithValue("@courseID", courseID);

                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvStudentsGrades.DataSource = table; // Mostrar los datos en el DataGridView

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        private void btnSaveGrades_Click(object sender, EventArgs e)
        {
            var selectedCourse = (dynamic)cmbCourses.SelectedItem;
            int courseID = selectedCourse.Value;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                foreach (DataGridViewRow row in dgvStudentsGrades.Rows)
                {
                    if (row.Cells["UserID"].Value != null && row.Cells["Grade"].Value != null)
                    {
                        int studentID = Convert.ToInt32(row.Cells["UserID"].Value);
                        decimal grade = Convert.ToDecimal(row.Cells["Grade"].Value);

                        string query = @"
                    INSERT INTO Grades (StudentID, CourseID, Grade)
                    VALUES (@studentID, @courseID, @grade)
                    ON DUPLICATE KEY UPDATE Grade = @grade";

                        MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        cmd.Parameters.AddWithValue("@grade", grade);

                        cmd.ExecuteNonQuery();
                    }
                }

                db.CloseConnection();
                MessageBox.Show("Grades saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving grades: " + ex.Message);
            }
        }

    }
}
