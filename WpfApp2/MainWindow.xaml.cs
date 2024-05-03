using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace VegetablesAndFruitsApp
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=\"10.0.0.40, 8843\";User ID=marcint;Password=1604;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectToDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectToDatabase())
            {
                MessageBox.Show("Connection successful!");

                DisplayVegetablesAndFruits();
                DisplayInfo();
            }
            else
            {
                MessageBox.Show("Connection failed!");
            }
        }

        private bool ConnectToDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private void DisplayVegetablesAndFruits()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM VegetablesAndFruits";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    vegetablesAndFruitsListBox.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void DisplayInfo()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                // Показати максимальну калорійність
                command.CommandText = "SELECT MAX(Calories) FROM VegetablesAndFruits";
                var maxCalories = command.ExecuteScalar();
                MessageBox.Show($"Max Calories: {maxCalories}");

                // Показати мінімальну калорійність
                command.CommandText = "SELECT MIN(Calories) FROM VegetablesAndFruits";
                var minCalories = command.ExecuteScalar();
                MessageBox.Show($"Min Calories: {minCalories}");

                // Показати середню калорійність
                command.CommandText = "SELECT AVG(Calories) FROM VegetablesAndFruits";
                var avgCalories = command.ExecuteScalar();
                MessageBox.Show($"Average Calories: {avgCalories}");

                // Показати кількість овочів
                command.CommandText = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Type = 'Vegetable'";
                var vegCount = command.ExecuteScalar();
                MessageBox.Show($"Number of vegetables: {vegCount}");

                // Показати кількість фруктів
                command.CommandText = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Type = 'Fruit'";
                var fruitCount = command.ExecuteScalar();
                MessageBox.Show($"Number of fruits: {fruitCount}");

                // Показати кількість овочів і фруктів заданого кольору
                command.CommandText = "SELECT COUNT(*) FROM VegetablesAndFruits WHERE Color = 'Red'";
                var redCount = command.ExecuteScalar();
                MessageBox.Show($"Number of red vegetables and fruits: {redCount}");

                // Показати кількість овочів і фруктів кожного кольору
                command.CommandText = "SELECT Color, COUNT(*) AS Count FROM VegetablesAndFruits GROUP BY Color";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MessageBox.Show($"Number of {reader["Color"]} vegetables and fruits: {reader["Count"]}");
                    }
                }

                // Показати овочі та фрукти з калорійністю нижче вказаної
                decimal maxCaloriesThreshold = 100; 
                command.CommandText = $"SELECT Name FROM VegetablesAndFruits WHERE Calories < {maxCaloriesThreshold}";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show($"Vegetables and fruits with calories below {maxCaloriesThreshold}:");
                    while (reader.Read())
                    {
                        MessageBox.Show($"{reader["Name"]}");
                    }
                }

                // Показати овочі та фрукти з калорійністю вище вказаної
                decimal minCaloriesThreshold = 200; 
                command.CommandText = $"SELECT Name FROM VegetablesAndFruits WHERE Calories > {minCaloriesThreshold}";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show($"Vegetables and fruits with calories above {minCaloriesThreshold}:");
                    while (reader.Read())
                    {
                        MessageBox.Show($"{reader["Name"]}");
                    }
                }

                // Показати овочі та фрукти з калорійністю у вказаному діапазоні
                decimal minRange = 100; 
                decimal maxRange = 300; 
                command.CommandText = $"SELECT Name FROM VegetablesAndFruits WHERE Calories BETWEEN {minRange} AND {maxRange}";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show($"Vegetables and fruits with calories between {minRange} and {maxRange}:");
                    while (reader.Read())
                    {
                        MessageBox.Show($"{reader["Name"]}");
                    }
                }

                // Показати усі овочі та фрукти жовтого або червоного кольору
                command.CommandText = "SELECT Name FROM VegetablesAndFruits WHERE Color = 'Yellow' OR Color = 'Red'";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show("Vegetables and fruits with yellow or red color:");
                    while (reader.Read())
                    {
                        MessageBox.Show($"{reader["Name"]}");
                    }
                }
            }
        }
    }
}
