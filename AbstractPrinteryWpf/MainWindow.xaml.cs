using Microsoft.Win32;
using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainSVC service;

        private readonly IReportSVC reportService;

        public MainWindow(IMainSVC service, IReportSVC reportService)
        {
            InitializeComponent();
            this.service = service;
            this.reportService = reportService;
        }

        private void LoadData()
        {
            try
            {
                List<BookingViewModel> list = service.GetList();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<CustomersWindow>();
            form.ShowDialog();
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<MaterialsWindow>();
            form.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<EditionsWindow>();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<RacksWindow>();
            form.ShowDialog();
        }

        private void работникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<TypographersWindow>();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<PutOnRackWindow>();
            form.ShowDialog();
        }

        private void buttonCreateBooking_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<CreateBookingWindow>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeBookingInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = Container.Resolve<TakeBookingInWorkWindow>();
                form.ID = ((BookingViewModel)dataGridViewMain.SelectedItem).Number;
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
                    service.FinishBooking(id);
                    LoadData();
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
                    service.PayBooking(id);
                    LoadData();
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
                    reportService.SaveProductPrice(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    reportService.SaveRacksLoad(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<CustomerBookingsWindow>();
            form.ShowDialog();
        }
    }
}