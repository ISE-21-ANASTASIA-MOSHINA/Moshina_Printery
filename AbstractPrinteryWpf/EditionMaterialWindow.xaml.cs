using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                comboBoxMaterial.DisplayMemberPath = "MaterialName";
                comboBoxMaterial.SelectedValuePath = "Number";
                comboBoxMaterial.ItemsSource = Task.Run(() => APIClient.GetRequestData<List<MaterialViewModel>>("api/Material/GetList")).Result;
                comboBoxMaterial.SelectedItem = null;

            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                MessageBox.Show("Выберите ингредиент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

