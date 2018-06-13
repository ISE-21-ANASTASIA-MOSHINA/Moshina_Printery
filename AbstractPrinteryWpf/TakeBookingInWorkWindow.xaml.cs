using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для TakeBookingInWorkWindow.xaml
    /// </summary>
    public partial class TakeBookingInWorkWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public TakeBookingInWorkWindow()
        {
            InitializeComponent();
            Loaded += TakeBookingInWorkWindow_Load;
        }

        private void TakeBookingInWorkWindow_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указана заявка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Typographer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<TypographerViewModel> list = APIClient.GetElement<List<TypographerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxTypographer.DisplayMemberPath = "TypographerFIO";
                        comboBoxTypographer.SelectedValuePath = "Id";
                        comboBoxTypographer.ItemsSource = list;
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxTypographer.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/TakeBookingInWork", new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = ((TypographerViewModel)comboBoxTypographer.SelectedItem).Number,
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
