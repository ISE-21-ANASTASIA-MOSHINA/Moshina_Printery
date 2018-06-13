using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для CreateBookingWindow.xaml
    /// </summary>
    public partial class CreateBookingWindow : Window
    {
        public CreateBookingWindow()
        {
            InitializeComponent();
            Loaded += CreateBookingWindow_Load;
            comboBoxEdition.SelectionChanged += comboBoxEdition_SelectedIndexChanged;
            comboBoxEdition.SelectionChanged += new SelectionChangedEventHandler(comboBoxEdition_SelectedIndexChanged);
        }

        private void CreateBookingWindow_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerVievModel> listC = Task.Run(() => APIClient.GetRequestData<List<CustomerVievModel>>("api/Customer/GetList")).Result;
                if (listC != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                    comboBoxCustomer.SelectedValuePath = "Number";
                    comboBoxCustomer.ItemsSource = listC;
                    comboBoxEdition.SelectedItem = null;
                }

                List<EditionViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<EditionViewModel>>("api/Edition/GetList")).Result;
                if (listP != null)
                {
                    comboBoxEdition.DisplayMemberPath = "EditionName";
                    comboBoxEdition.SelectedValuePath = "Number";
                    comboBoxEdition.ItemsSource = listP;
                    comboBoxEdition.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxEdition.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((EditionViewModel)comboBoxEdition.SelectedItem).Number;
                    EditionViewModel edition = Task.Run(() => APIClient.GetRequestData<EditionViewModel>("api/Edition/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)edition.Coast).ToString();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxCustomer.SelectedItem == null)
            {
                MessageBox.Show("Выберите получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxEdition.SelectedItem == null)
            {
                MessageBox.Show("Выберите мебель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int customerNumber = Convert.ToInt32(comboBoxCustomer.SelectedValue);
            int editionNumber = Convert.ToInt32(comboBoxEdition.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Main/CreateBooking", new BookingBindingModel
            {
                CustomerNumber = customerNumber,
                EditionNumber = editionNumber,
                Count = count,
                Sum = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
