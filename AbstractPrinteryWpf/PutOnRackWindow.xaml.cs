using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
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
                var responseC = APIClient.GetRequest("api/Material/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<MaterialViewModel> list = APIClient.GetElement<List<MaterialViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxMaterial.DisplayMemberPath = "MaterialName";
                        comboBoxMaterial.SelectedValuePath = "Number";
                        comboBoxMaterial.ItemsSource = list;
                        comboBoxMaterial.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Rack/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<RackViewModel> list = APIClient.GetElement<List<RackViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxRack.DisplayMemberPath = "RackName";
                        comboBoxRack.SelectedValuePath = "Number";
                        comboBoxRack.ItemsSource = list;
                        comboBoxRack.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
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
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxRack.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutMaterialOnRack", new RackMaterialBindingModel
                {
                    MaterialNumber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
                    RackNumber = Convert.ToInt32(comboBoxRack.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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