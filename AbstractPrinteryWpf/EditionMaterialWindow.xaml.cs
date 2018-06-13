using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для EditionMaterialWindow.xaml
    /// </summary>
    public partial class EditionMaterialWindow : Window
    {
        public EditionMaterialViewModel Model { set { model = value; } get { return model; } }

        private EditionMaterialViewModel model;

        public EditionMaterialWindow()
        {
            InitializeComponent();
            Loaded += EditionMaterialWindow_Load;
        }

        private void EditionMaterialWindow_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Material/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxMaterial.DisplayMemberPath = "MaterialName";
                    comboBoxMaterial.SelectedValuePath = "Number";
                    comboBoxMaterial.ItemsSource = APIClient.GetElement<List<MaterialViewModel>>(response);
                    comboBoxMaterial.SelectedItem = null;
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

            if (model != null)
            {
                comboBoxMaterial.IsEnabled = false;
                comboBoxMaterial.SelectedValue = model.MaterialNumber;
                textBoxCount.Text = model.Count.ToString();
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
            try
            {
                if (model == null)
                {
                    model = new EditionMaterialViewModel
                    {
                        MaterialNumber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
                        MaterialName = comboBoxMaterial.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
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

