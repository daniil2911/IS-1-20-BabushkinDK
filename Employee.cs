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
        public Employee()
        {
            InitializeComponent();
        }

        private void Employee_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show(
                "Вы действительно хотите выйти из сотрудников?",
                "Завершение формы сотрудники",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
               );
            if (dialog == DialogResult.Yes)
            {
                this.Close();
                Application.Exit();
            }
        }
    }
    
}
