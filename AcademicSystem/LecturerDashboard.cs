using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AcademicSystem
{
    public partial class LecturerDashboard : Form
    {
        private int loggedInUserID; // ID del lecturer

        public LecturerDashboard(int userID)
        {
            InitializeComponent();
            this.loggedInUserID = userID;
        }

        // Cargar los cursos asignados al profesor
        private void LecturerDashboard_Load(object sender, EventArgs e)
        {
            LoadCoursesForLecturer();
        }

        private void LoadCoursesForLecturer()
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
                cmd.Parameters.AddWithValue("@lecturerID", loggedInUserID);
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbCourses.Items.Clear();
                while (reader.Read())
                {
                    cmbCourses.Items.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(reader["CourseID"]),
                        reader["CourseName"].ToString()));
                }

                cmbCourses.DisplayMember = "Value"; // Mostrar el nombre del curso
                cmbCourses.ValueMember = "Key";

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message);
            }
        }

        // Cargar estudiantes del curso seleccionado
        private void cmbCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCourse = (KeyValuePair<int, string>)cmbCourses.SelectedItem;
            int courseID = selectedCourse.Key;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
                    SELECT u.UserID, u.Username, IFNULL(g.Grade, '') AS Grade
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

                dgvStudentsGrades.DataSource = table;

                // Configurar columnas del DataGridView
                dgvStudentsGrades.Columns["UserID"].ReadOnly = true;
                dgvStudentsGrades.Columns["Username"].ReadOnly = true;
                dgvStudentsGrades.Columns["Grade"].ReadOnly = false;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        // Guardar notas introducidas o editadas
        private void btnSaveGrades_Click(object sender, EventArgs e)
        {
            if (cmbCourses.SelectedItem == null)
            {
                MessageBox.Show("Please select a course before saving grades.");
                return;
            }

            var selectedCourse = (KeyValuePair<int, string>)cmbCourses.SelectedItem;
            int courseID = selectedCourse.Key;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                foreach (DataGridViewRow row in dgvStudentsGrades.Rows)
                {
                    if (row.Cells["UserID"].Value != null && row.Cells["Grade"].Value != null)
                    {
                        int studentID = Convert.ToInt32(row.Cells["UserID"].Value);
                        string gradeValue = row.Cells["Grade"].Value.ToString();

                        // Validar si la nota es un número válido
                        if (!decimal.TryParse(gradeValue, out decimal grade))
                        {
                            MessageBox.Show($"Invalid grade format for student ID: {studentID}");
                            continue;
                        }

                        // Guardar la nota en la base de datos
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
