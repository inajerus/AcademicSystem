using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AcademicSystem
{
    public partial class StudentDashboard : Form
    {
        private int loggedInUserID;

        public StudentDashboard(int userID)
        {
            InitializeComponent();
            this.loggedInUserID = userID; // ID del estudiante logueado
        }

        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            LoadStudentGrades(); // Cargar las calificaciones del alumno
        }

        private void LoadStudentGrades()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                // Consulta SQL para obtener los cursos y calificaciones del estudiante
                string query = @"
                    SELECT c.CourseName AS 'Course', 
                           IFNULL(g.Grade, 'N/A') AS 'Grade'
                    FROM GroupStudents gs
                    JOIN GroupCourses gc ON gs.GroupID = gc.GroupID
                    JOIN Courses c ON gc.CourseID = c.CourseID
                    LEFT JOIN Grades g ON g.StudentID = gs.StudentID AND g.CourseID = c.CourseID
                    WHERE gs.StudentID = @studentID";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@studentID", loggedInUserID);

                // Llenar el DataGridView con los datos obtenidos
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvStudentGrades.DataSource = table;
                dgvStudentGrades.Columns["Course"].HeaderText = "Course";
                dgvStudentGrades.Columns["Grade"].HeaderText = "Grade";

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading grades: " + ex.Message);
            }
        }
    }
}
