using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для EditionMaterialWindow.xaml
    /// </summary>
    public partial class EditionMaterialWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public EditionMaterialViewModel Model { set { model = value; } get { return model; } }

        private readonly IMaterialSVC service;

        private EditionMaterialViewModel model;

        public EditionMaterialWindow(IMaterialSVC service)
        {
            InitializeComponent();
            Loaded += EditionMaterialWindow_Load;
            this.service = service;
        }

        private void EditionMaterialWindow_Load(object sender, EventArgs e)
        {
            List<MaterialViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxMaterial.DisplayMemberPath = "MaterialName";
                    comboBoxMaterial.SelectedValuePath = "Id";
                    comboBoxMaterial.ItemsSource = list;
                    comboBoxMaterial.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxMaterial.IsEnabled = false;
                foreach (MaterialViewModel item in list)
                {
                    if (item.MaterialName == model.MaterialName)
                    {
                        comboBoxMaterial.SelectedItem = item;
                    }
                }
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
                        MaterialNamber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
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

