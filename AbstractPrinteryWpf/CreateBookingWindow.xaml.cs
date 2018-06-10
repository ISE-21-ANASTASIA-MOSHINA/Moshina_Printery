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
    /// Логика взаимодействия для CreateBookingWindow.xaml
    /// </summary>
    public partial class CreateBookingWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ICustomerSVC serviceCustomer;

        private readonly IEditionSVC serviceEdition;

        private readonly IMainSVC serviceMain;


        public CreateBookingWindow(ICustomerSVC serviceV, IEditionSVC serviceS, IMainSVC serviceB)
        {
            InitializeComponent();
            Loaded += CreateBookingWindow_Load;
            comboBoxEdition.SelectionChanged += comboBoxEdition_SelectedIndexChanged;
            comboBoxEdition.SelectionChanged += new SelectionChangedEventHandler(comboBoxEdition_SelectedIndexChanged);
            this.serviceCustomer = serviceV;
            this.serviceEdition = serviceS;
            this.serviceMain = serviceB;
        }

        private void CreateBookingWindow_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerVievModel> listCustomer = serviceCustomer.GetList();
                if (listCustomer != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                    comboBoxCustomer.SelectedValuePath = "Id";
                    comboBoxCustomer.ItemsSource = listCustomer;
                    comboBoxEdition.SelectedItem = null;
                }
                List<EditionViewModel> listEdition = serviceEdition.GetList();
                if (listEdition != null)
                {
                    comboBoxEdition.DisplayMemberPath = "EditionName";
                    comboBoxEdition.SelectedValuePath = "Id";
                    comboBoxEdition.ItemsSource = listEdition;
                    comboBoxEdition.SelectedItem = null;
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
                    EditionViewModel edition = serviceEdition.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * edition.Cost).ToString();
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
                MessageBox.Show("Выберите посетителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxEdition.SelectedItem == null)
            {
                MessageBox.Show("Выберите суши", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.CreateBooking(new BookingBindingModel
                {
                    CustomerNumber = ((CustomerVievModel)comboBoxCustomer.SelectedItem).Number,
                    EditionNumber = ((EditionViewModel)comboBoxEdition.SelectedItem).Number,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
