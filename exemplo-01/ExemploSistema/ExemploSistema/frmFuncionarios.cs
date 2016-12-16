using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExemploSistema
{
    public partial class frmFuncionarios : Form
    {
        private bool _isNewEmployee;
        private Employee _selectedEmployee;

        public frmFuncionarios()
        {
            InitializeComponent();
        }

        private void frmFuncionarios_Load(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;

            var listOfEmployess = new List<Employee>();
            var selectCommand = @"SELECT * FROM Employees";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(selectCommand, connection))
            {
                connection.Open();
                var dataReaded = command.ExecuteReader();
                while (dataReaded.Read())
                {
                    var employee = new Employee();
                    employee.Id = dataReaded.GetInt32(0);
                    employee.Name = dataReaded.GetString(1);
                    var civilState = dataReaded.GetString(2);
                    switch (civilState)
                    {
                        case "C":
                            employee.CivilState = "Casado";
                            break;
                        case "S":
                            employee.CivilState = "Solteiro";
                            break;
                        default:
                            employee.CivilState = "N/A";
                            break;
                    }
                    employee.Salary = dataReaded.GetDecimal(3);
                    listOfEmployess.Add(employee);
                }
                connection.Close();
            }

            dataGrid.DataSource = listOfEmployess;
            dataGrid.Columns[0].HeaderText = "Id";
            dataGrid.Columns[1].HeaderText = "Nome";
            dataGrid.Columns[2].HeaderText = "Estado Civil";
            dataGrid.Columns[3].HeaderText = "Salário";
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 1)
            {
                _selectedEmployee = new Employee();
                if (dataGrid.SelectedRows.Count > 0)
                    _selectedEmployee = dataGrid.SelectedRows[0].DataBoundItem as Employee;

                txtName.Text = _selectedEmployee.Name;
                cboCivilState.Text = _selectedEmployee.CivilState;
                txtSalary.Text = _selectedEmployee.Salary.ToString();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ((Control)tabList).Enabled = false;
            tabControl.SelectTab(1);

            txtName.Enabled = true;
            cboCivilState.Enabled = true;
            txtSalary.Enabled = true;

            txtName.Text = "";
            cboCivilState.Text = "";
            txtSalary.Text = "";

            _selectedEmployee = new Employee();
            _isNewEmployee = true;
            
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ((Control)tabList).Enabled = true;
            txtName.Enabled = false;
            cboCivilState.Enabled = false;
            txtSalary.Enabled = false;

            _selectedEmployee = new Employee();

            tabControl.SelectTab(0);

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Selecione um funcionário");
                return;
            }

            ((Control)tabList).Enabled = false;
            txtName.Enabled = true;
            cboCivilState.Enabled = true;
            txtSalary.Enabled = true;

            _selectedEmployee = dataGrid.SelectedRows[0].DataBoundItem as Employee;

            txtName.Text = _selectedEmployee.Name;
            cboCivilState.Text = _selectedEmployee.CivilState;
            txtSalary.Text = _selectedEmployee.Salary.ToString();
            _isNewEmployee = false;

            tabControl.SelectTab(1);

            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _selectedEmployee.Name = txtName.Text;
            _selectedEmployee.CivilState = cboCivilState.SelectedItem.ToString();
            _selectedEmployee.Salary = Convert.ToDecimal(txtSalary.Text);

            var connectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
            var commandSql = "";

            if (_isNewEmployee)
                commandSql = @"INSERT INTO EMPLOYEES (NAME, CIVILSTATE, SALARY) VALUES (@Name, @CivilState, @Salary)";
            else
                commandSql = @"UPDATE EMPLOYEES SET NAME = @Name, CIVILSTATE = @CivilState, SALARY = @Salary WHERE ID = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(commandSql, connection))
            {
                var nameParam = new SqlParameter("@Name", _selectedEmployee.Name);
                command.Parameters.Add(nameParam);

                var civilState = "";
                switch (_selectedEmployee.CivilState)
                {
                    case "Casado":
                        civilState = "C";
                        break;
                    case "Solteiro":
                        civilState = "S";
                        break;
                    default:
                        break;
                }
                var civilStateParam = new SqlParameter("@CivilState", civilState);
                command.Parameters.Add(civilStateParam);

                var salarayParam = new SqlParameter("@Salary", _selectedEmployee.Salary);
                command.Parameters.Add(salarayParam);

                var idParam = new SqlParameter("@Id", _selectedEmployee.Id);
                command.Parameters.Add(idParam);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            var listOfEmployess = new List<Employee>();
            var selectCommand = @"SELECT * FROM Employees";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(selectCommand, connection))
            {
                connection.Open();
                var dataReaded = command.ExecuteReader();
                while (dataReaded.Read())
                {
                    var employee = new Employee();
                    employee.Id = dataReaded.GetInt32(0);
                    employee.Name = dataReaded.GetString(1);
                    var civilState = dataReaded.GetString(2);
                    switch (civilState)
                    {
                        case "C":
                            employee.CivilState = "Casado";
                            break;
                        case "S":
                            employee.CivilState = "Solteiro";
                            break;
                        default:
                            employee.CivilState = "N/A";
                            break;
                    }
                    employee.Salary = dataReaded.GetDecimal(3);
                    listOfEmployess.Add(employee);
                }
                connection.Close();
            }

            dataGrid.DataSource = listOfEmployess;
            dataGrid.Columns[0].HeaderText = "Id";
            dataGrid.Columns[1].HeaderText = "Nome";
            dataGrid.Columns[2].HeaderText = "Estado Civil";
            dataGrid.Columns[3].HeaderText = "Salário";

            ((Control)tabList).Enabled = true;
            txtName.Enabled = false;
            cboCivilState.Enabled = false;
            txtSalary.Enabled = false;

            _selectedEmployee = new Employee();

            tabControl.SelectTab(0);

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }
    }
}
