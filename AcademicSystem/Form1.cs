using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcademicSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Obtener los valores ingresados por el usuario
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
                // Conectar a la base de datos
                DatabaseConnection db = new DatabaseConnection();
                db.OpenConnection();

                // Consulta SQL para validar el usuario
                string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password AND Role = @role";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);

                MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    int userID = Convert.ToInt32(reader["UserID"]); // Obtener el UserID del usuario logueado
                    MessageBox.Show("Login successful!");

                    // Redirigir al panel correspondiente según el rol
                    switch (role)
                    {
                        case "Administrator":
                            AdminDashboard adminDashboard = new AdminDashboard();
                            adminDashboard.Show();
                            break;

                        case "Lecturer":
                            LecturerDashboard lecturerDashboard = new LecturerDashboard(userID);
                            lecturerDashboard.Show();
                            break;

                        case "Student":
                            StudentDashboard studentDashboard = new StudentDashboard();
                            studentDashboard.Show();
                            break;
                    }

                    this.Hide(); // Ocultar el formulario actual
                }
                else
                {
                    MessageBox.Show("Invalid credentials!");
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
