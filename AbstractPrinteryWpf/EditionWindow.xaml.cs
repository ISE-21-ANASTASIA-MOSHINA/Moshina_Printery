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
    /// Логика взаимодействия для EditionWindow.xaml
    /// </summary>
    public partial class EditionWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IEditionSVC service;

        private int? id;

        private List<EditionMaterialViewModel> editionMaterials;

        public EditionWindow(IEditionSVC service)
        {
            InitializeComponent();
            Loaded += EditionWindow_Load;
            this.service = service;
        }

        private void EditionWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    EditionViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.EditionName;
                        textBoxCoast.Text = view.Cost.ToString();
                        editionMaterials = view.EditionMaterials;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                editionMaterials = new List<EditionMaterialViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (editionMaterials != null)
                {
                    dataGridViewMaterial.ItemsSource = null;
                    dataGridViewMaterial.ItemsSource = editionMaterials;
                    dataGridViewMaterial.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMaterial.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMaterial.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewMaterial.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<EditionMaterialWindow>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.EditionNamber = id.Value;
                    editionMaterials.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterial.SelectedItem != null)
            {
                var form = Container.Resolve<EditionMaterialWindow>();
                form.Model = editionMaterials[dataGridViewMaterial.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    editionMaterials[dataGridViewMaterial.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterial.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        editionMaterials.RemoveAt(dataGridViewMaterial.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxCoast.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (editionMaterials == null || editionMaterials.Count == 0)
            {
                MessageBox.Show("Заполните ингредиенты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<EditionMaterialBindingModel> editionMaterialBM = new List<EditionMaterialBindingModel>();
                for (int i = 0; i < editionMaterials.Count; ++i)
                {
                    editionMaterialBM.Add(new EditionMaterialBindingModel
                    {
                        Number = editionMaterials[i].Number,
                        EditionNamber = editionMaterials[i].EditionNamber,
                        MaterialNamber = editionMaterials[i].MaterialNamber,
                        Count = editionMaterials[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpElement(new EdiitionViewModel
                    {
                        Number = id.Value,
                        EditionName = textBoxName.Text,
                        Coast = Convert.ToInt32(textBoxCoast.Text),
                        EditionMaterials = editionMaterialBM
                    });
                }
                else
                {
                    service.AddElement(new EdiitionViewModel
                    {
                        EditionName = textBoxName.Text,
                        Coast = Convert.ToInt32(textBoxCoast.Text),
                        EditionMaterials = editionMaterialBM
                    });
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

