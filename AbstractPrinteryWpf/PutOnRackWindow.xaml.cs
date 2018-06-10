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
    /// Логика взаимодействия для PutOnRackWindow.xaml
    /// </summary>
    public partial class PutOnRackWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IRackSVC serviceRack;

        private readonly IMaterialSVC serviceMaterial;

        private readonly IMainSVC serviceMain;

        public PutOnRackWindow(IRackSVC serviceS, IMaterialSVC serviceC, IMainSVC serviceM)
        {
            InitializeComponent();
            Loaded += PutOnRackWindow_Load;
            this.serviceRack = serviceS;
            this.serviceMaterial = serviceC;
            this.serviceMain = serviceM;
        }

        private void PutOnRackWindow_Load(object sender, EventArgs e)
        {
            try
            {
                List<MaterialViewModel> listMaterial = serviceMaterial.GetList();
                if (listMaterial != null)
                {
                    comboBoxMaterial.DisplayMemberPath = "MaterialName";
                    comboBoxMaterial.SelectedValuePath = "Id";
                    comboBoxMaterial.ItemsSource = listMaterial;
                    comboBoxMaterial.SelectedItem = null;
                }
                List<RackViewModel> listRack = serviceRack.GetList();
                if (listRack != null)
                {
                    comboBoxRack.DisplayMemberPath = "RackName";
                    comboBoxRack.SelectedValuePath = "Id";
                    comboBoxRack.ItemsSource = listRack;
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
                MessageBox.Show("Выберите ингредиент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxRack.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.PutMaterialOnRack(new RackMaterialBindingModel
                {
                    MaterialNamber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
                    RackNamber = Convert.ToInt32(comboBoxRack.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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

