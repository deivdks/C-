using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskProject.classes;
using System.Data.SQLite;
using System.Globalization;

namespace TaskProject
{
    public partial class frmTask : Form
    {
        public frmTask()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteConnection sqliteConnection;
            sqliteConnection = sqlConn.DbConnection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 47 && e.KeyChar != 8)
            {
                e.Handled = true;
            }


        }

        private void txtData_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            int index = dgvTask.Rows.Add();
            dgvTask.Rows[index].Cells["clnData"].Value = txtData.Text; 
            dgvTask.Rows[index].Cells["clnTitulo"].Value = txtTitulo.Text;
            dgvTask.Rows[index].Cells["clnHora"].Value = txtHoraDespendida.Text;
            dgvTask.Rows[index].Cells["clnDescricao"].Value = txtDescricao.Text;


        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (txtData.Tag == null)
            {
                /*String teste;
                teste = DateTime.Parse(txtData.Text, new CultureInfo("en-US")).ToString();
                teste = Convert.ToDateTime(txtData.Text).ToString("yyyy/MM/dd");*/

                if (txtData.Tag == null)
                {
                    sqlConn.Execute(SqlGenerate.InsertDataTask(txtData.Text), sqlConn.DbConnection());
                    DataTable Id = sqlConn.Read(SqlGenerate.GetLastIdDataTask(), sqlConn.DbConnection());
                    txtData.Tag = Id.Rows[0].ItemArray[0];
                }
            }

            for (int i = 0; i < dgvTask.RowCount -1; i++)
            { 
                DataGridViewRow r = dgvTask.Rows[i];
                TaskProject.classes.Task task = new TaskProject.classes.Task(DateTime.Parse(r.Cells["clnData"].Value.ToString()),
                                                            r.Cells["clnTitulo"].Value.ToString(),
                                                            Convert.ToDouble(r.Cells["clnHora"].Value, new CultureInfo("en-US")),
                                                            r.Cells["clnDescricao"].Value.ToString());

                if (r.Cells["clnIdDesc"].Value == null || r.Cells["clnIdDesc"].Value.ToString() == "")
                {
                    sqlConn.Execute(SqlGenerate.InsertDescriptionTask(task, int.Parse(txtData.Tag.ToString())), sqlConn.DbConnection());
                }else
                { 
                    sqlConn.Execute(SqlGenerate.UpdatetDescriptionTask(task, Convert.ToInt32(r.Cells["clnIdDesc"].Value), Convert.ToInt32(txtData.Tag)), sqlConn.DbConnection());
                }

            }
            LimpaCampos();
        }

        private void frmTask_Load(object sender, EventArgs e)
        {
            txtData.Text = DateTime.Today.ToString();
        }

        private void txtData_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtData_KeyPress_2(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;
            }

        }

        private void txtData_TextChanged_1(object sender, EventArgs e)
        {
            DateTime Date;
            bool isDate = DateTime.TryParseExact(txtData.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                         DateTimeStyles.None, out Date);
            if (isDate)
            {

                DataTable dt = sqlConn.Read(SqlGenerate.SelecttDataTask(txtData.Text), sqlConn.DbConnection());
                if (dt.Rows.Count > 0)
                {
                    txtData.Tag = dt.Rows[0][0];
                    dt = sqlConn.Read(SqlGenerate.SelectTasks(Convert.ToInt32(txtData.Tag)), sqlConn.DbConnection());
                    // dgvSenhas.DataSource = dt;
                    foreach (DataRow r in dt.Rows)
                    {
                        int index = dgvTask.Rows.Add();
                        dgvTask.Rows[index].Cells["clnData"].Value = r["Data"].ToString().Replace("-", "/");
                        dgvTask.Rows[index].Cells["clnTitulo"].Value = r["Titulo"];
                        dgvTask.Rows[index].Cells["clnHora"].Value = r["Hora"];
                        dgvTask.Rows[index].Cells["clnDescricao"].Value = r["Descricao"];
                        dgvTask.Rows[index].Cells["clnIdDesc"].Value = r["id"];
                        txtData.Tag = r["IdTask"];
                    }
                }
                else
                {
                    dgvTask.Rows.Clear();
                    txtData.Tag = null;
                }
            }else
            {
                txtData.Tag = null;
                dgvTask.Rows.Clear();
            }
        }
        private void LimpaCampos()
        {
            dgvTask.Rows.Clear();
            txtData.Text = "";
            txtData.Tag = null;
            txtTitulo.Text = "";
            txtHoraDespendida.Text = "";
            txtDescricao.Text = "";
            btnExcluir.Enabled = false;
            btnExportar.Enabled = false;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count > 0)
            {
                if (dgvTask.CurrentRow.Cells["clnIdDesc"].Value != null)
                {
                    sqlConn.Execute(SqlGenerate.DeleteDescription(Convert.ToInt32(dgvTask.CurrentRow.Cells["clnIdDesc"].Value), Convert.ToInt32(txtData.Tag)), sqlConn.DbConnection());
                }
                dgvTask.Rows.RemoveAt(dgvTask.CurrentRow.Index);

                if ((dgvTask.SelectedRows.Count -1) == 0) //Se não tem mais descrições, deleta task
                {
                    sqlConn.Execute(SqlGenerate.DeleteTask(Convert.ToInt32(txtData.Tag)), sqlConn.DbConnection());
                }
            }
            else
            {
                MessageBox.Show("Selecione um registro para exclusão.");
            }
            

        }

        private void dgvTask_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnExcluir.Enabled = true;
                btnExportar.Enabled = true;
            }else
            {
                btnExcluir.Enabled = false;
                btnExportar.Enabled = false;
            }
                
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            // Código pego na internet e adaptado para este projeto

            StringBuilder sb = new StringBuilder();
            int RowCount = dgvTask.RowCount;
            int ColumnCount = dgvTask.ColumnCount -1;

            // get column headers
            for (int currentCol = 0; currentCol < ColumnCount; currentCol++)
            {
                sb.Append(dgvTask.Columns[currentCol].HeaderText.ToString());
                if (currentCol < ColumnCount - 1 && dgvTask.Columns[currentCol].Name != "clnIdDesc")
                {
                    sb.Append(";");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            // get the rows data
            for (int currentRow = 0; currentRow < RowCount; currentRow++)
            {
                if (!dgvTask.Rows[currentRow].IsNewRow)
                {
                    for (int currentCol = 0; currentCol < ColumnCount; currentCol++)
                    {
                        if (dgvTask.Rows[currentRow].Cells[currentCol].Value != null)
                        {
                            sb.Append(dgvTask.Rows[currentRow].Cells[currentCol].Value.ToString());
                        }
                        if (currentCol < ColumnCount - 1)
                        {
                            sb.Append(";");
                        }
                        else
                        {
                            sb.AppendLine();
                        }
                    }
                }
            }
            System.IO.File.WriteAllText(@"C:\Users\deivd.silva\Documents\Deivd Krug\C#\TaskProject\TaskProject\DGV_CSV_EXPORT.csv", sb.ToString(), Encoding.Default);
        }
    }
}
