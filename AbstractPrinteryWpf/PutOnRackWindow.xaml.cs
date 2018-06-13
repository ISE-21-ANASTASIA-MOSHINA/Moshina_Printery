using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для PutOnRackWindow.xaml
    /// </summary>
    public partial class PutOnRackWindow : Window
    {
        public PutOnRackWindow()
        {
            InitializeComponent();
            Loaded += PutOnRackWindow_Load;
        }

        private void PutOnRackWindow_Load(object sender, EventArgs e)
        {
            try
            {
                List<MaterialViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<MaterialViewModel>>("api/Material/GetList")).Result;
                if (listC != null)
                {
                    comboBoxMaterial.DisplayMemberPath = "MaterialName";
                    comboBoxMaterial.SelectedValuePath = "Number";
                    comboBoxMaterial.ItemsSource = listC;
                    comboBoxMaterial.SelectedItem = null;
                }
                List<RackViewModel> listS = Task.Run(() => APIClient.GetRequestData<List<RackViewModel>>("api/Rack/GetList")).Result;
                if (listS != null)
                {
                    comboBoxRack.DisplayMemberPath = "RackName";
                    comboBoxRack.SelectedValuePath = "Number";
                    comboBoxRack.ItemsSource = listS;
                    comboBoxRack.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxMaterial.SelectedItem == null)
            {
                MessageBox.Show("Выберите материал", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxRack.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int materialNumber = Convert.ToInt32(comboBoxMaterial.SelectedValue);
                int rackNumber = Convert.ToInt32(comboBoxRack.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutMaterialOnRack", new RackMaterialBindingModel
                {
                    MaterialNumber = materialNumber,
                    RackNumber = rackNumber,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Склад пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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