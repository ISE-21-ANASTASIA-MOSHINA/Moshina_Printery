using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PrinterySVC.ViewModel;
using PrinterySVC.BindingModel;

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
                var responseC = APIClient.GetRequest("api/Material/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<MaterialViewModel> list = APIClient.GetElement<List<MaterialViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxMaterial.DisplayMember = "MaterialName";
                        comboBoxMaterial.ValueMember = "Number";
                        comboBoxMaterial.DataSource = list;
                        comboBoxMaterial.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Rack/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<RackViewModel> list = APIClient.GetElement<List<RackViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxRack.DisplayMember = "RackName";
                        comboBoxRack.ValueMember = "Number";
                        comboBoxRack.DataSource = list;
                        comboBoxRack.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
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
                var response = APIClient.PostRequest("api/Main/PutMaterialOnRack", new RackMaterialBindingModel
                {
                    MaterialNamber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
                    RackNamber = Convert.ToInt32(comboBoxRack.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
