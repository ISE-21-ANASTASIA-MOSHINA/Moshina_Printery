using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractPrinteryView
{
    public partial class FormTakeOrderInWork : Form
    {

        public int Number { set { id = value; } }

        private int? id;

        public FormTakeOrderInWork()
        {
            InitializeComponent();
        }

        private void FormTakeOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Typographer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<TypographerViewModel> list = APIClient.GetElement<List<TypographerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxTypographer.DisplayMember = "TypographerFIO";
                        comboBoxTypographer.ValueMember = "Number";
                        comboBoxTypographer.DataSource = list;
                        comboBoxTypographer.SelectedItem = null;
                    }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxTypographer.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/TakeBookingInWork", new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = Convert.ToInt32(comboBoxTypographer.SelectedValue)
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

        private void comboBoxTypographer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
