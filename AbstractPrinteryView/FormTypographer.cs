using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractPrinteryView
{
    public partial class FormTypographer : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormTypographer()
        {
            InitializeComponent();
        }

        private void FormTypographer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var typographer = Task.Run(() => APIClient.GetRequestData<TypographerViewModel>("api/Typographer/Get/" + id.Value)).Result;
                    textBoxFIO.Text = typographer.TypographerFIO;
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
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string fio = textBoxFIO.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Typographer/UpdElement", new TypographerBildingModel
                {
                    Number = id.Value,
                    TypographerFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Typographer/AddElement", new TypographerBildingModel
                {
                    TypographerFIO = fio
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
            Close();
        }
    }
}
