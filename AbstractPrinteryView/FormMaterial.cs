using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryView
{
    public partial class FormMaterial : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IMaterialSVC service;

        private int? id;

        public FormMaterial(IMaterialSVC service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormComponent_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    MaterialViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.MaterialName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
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
                task = Task.Run(() => APIClient.PostRequestData("api/Material/UpdElement", new MaterialBindingModel
                {
                    Number = id.Value,
                    MaterialName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Material/AddElement", new MaterialBindingModel
                {
                    MaterialName = name
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
    }
}
