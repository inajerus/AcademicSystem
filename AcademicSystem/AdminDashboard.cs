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
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to the Admin Dashboard!");
            LoadGroups();
            LoadCourses();
            LoadUsers();
            LoadGroupsToComboBox();
            LoadCoursesToComboBox();
            LoadGroupCourses();
            LoadStudentsToComboBox();            // Cargar estudiantes en cmbStudents
            LoadGroupsForStudentsToComboBox();   // Cargar grupos en cmbGroupsForStudents
            LoadStudentGroups();
            LoadLecturersToComboBox();     // Cargar profesores
            LoadLecturerCourses();                // Cargar asignaciones existentes
            LoadCoursesForLecturersToComboBox();     // Cargar cursos para asignar a lecturers

        }

        private void btnCreateGroup_Click(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text;

            if (string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Please enter a group name.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "INSERT INTO `Groups` (GroupName) VALUES (@groupName)";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@groupName", groupName);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Group created successfully!");

                db.CloseConnection();

                // Refrescar el DataGridView después de insertar
                LoadGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateCourse_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text;

            if (string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("Please enter a course name.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "INSERT INTO `Courses` (CourseName) VALUES (@courseName)";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@courseName", courseName);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Course created successfully!");

                db.CloseConnection();

                // Actualizar el DataGridView
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadCourses()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT * FROM `Courses`";
                MySql.Data.MySqlClient.MySqlDataAdapter adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvCourses.DataSource = table;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem?.ToString();

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                // Consulta SQL para insertar un nuevo usuario
                string query = "INSERT INTO `Users` (Username, Password, Role) VALUES (@username, @password, @role)";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);

                cmd.ExecuteNonQuery();
                MessageBox.Show("User created successfully!");

                db.CloseConnection();

                // Actualizar el DataGridView
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadUsers()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                // Consulta SQL para seleccionar los usuarios
                string query = "SELECT UserID, Username, Role FROM `Users`";
                MySql.Data.MySqlClient.MySqlDataAdapter adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvUsers.DataSource = table;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadGroups()
        {
            try
            {
                // Conectar a la base de datos
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                // Consulta SQL para obtener los grupos
                string query = "SELECT GroupID, GroupName FROM `Groups`";
                MySql.Data.MySqlClient.MySqlDataAdapter adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();

                // Llenar el DataTable con los datos de la consulta
                adapter.Fill(table);

                // Asignar los datos al DataGridView
                dgvGroups.DataSource = table;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroups.CurrentRow == null)
            {
                MessageBox.Show("Please select a group to delete.");
                return;
            }

            int groupId = Convert.ToInt32(dgvGroups.CurrentRow.Cells["GroupID"].Value);

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "DELETE FROM `Groups` WHERE GroupID = @groupId";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@groupId", groupId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Group deleted successfully!");

                db.CloseConnection();

                LoadGroups(); // Refrescar el DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course to delete.");
                return;
            }

            int courseId = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "DELETE FROM `Courses` WHERE CourseID = @courseId";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@courseId", courseId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Course deleted successfully!");

                db.CloseConnection();

                LoadCourses(); // Refrescar el DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "DELETE FROM `Users` WHERE UserID = @userId";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("User deleted successfully!");

                db.CloseConnection();

                LoadUsers(); // Refrescar el DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadGroupsToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT GroupID, GroupName FROM `Groups`";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbGroups.Items.Clear();
                while (reader.Read())
                {
                    cmbGroups.Items.Add(new { Text = reader["GroupName"].ToString(), Value = reader["GroupID"] });
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadCoursesToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT CourseID, CourseName FROM `Courses`";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
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
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAssignCourse_Click(object sender, EventArgs e)
        {
            if (cmbGroups.SelectedItem == null || cmbCourses.SelectedItem == null)
            {
                MessageBox.Show("Please select a group and a course.");
                return;
            }

            var group = (dynamic)cmbGroups.SelectedItem;
            var course = (dynamic)cmbCourses.SelectedItem;

            int groupId = group.Value;
            int courseId = course.Value;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "INSERT INTO `GroupCourses` (GroupID, CourseID) VALUES (@groupId, @courseId)";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@groupId", groupId);
                cmd.Parameters.AddWithValue("@courseId", courseId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Course assigned to group successfully!");

                db.CloseConnection();

                LoadGroupCourses(); // Refrescar el DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadGroupCourses()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
            SELECT gc.AssignmentID, g.GroupName, c.CourseName
            FROM `GroupCourses` gc
            JOIN `Groups` g ON gc.GroupID = g.GroupID
            JOIN `Courses` c ON gc.CourseID = c.CourseID";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvGroupCourses.DataSource = table;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadStudentsToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT UserID, Username FROM Users WHERE Role = 'Student'";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbStudents.Items.Clear(); // Limpiar items previos
                while (reader.Read())
                {
                    cmbStudents.Items.Add(new { Text = reader["Username"].ToString(), Value = reader["UserID"] });
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }
        private void LoadGroupsForStudentsToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT GroupID, GroupName FROM `Groups`";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbGroupsForStudents.Items.Clear(); // Limpiar los ítems del ComboBox específico
                while (reader.Read())
                {
                    cmbGroupsForStudents.Items.Add(new { Text = reader["GroupName"].ToString(), Value = reader["GroupID"] });
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAssignStudent_Click(object sender, EventArgs e)
        {
            if (cmbStudents.SelectedItem == null || cmbGroupsForStudents.SelectedItem == null)
            {
                MessageBox.Show("Please select both a student and a group.");
                return;
            }

            var student = (dynamic)cmbStudents.SelectedItem;
            var group = (dynamic)cmbGroupsForStudents.SelectedItem;

            int studentId = student.Value;
            int groupId = group.Value;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "INSERT INTO GroupStudents (GroupID, StudentID) VALUES (@groupId, @studentId)";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@groupId", groupId);
                cmd.Parameters.AddWithValue("@studentId", studentId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student successfully assigned to group!");

                db.CloseConnection();

                LoadStudentGroups(); // Refrescar el DataGridView con las nuevas asignaciones
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("This student is already assigned to this group.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadStudentGroups()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
    SELECT sg.GroupID, g.GroupName, u.Username AS StudentUsername
    FROM GroupStudents sg
    JOIN `Groups` g ON sg.GroupID = g.GroupID
    JOIN Users u ON sg.StudentID = u.UserID";


                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvStudentGroups.DataSource = table; // Mostrar las asignaciones en el DataGridView

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student groups: " + ex.Message);
            }
        }
        private void LoadLecturersToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT UserID, Username FROM Users WHERE Role = 'Lecturer'";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbLecturers.Items.Clear();
                while (reader.Read())
                {
                    cmbLecturers.Items.Add(new { Text = reader["Username"].ToString(), Value = reader["UserID"] });
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lecturers: " + ex.Message);
            }
        }

        private void btnAssignLecturerToCourse_Click(object sender, EventArgs e)
        {
            if (cmbLecturers.SelectedItem == null || cmbCoursesForLecturers.SelectedItem == null)
            {
                MessageBox.Show("Please select both a lecturer and a course.");
                return;
            }

            var lecturer = (dynamic)cmbLecturers.SelectedItem;
            var course = (dynamic)cmbCoursesForLecturers.SelectedItem;

            int lecturerID = lecturer.Value;
            int courseID = course.Value;

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "INSERT INTO LecturerCourses (LecturerID, CourseID) VALUES (@lecturerID, @courseID)";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@lecturerID", lecturerID);
                cmd.Parameters.AddWithValue("@courseID", courseID);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Lecturer assigned to course successfully!");

                db.CloseConnection();

                LoadLecturerCourses(); // Refrescar el DataGridView
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("This lecturer is already assigned to this course.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadLecturerCourses()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = @"
            SELECT lc.LecturerID, u.Username AS LecturerName, c.CourseName
            FROM LecturerCourses lc
            JOIN Users u ON lc.LecturerID = u.UserID
            JOIN Courses c ON lc.CourseID = c.CourseID";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvLecturerCourses.DataSource = table;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lecturer courses: " + ex.Message);
            }
        }
        private void LoadCoursesForLecturersToComboBox()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                string query = "SELECT CourseID, CourseName FROM `Courses`";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbCoursesForLecturers.Items.Clear();
                while (reader.Read())
                {
                    cmbCoursesForLecturers.Items.Add(new { Text = reader["CourseName"].ToString(), Value = reader["CourseID"] });
                }

                cmbCoursesForLecturers.DisplayMember = "Text";
                cmbCoursesForLecturers.ValueMember = "Value";

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message);
            }
        }


    }
}
