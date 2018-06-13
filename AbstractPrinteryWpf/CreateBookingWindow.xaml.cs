using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
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
                var responseC = APIClient.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerVievModel> list = APIClient.GetElement<List<CustomerVievModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                        comboBoxCustomer.SelectedValuePath = "Number";
                        comboBoxCustomer.ItemsSource = list;
                        comboBoxEdition.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/Edition/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<EditionViewModel> list = APIClient.GetElement<List<EditionViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxEdition.DisplayMemberPath = "EditionName";
                        comboBoxEdition.SelectedValuePath = "Number";
                        comboBoxEdition.ItemsSource = list;
                        comboBoxEdition.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }

            }
            catch (Exception ex)
            {
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
                    var responseP = APIClient.GetRequest("api/Edition/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        EditionViewModel mebel = APIClient.GetElement<EditionViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)mebel.Coast).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateBooking", new BookingBindingModel
                {
                    CustomerNumber = Convert.ToInt32(comboBoxCustomer.SelectedValue),
                    EditionNumber = Convert.ToInt32(comboBoxEdition.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
