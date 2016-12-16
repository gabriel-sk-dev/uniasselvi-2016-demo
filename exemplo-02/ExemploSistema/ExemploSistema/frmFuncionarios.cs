using Dapper;
using Dapper.Contrib.Extensions;
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
            var listOfEmployess = Enumerable.Empty<Employee>();
            using (var connection = new SqlConnection(connectionString))
                listOfEmployess = connection.Query<Employee>("SELECT * FROM EMPLOYEES");
            
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
            if (_isNewEmployee)
                using (var connection = new SqlConnection(connectionString))
                    connection.Insert<Employee>(_selectedEmployee);
            else
                using (var connection = new SqlConnection(connectionString))
                    connection.Update<Employee>(_selectedEmployee);

            var listOfEmployess = Enumerable.Empty<Employee>();
            using (var connection = new SqlConnection(connectionString))
                listOfEmployess = connection.Query<Employee>("SELECT * FROM EMPLOYEES");

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
