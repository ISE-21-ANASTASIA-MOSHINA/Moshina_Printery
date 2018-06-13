using System;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Windows;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для CustomerBookingsWindow.xaml
    /// </summary>
    public partial class CustomerBookingsWindow : Window
    {
        public CustomerBookingsWindow()
        {
            InitializeComponent();
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "AbstractPrinteryWpf.ReportCustomersBookings.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + Convert.ToDateTime(dateTimePickerFrom.SelectedDate).ToString("dd-MM") +
                                            " по " + Convert.ToDateTime(dateTimePickerTo.SelectedDate).ToString("dd-MM"));
                reportViewer.LocalReport.SetParameters(parameter);

                var dataSource = Task.Run(() => APIClient.PostRequestData<ReportBindingModel, List<CustomerBookingsModel>>("api/Report/GetCustomerBookings",
                     new ReportBindingModel
                     {
                         DateFrom = dateTimePickerFrom.SelectedDate,
                         DateTo = dateTimePickerTo.SelectedDate
                     })).Result;

                ReportDataSource source = new ReportDataSource("DataSetBookings", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveCustomerBookings", new ReportBindingModel
                {
                    FileName = sfd.FileName,
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                }));
                task.ContinueWith((prevTask) => MessageBox.Show("Список заказов сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
            }
        }
    }
}