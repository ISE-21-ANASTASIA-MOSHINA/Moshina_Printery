using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PrinterySVC.ViewModel;
using PrinterySVC.BindingModel;
using System.Threading.Tasks;

namespace AbstractPrinteryView
{
    public partial class FormPutOnRack : Form
    {

        public FormPutOnRack()
        {
            InitializeComponent();
        }

        private void FormPutOnRack_Load(object sender, EventArgs e)
        {

            try
            {
                List<MaterialViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<MaterialViewModel>>("api/Material/GetList")).Result;
                if (listC != null)
                {
                    comboBoxMaterial.DisplayMember = "MaterialName";
                    comboBoxMaterial.ValueMember = "Number";
                    comboBoxMaterial.DataSource = listC;
                    comboBoxMaterial.SelectedItem = null;
                }

                List<RackViewModel> listS = Task.Run(() => APIClient.GetRequestData<List<RackViewModel>>("api/Rack/GetList")).Result;
                if (listS != null)
                {
                    comboBoxRack.DisplayMember = "RackName";
                    comboBoxRack.ValueMember = "Number";
                    comboBoxRack.DataSource = listS;
                    comboBoxRack.SelectedItem = null;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMaterial.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxRack.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int materialId = Convert.ToInt32(comboBoxMaterial.SelectedValue);
                int rackId = Convert.ToInt32(comboBoxRack.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutMaterialOnRack", new RackMaterialBindingModel
                {
                    MaterialNamber = materialId,
                    RackNamber = rackId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Склад пополнен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxRack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
