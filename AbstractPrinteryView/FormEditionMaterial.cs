using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PrinterySVC.ViewModel;

namespace AbstractPrinteryView
{
    public partial class FormEditionMaterial : Form
    {

        public EditionMaterialViewModel Model { set { model = value; } get { return model; } }

        private EditionMaterialViewModel model;

        public FormEditionMaterial()
        {
            InitializeComponent();
        }

        private void FormEditionMaterial_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Material/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxMaterial.DisplayMember = "MaterialName";
                    comboBoxMaterial.ValueMember = "Number";
                    comboBoxMaterial.DataSource = APIClient.GetElement<List<MaterialViewModel>>(response);
                    comboBoxMaterial.SelectedItem = null;
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxMaterial.Enabled = false;
                comboBoxMaterial.SelectedValue = model.MaterialNamber;
                textBoxCount.Text = model.Count.ToString();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
