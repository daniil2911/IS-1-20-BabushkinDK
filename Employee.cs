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

namespace IS1_20_BabushkinDK
{
    public partial class Employee : MetroFramework.Forms.MetroForm
    {
        //Переменная соединения
        MySqlConnection conn;
        //DataAdapter представляет собой объект Command , получающий данные из источника данных.
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table = new DataTable();
        //Переменная для ID записи в БД, выбранной в гриде. Пока она не содердит значения, лучше его инициализировать с 0
        //что бы в БД не отправлялся null
        string id_selected_rows = "0";
        public Employee()
        {
            InitializeComponent();
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            // строка подключения к БД
            //string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_20_2;database=is_1_20_st2_KURS;password=34354559;";
            string connStr = "server=10.90.12.110;port=33333;user=st_1_20_2;database=is_1_20_st2_KURS;password=34354559;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);
            //Вызываем метод для заполнение дата Грида
            GetListUsers();
            //Видимость полей в гриде
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;


            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;

            //Режим для полей "Только для чтения"
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;

            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;            
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main a = new Main();
            a.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //Магические строки
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.CurrentRow.Selected = true;
            //Метод получения ID выделенной строки в глобальную переменную
            GetSelectedIDString();
        }
        public void GetSelectedIDString()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
            //Указываем ID выделенной строки в метке
            toolStripLabel4.Text = id_selected_rows;
        }
        //Метод обновления DataGreed
        public void reload_list()
        {
            //Чистим виртуальную таблицу
            table.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListUsers();
        }
        //Метод удаления пользователей
        //public void DeleteUser()
        //{
        //    //Формируем строку запроса на добавление строк
        //    string sql_delete_user = "DELETE FROM t_stud WHERE id='" + id_selected_rows + "'";
        //    //Посылаем запрос на обновление данных
        //    MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
        //    try
        //    {
        //        conn.Open();
        //        delete_user.ExecuteNonQuery();
        //        MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        Application.Exit();
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        //Вызов метода обновления ДатаГрида
        //        reload_list();
        //    }
        //}
        //Метод изменения состояния студента
        //public void ChangeStateStudent(string new_state)
        //{
        //    //Получаем ID изменяемого студента
        //    string redact_id = id_selected_rows;
        //    // устанавливаем соединение с БД
        //    conn.Open();
        //    // запрос обновления данных
        //    string query2 = $"UPDATE t_stud SET id_state='{new_state}' WHERE (id='{redact_id}')";
        //    // объект для выполнения SQL-запроса
        //    MySqlCommand command = new MySqlCommand(query2, conn);
        //    // выполняем запрос
        //    command.ExecuteNonQuery();
        //    // закрываем подключение к БД
        //    conn.Close();
        //    //Обновляем DataGrid
        //    reload_list();
        //    //Красим опять грид
        //}
        //Метод наполнения виртуальной таблицы и присвоение её к датагриду
        public void GetListUsers()
        {
            //Запрос для вывода строк в БД
            string commandStr = "SELECT id_empl AS 'Код', fio_empl AS 'ФИО', phone_empl AS 'Телефон' FROM T_Empl";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            //Закрываем соединение
            conn.Close();
            //Отражаем количество записей в ДатаГриде
            int count_rows = dataGridView1.RowCount - 1;
            toolStripLabel2.Text = (count_rows).ToString();
        }


        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                //dataGridView1.CurrentRow.Selected = true;
                dataGridView1.CurrentCell.Selected = true;
                //Метод получения ID выделенной строки в глобальную переменную
                GetSelectedIDString();
            }
        }
    }
}



            
      
