using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public class DatabaseConnection
{
    private MySqlConnection connection;

    public DatabaseConnection()
    {
        string connectionString = "server=localhost;database=academicsystem;user=root;password=root;";
        connection = new MySqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public MySqlConnection GetConnection()
    {
        return connection;
    }
}
