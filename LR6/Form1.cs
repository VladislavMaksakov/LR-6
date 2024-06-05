using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace LR6
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        Dictionary<string, List<string>> cityRegions = new Dictionary<string, List<string>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Key"].ConnectionString);
            sqlConnection.Open();
            comboBox5.SelectedIndexChanged += comboBoxCities_SelectedIndexChanged;

            cityRegions.Add("Vinnytsia", new List<string> { "Vinnytsia Oblast" });
            cityRegions.Add("Volyn", new List<string> { "Volyn Oblast" });
            cityRegions.Add("Dnipropetrovsk", new List<string> { "Dnipropetrovsk Oblast" });
            cityRegions.Add("Donetsk", new List<string> { "Donetsk Oblast" });
            cityRegions.Add("Zhytomyr", new List<string> { "Zhytomyr Oblast" });
            cityRegions.Add("Zakarpattia", new List<string> { "Zakarpattia Oblast" });
            cityRegions.Add("Zaporizhzhia", new List<string> { "Zaporizhzhia Oblast" });
            cityRegions.Add("Ivano-Frankivsk", new List<string> { "Ivano-Frankivsk Oblast" });
            cityRegions.Add("Kyiv", new List<string> { "Kyiv Oblast" });
            cityRegions.Add("Kirovohrad", new List<string> { "Kirovohrad Oblast" });
            cityRegions.Add("Luhansk", new List<string> { "Luhansk Oblast" });
            cityRegions.Add("Lviv", new List<string> { "Lviv Oblast" });
            cityRegions.Add("Mykolaiv", new List<string> { "Mykolaiv Oblast" });
            cityRegions.Add("Odessa", new List<string> { "Odessa Oblast" });
            cityRegions.Add("Poltava", new List<string> { "Poltava Oblast" });
            cityRegions.Add("Rivne", new List<string> { "Rivne Oblast" });
            cityRegions.Add("Sumy", new List<string> { "Sumy Oblast" });
            cityRegions.Add("Ternopil", new List<string> { "Ternopil Oblast" });
            cityRegions.Add("Kharkiv", new List<string> { "Kharkiv Oblast" });
            cityRegions.Add("Kherson", new List<string> { "Kherson Oblast" });
            cityRegions.Add("Khmelnytskyi", new List<string> { "Khmelnytskyi Oblast" });
            cityRegions.Add("Cherkasy", new List<string> { "Cherkasy Oblast" });
            cityRegions.Add("Chernivtsi", new List<string> { "Chernivtsi Oblast" });
            cityRegions.Add("Chernihiv", new List<string> { "Chernihiv Oblast" });

            comboBox5.DataSource = new BindingSource(cityRegions.Keys.ToList(), null);
        }
        //
        //SELECT
        //
        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT suppliers.supplier_name, suppliers.contract_exp_date, " +
                "goods.product_name FROM suppliers JOIN goods ON suppliers.supplier_id = goods.product_supplier_id", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM category;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM clients;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT goods.product_name, goods.product_unit, " +
                "goods.product_price, producers.producers_name, category.category_name, suppliers.supplier_name, " +
                "suppliers.contract_exp_date FROM goods JOIN producers ON goods.product_producer_id = producers.producers_id " +
                "JOIN category ON goods.product_category_id = category.category_id JOIN suppliers ON goods.product_supplier_id = suppliers.supplier_id;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM producers;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT receipts.amount, receipts.cost, goods.product_name, goods.product_unit, " +
                "clients.first_name, clients.last_name, clients.city, clients.region FROM receipts " +
                "JOIN goods ON receipts.purchased_product_id = goods.product_id " +
                "JOIN clients ON receipts.purchased_id = clients.client_id;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }
        //
        //Додавання нового замовлення з існуючим покупцем
        //
        private void ComboBoxPIB(ComboBox comboBox)
        {
            string query = "SELECT last_name, first_name FROM clients;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                string lastName = reader.GetString(0);
                string firstName = reader.GetString(1);
                string fullName = $"{lastName}, {firstName}";
                comboBox.Items.Add(fullName);
            }
            reader.Close();
        }
        private void ComboBoxTovar(ComboBox comboBox)
        {
            string query = "SELECT product_name FROM goods;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Будь ласка, введіть всі дані перед додаванням запису.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string PIB = comboBox1.SelectedItem.ToString();
            string[] parts = PIB.Split(',');
            string lastName = parts[0].Trim();
            string firstName = parts[1].Trim();
            string sqlPIB = "SELECT client_id FROM clients WHERE first_name = @FirstName AND last_name = @LastName";
            SqlCommand command = new SqlCommand(sqlPIB, sqlConnection);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            object result1 = command.ExecuteScalar();

            int clientId;

            if (result1 != null && result1 != DBNull.Value)
            {
                if (int.TryParse(result1.ToString(), out clientId))
                {
                    string selectedProduct = comboBox2.SelectedItem.ToString();
                    string sqlselectedProduct = "SELECT product_id, product_price FROM goods WHERE product_name = @ProductName";
                    command = new SqlCommand(sqlselectedProduct, sqlConnection);
                    command.Parameters.AddWithValue("@ProductName", selectedProduct);
                    SqlDataReader reader = command.ExecuteReader();

                    int productId;
                    decimal productPrice;

                    if (reader.Read())
                    {
                        productId = reader.GetInt32(0);
                        productPrice = reader.GetDecimal(1);
                        reader.Close();

                        string amountText = textBox1.Text;
                        decimal amount;

                        if (decimal.TryParse(amountText, out amount))
                        {
                            decimal totalCost = amount * productPrice;
                            string query = "INSERT INTO receipts (amount, cost, purchased_product_id, purchased_id) VALUES (@Amount, @TotalCost, @ProductId, @ClientId)";
                            command = new SqlCommand(query, sqlConnection);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@TotalCost", totalCost);
                            command.Parameters.AddWithValue("@ProductId", productId);
                            command.Parameters.AddWithValue("@ClientId", clientId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Запис успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося додати запис.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Введіть правильне ціле значення для кількості.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Товар не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Помилка при перетворенні ідентифікатора клієнта.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Клієнт не знайдений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //
        //Оновлення
        //
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ComboBoxPIB(comboBox1);
            ComboBoxTovar(comboBox2);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT receipts.amount, receipts.cost, goods.product_name, goods.product_unit, " +
                 "clients.first_name, clients.last_name, clients.city, clients.region FROM receipts " +
                 "JOIN goods ON receipts.purchased_product_id = goods.product_id " +
                 "JOIN clients ON receipts.purchased_id = clients.client_id;", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView2.DataSource = dataSet.Tables[0];
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ComboBoxProductUnit(comboBox3);
            ComboBoxProducer(comboBox6);
            ComboBoxCategory(comboBox7);
            ComboBoxSupplier(comboBox8);
        }
        //
        //Додавання нового покупця
        //
        private bool IsLatinLettersOnly(string input)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            return regex.IsMatch(input);
        }
        private void comboBoxCities_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCity = comboBox5.SelectedItem.ToString();

            if (cityRegions.ContainsKey(selectedCity))
            {
                comboBox4.DataSource = cityRegions[selectedCity];
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null || comboBox5.SelectedItem == null || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Будь ласка, введіть всі дані перед додаванням клієнта.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string firstName = textBox3.Text;
            string lastName = textBox4.Text;
            string city = comboBox5.SelectedItem.ToString();
            string region = comboBox4.SelectedItem.ToString();

            if (!IsLatinLettersOnly(firstName) || !IsLatinLettersOnly(lastName))
            {
                MessageBox.Show("Будь ласка, введіть ім'я та прізвище лише на латиниці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = $"INSERT INTO clients (first_name, last_name, city, region) VALUES ('{firstName}', '{lastName}', '{city}', '{region}');";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Нового клієнта успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не вдалося додати нового клієнта.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні нового клієнта: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //
        //Додавання нової категорії
        //
        private void AddNewCategoryToDatabase(string categoryName)
        {
            string query = "INSERT INTO category (category_name) VALUES (@CategoryName)";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@CategoryName", categoryName);
                command.ExecuteNonQuery();
            }
        }
        private bool CategoryExists(string categoryName)
        {
            string query = "SELECT COUNT(*) FROM category WHERE category_name = @CategoryName";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@CategoryName", categoryName);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string new_categoty = textBox2.Text;
            
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Будь ласка, введіть дані перед додаванням нової категорії.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsLatinLettersOnly(new_categoty))
            {
                MessageBox.Show("Будь ласка, введіть назву категорії лише на латиниці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CategoryExists(new_categoty))
            {
                MessageBox.Show("Категорія з такою назвою вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddNewCategoryToDatabase(new_categoty);
            MessageBox.Show("Нову категорію успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //
        //Додавання нових виробників
        //
        private void AddNewProducersToDatabase(string producers_name)
        {
            string query = "INSERT INTO producers (producers_name) VALUES (@producers_name)";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@producers_name", producers_name);
                command.ExecuteNonQuery();
            }
        }
        private bool ProducersExists(string producersName)
        {
            string query = "SELECT COUNT(*) FROM producers WHERE producers_name = @producers_name";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@producers_name", producersName);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string new_producers = textBox5.Text;

            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Будь ласка, введіть дані перед додаванням нового виробника.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsLatinLettersOnly(new_producers))
            {
                MessageBox.Show("Будь ласка, введіть назву виробника лише на латиниці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ProducersExists(new_producers))
            {
                MessageBox.Show("Виробник з такою назвою вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddNewProducersToDatabase(new_producers);
            MessageBox.Show("Нового виробника успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //
        //Додавання нових постачальників
        //
        private bool IsLatinLettersAndNumbers(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z]+(\s+""[a-zA-Z]+"")?$");
            return regex.IsMatch(input);
        }
        private void AddNewSuppliersToDatabase(string supplier_name, DateTime contract_exp_dat)
        {
            string query = "INSERT INTO suppliers (supplier_name, contract_exp_date) VALUES (@supplier_name, @contract_exp_date)";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@supplier_name", supplier_name);
                command.Parameters.AddWithValue("@contract_exp_date", contract_exp_dat);
                command.ExecuteNonQuery();
            }
        }
        private bool SuppliersExists(string supplier_name)
        {
            string query = "SELECT COUNT(*) FROM suppliers WHERE supplier_name = @supplier_name";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@supplier_name", supplier_name);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            string new_suppliers = textBox6.Text;
            DateTime contract_exp_dat = monthCalendar1.SelectionStart;

            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Будь ласка, введіть дані перед додаванням нового постачальника.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsLatinLettersAndNumbers(new_suppliers))
            {
                MessageBox.Show("Будь ласка, введіть назву постачальника лише на латиниці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SuppliersExists(new_suppliers))
            {
                MessageBox.Show("Постачальник з такою назвою вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (contract_exp_dat < DateTime.Today)
            {
                MessageBox.Show("Оберіть дату, яка не є минулою.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddNewSuppliersToDatabase(new_suppliers, contract_exp_dat);
            MessageBox.Show("Нового постачальника успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //
        //Додавання нового товару
        //
        private void ComboBoxProductUnit(ComboBox comboBox)
        {
            string query = "SELECT DISTINCT product_unit FROM goods;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }
        private void ComboBoxProducer(ComboBox comboBox)
        {
            string query = "SELECT producers_name FROM producers;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }
        private void ComboBoxCategory(ComboBox comboBox)
        {
            string query = "SELECT category_name FROM category;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }
        private void ComboBoxSupplier(ComboBox comboBox)
        {
            string query = "SELECT supplier_name FROM suppliers;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }
        private bool ProductExists(string product_name)
        {
            string query = "SELECT COUNT(*) FROM goods WHERE product_name = @product_name";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@product_name", product_name);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox7.Text) || comboBox3.SelectedItem == null || string.IsNullOrEmpty(textBox8.Text) || comboBox6.SelectedItem == null || comboBox7.SelectedItem == null || comboBox8.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, введіть дані перед додаванням нового товару.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string product_name = textBox7.Text;
            string product_unit = comboBox3.SelectedItem.ToString();
            string productPriceText = textBox8.Text;
            decimal product_price;
            string product_producer_name = comboBox6.SelectedItem.ToString();
            string product_category_name = comboBox7.SelectedItem.ToString();
            string product_supplier_name = comboBox8.SelectedItem.ToString();

            if (!IsLatinLettersAndNumbers(product_name))
            {
                MessageBox.Show("Будь ласка, введіть назву лише на латиниці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ProductExists(product_name))
            {
                MessageBox.Show("Товар з такою назвою вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int product_producer_id;
            int product_category_id;
            int product_supplier_id;

            // Отримання ідентифікатора виробника
            string queryProducer = $"SELECT producers_id FROM producers WHERE producers_name = '{product_producer_name}'";
            using (SqlCommand command = new SqlCommand(queryProducer, sqlConnection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    product_producer_id = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Виробник не знайдений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Отримання ідентифікатора категорії
            string queryCategory = $"SELECT category_id FROM category WHERE category_name = '{product_category_name}'";
            using (SqlCommand command = new SqlCommand(queryCategory, sqlConnection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    product_category_id = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Категорія не знайдена.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Отримання ідентифікатора постачальника
            string querySupplier = $"SELECT supplier_id FROM suppliers WHERE supplier_name = '{product_supplier_name}'";
            using (SqlCommand command = new SqlCommand(querySupplier, sqlConnection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    product_supplier_id = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("Постачальник не знайдений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (decimal.TryParse(productPriceText, out product_price))
            {
                string query = "INSERT INTO goods (product_name, product_unit, product_price, product_producer_id, " +
                               "product_category_id, product_supplier_id) VALUES (@product_name, @product_unit, @product_price, " +
                               "@product_producer_id, @product_category_id, @product_supplier_id)";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@product_name", product_name);
                    command.Parameters.AddWithValue("@product_unit", product_unit);
                    command.Parameters.AddWithValue("@product_price", product_price);
                    command.Parameters.AddWithValue("@product_producer_id", product_producer_id);
                    command.Parameters.AddWithValue("@product_category_id", product_category_id);
                    command.Parameters.AddWithValue("@product_supplier_id", product_supplier_id);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Новий товар успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Введіть коректну ціну товару.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}