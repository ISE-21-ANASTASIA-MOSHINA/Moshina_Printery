using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<TypographerViewModel> list = Task.Run(() => APIClient.GetRequestData<List<TypographerViewModel>>("api/Typographer/GetList")).Result;
                if (list != null)
                {
                    comboBoxTypographer.DisplayMember = "TypographerFIO";
                    comboBoxTypographer.ValueMember = "Number";


                    /*     =3     ^_^    =D      0_0      =(    >_<        */
                    /*     =3     ^_^    =D      0_0      =(    >_<        */
                    /*     =3     ^_^    =D      0_0      =(    >_<        */
                    /*     =3     ^_^    =D      0_0      =(    >_<        */
                    /*     =3     ^_^    =D      0_0      =(    >_<        */
                    comboBoxTypographer.DataSource = list;
                    comboBoxTypographer.SelectedItem = null;
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
            if (comboBoxTypographer.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var huiniya = comboBoxTypographer.SelectedValue;
                int typographerId = Convert.ToInt32(comboBoxTypographer.SelectedValue);

                /*     =3     ^_^    =D      0_0      =(    >_<        */
                /*     =3     ^_^    =D      0_0      =(    >_<        */
                /*     =3     ^_^    =D      0_0      =(    >_<        */
                /*     =3     ^_^    =D      0_0      =(    >_<        */
                /*     =3     ^_^    =D      0_0      =(    >_<        */
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/TakeBookingInWork", new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = typographerId
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заказ передан в работу. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

        private void comboBoxTypographer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
