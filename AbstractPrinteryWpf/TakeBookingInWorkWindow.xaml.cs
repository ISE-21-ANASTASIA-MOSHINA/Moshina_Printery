using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для TakeBookingInWorkWindow.xaml
    /// </summary>
    public partial class TakeBookingInWorkWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly ITypographerSVC serviceTypographer;

        private readonly IMainSVC serviceMain;

        private int? id;

        public TakeBookingInWorkWindow(ITypographerSVC serviceI, IMainSVC serviceM)
        {
            InitializeComponent();
            Loaded += TakeBookingInWorkWindow_Load;
            this.serviceTypographer = serviceI;
            this.serviceMain = serviceM;
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
                List<TypographerViewModel> listTypographer = serviceTypographer.GetList();
                if (listTypographer != null)
                {
                    comboBoxTypographer.DisplayMemberPath = "TypographerFIO";
                    comboBoxTypographer.SelectedValuePath = "Id";
                    comboBoxTypographer.ItemsSource = listTypographer;
                    comboBoxTypographer.SelectedItem = null;

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
                MessageBox.Show("Выберите повара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.TakeBookingInWork(new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = ((TypographerViewModel)comboBoxTypographer.SelectedItem).Number,
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


