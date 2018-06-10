using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrinterySVC.ViewModel;
using PrinterySVC.BindingModel;
using System.Net.Http;

namespace AbstractPrinteryView
{
    public partial class FormRack : Form
    {

        public int Number { set { id = value; } }

        private int? id;

        public FormRack()
        {
            InitializeComponent();
        }

        private void FormRack_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var rack = Task.Run(() => APIClient.GetRequestData<RackViewModel>("api/Rack/Get/" + id.Value)).Result;
                    textBoxName.Text = rack.RackName;
                    dataGridView.DataSource = rack.RackMaterial;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Rack/UpdElement", new RackBindingModel
                {
                    Number = id.Value,
                    RackName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Rack/AddElement", new RackBindingModel
                {
                    RackName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
