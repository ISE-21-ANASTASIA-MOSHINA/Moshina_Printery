using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<TypographerViewModel> list = Task.Run(() => APIClient.GetRequestData<List<TypographerViewModel>>("api/Typographer/GetList")).Result;
                if (list != null)
                {
                    comboBoxTypographer.DisplayMemberPath = "TypographerFIO";
                    comboBoxTypographer.SelectedValuePath = "Number";
                    comboBoxTypographer.ItemsSource = list;
                    comboBoxTypographer.SelectedItem = null;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxTypographer.SelectedItem == null)
            {
                MessageBox.Show("Выберите повара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int typographerNumber = Convert.ToInt32(comboBoxTypographer.SelectedValue);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/TakeBookingInWork", new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = typographerNumber
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заказ готовится. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
