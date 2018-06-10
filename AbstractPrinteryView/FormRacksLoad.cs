using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AbstractPrinteryView
{
    public partial class FormRacksLoad : Form
    {
        public FormRacksLoad()
        {
            InitializeComponent();
        }

        private void FormRacksLoad_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView.Rows.Clear();
                foreach (var elem in Task.Run(() => APIClient.GetRequestData<List<RacksLoadViewModel>>("api/Report/GetRacksLoad")).Result)
                {
                    dataGridView.Rows.Add(new object[] { elem.RackName, "", "" });
                    foreach (var listElem in elem.Materials)
                    {
                        dataGridView.Rows.Add(new object[] { "", listElem.MaterialName, listElem.Count });
                    }
                    dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                    dataGridView.Rows.Add(new object[] { });
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonSaveToExcel_Click_1(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveRacksLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }     
    }
}
