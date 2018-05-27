using Microsoft.Reporting.WinForms;
using PrinterySVC.BindingModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractPrinteryView
{
    public partial class FormCustomertOrders : Form
    {

        public FormCustomertOrders()
        {
            InitializeComponent();
        }


        private void FormCustomertOrders_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveCustomerOrders", new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.Value,
                        DateTo = dateTimePickerTo.Value
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        }


            private void buttonMake_Click_1(object sender, EventArgs e)
            {
                if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
                {
                    MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                                "c " + dateTimePickerFrom.Value.ToShortDateString() +
                                                " по " + dateTimePickerTo.Value.ToShortDateString());
                    reportViewer.LocalReport.SetParameters(parameter);

                var response = APIClient.PostRequest("api/Report/GetCustomerOrders", new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    var dataSource = APIClient.GetElement<List<CustomerBindingModel>>(response);
                    ReportDataSource source = new ReportDataSource("DataSet", dataSource);
                    reportViewer.LocalReport.DataSources.Add(source);
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }

                reportViewer.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
    } 
}
