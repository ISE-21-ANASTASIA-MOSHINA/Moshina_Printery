using Microsoft.Win32;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Main/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<BookingViewModel> list = APIClient.GetElement<List<BookingViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewMain.ItemsSource = list;
                        dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
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

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CustomersWindow();
            form.ShowDialog();
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MaterialsWindow();
            form.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new EditionsWindow();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new RacksWindow();
            form.ShowDialog();
        }

        private void работникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TypographersWindow();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new PutOnRackWindow();
            form.ShowDialog();
        }

        private void buttonCreateBooking_Click(object sender, EventArgs e)
        {
            var form = new CreateBookingWindow();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeBookingInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = new TakeBookingInWorkWindow();
                form.Id = ((BookingViewModel)dataGridViewMain.SelectedItem).Number;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonBookingReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((BookingViewModel)dataGridViewMain.SelectedItem).Number;
                try
                {
                    var response = APIClient.PostRequest("api/Main/FinishBooking", new BookingBindingModel
                    {
                        Number = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        private void buttonPayBooking_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((BookingViewModel)dataGridViewMain.SelectedItem).Number;
                try
                {
                    var response = APIClient.PostRequest("api/Main/PayBooking", new BookingBindingModel
                    {
                        Number = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсИзделийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {

                try
                {

                    var response = APIClient.PostRequest("api/Report/SaveEditionPrice", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void загруженностьСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveRacksLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CustomerBookingsWindow();
            form.ShowDialog();
        }
    }
}
