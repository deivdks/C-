using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskProject.Classes;
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
            dgvTask.Rows[index].Cells["clnData"].Value = dtpData.Value.Date; 
            dgvTask.Rows[index].Cells["clnTitulo"].Value = txtTitulo.Text;
            dgvTask.Rows[index].Cells["clnHora"].Value = txtHoraDespendida.Text;
            dgvTask.Rows[index].Cells["clnDescricao"].Value = txtDescricao.Text;

        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (dgvTask.Tag == null)
            {
                /*String teste;
                teste = DateTime.Parse(txtData.Text, new CultureInfo("en-US")).ToString();
                teste = Convert.ToDateTime(txtData.Text).ToString("yyyy/MM/dd");*/

                if (dgvTask.Tag == null)
                {
                    Dictionary<string, object> dataTask = new Dictionary<string, object>();
                    dataTask.Add("data", dtpData.Value);
                    dgvTask.Tag = SqlConn.ExecuteScalar(SqlGenerate.InsertDataTask(), dataTask);
                }
            }

            for (int i = 0; i < dgvTask.RowCount -1; i++)
            { 
                DataGridViewRow r = dgvTask.Rows[i];
                int teste = int.Parse(r.Cells["clnId"].Value == null ? "0" : r.Cells["clnId"].Value.ToString());


                var task = new DailyTask(int.Parse(r.Cells["clnId"].Value == null?"0":r.Cells["clnId"].Value.ToString()),
                                                            int.Parse(dgvTask.Tag == null ? "0": dgvTask.Tag.ToString()),
                                                            DateTime.Parse(r.Cells["clnData"].Value.ToString()),
                                                            r.Cells["clnTitulo"].Value.ToString(),
                                                            Convert.ToDouble(r.Cells["clnHora"].Value, new CultureInfo("en-US")),
                                                            r.Cells["clnDescricao"].Value.ToString());

                Dictionary<string, object> dataTaskDesc = new Dictionary<string, object>();
                dataTaskDesc.Add("id", task.Id);
                dataTaskDesc.Add("idTask", task.IdTask);
                dataTaskDesc.Add("titulo", task.Titulo);
                dataTaskDesc.Add("descricao", task.Descricao);
                dataTaskDesc.Add("hora", task.Hora);

                if (task.Id == 0)
                {
                    SqlConn.ExecuteNonQuery(SqlGenerate.InsertDescriptionTask(), dataTaskDesc);
                }else
                { 
                    SqlConn.ExecuteNonQuery(SqlGenerate.UpdatetDescriptionTask(), dataTaskDesc);
                }

            }
            LimpaCampos();
        }

        private void frmTask_Load(object sender, EventArgs e)
        {
            
        }

        private void LimpaCampos()
        {
            dgvTask.Rows.Clear();
            dtpData.Text = "";
            dtpData.Tag = "";
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

                Dictionary<string, object> dataTaskDesc = new Dictionary<string, object>();
                dataTaskDesc.Add("idDesc", Convert.ToInt32(dgvTask.CurrentRow.Cells["clnIdDesc"].Value));
                dataTaskDesc.Add("idTask", Convert.ToInt32(dtpData.Tag));

                if (dgvTask.CurrentRow.Cells["clnIdDesc"].Value != null)
                {
                    SqlConn.ExecuteNonQuery(SqlGenerate.DeleteDescription(), dataTaskDesc);
                }
                dgvTask.Rows.RemoveAt(dgvTask.CurrentRow.Index);

                if ((dgvTask.SelectedRows.Count -1) == 0) //Se não tem mais descrições, deleta task
                {
                    SqlConn.ExecuteNonQuery(SqlGenerate.DeleteTask(), dataTaskDesc);
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime Date;
            bool isDate = DateTime.TryParseExact(dtpData.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                         DateTimeStyles.None, out Date);
            if (isDate)
            {

                DataTable dt = SqlConn.Read(SqlGenerate.SelecttDataTask(dtpData.Text));
                if (dt.Rows.Count > 0)
                {
                    dtpData.Tag = dt.Rows[0][0];
                    dt = SqlConn.Read(SqlGenerate.SelectTasks(Convert.ToInt32(dtpData.Tag)));
                    // dgvSenhas.DataSource = dt;
                    foreach (DataRow r in dt.Rows)
                    {
                        int index = dgvTask.Rows.Add();
                        dgvTask.Rows[index].Cells["clnData"].Value = r["Data"].ToString().Replace("-", "/");
                        dgvTask.Rows[index].Cells["clnTitulo"].Value = r["Titulo"];
                        dgvTask.Rows[index].Cells["clnHora"].Value = r["Hora"];
                        dgvTask.Rows[index].Cells["clnDescricao"].Value = r["Descricao"];
                        dgvTask.Rows[index].Cells["clnIdDesc"].Value = r["id"];
                        dtpData.Tag = r["IdTask"];
                    }
                }
                else
                {
                    dgvTask.Rows.Clear();
                    dtpData.Tag = null;
                }
            }
            else
            {
                dtpData.Tag = null;
                dgvTask.Rows.Clear();
            }
        }
    }
}
