using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractPrinteryWpf
{
    /// <summary>
    /// Логика взаимодействия для EditionWindow.xaml
    /// </summary>
    public partial class EditionWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<EditionMaterialViewModel> editionMaterials;

        public EditionWindow()
        {
            InitializeComponent();
            Loaded += EditionWindow_Load;
        }

        private void EditionWindow_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var edition = Task.Run(() => APIClient.GetRequestData<EditionViewModel>("api/Edition/Get/" + id.Value)).Result;
                    textBoxName.Text = edition.EditionName;
                    textBoxCoast.Text = edition.Coast.ToString();
                    editionMaterials = edition.EditionMaterials;
                    LoadData();
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
            var form = new EditionMaterialWindow();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.EditionNumber = id.Value;
                    editionMaterials.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterial.SelectedItem != null)
            {
                var form = new EditionMaterialWindow();
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

            List<EditionMaterialBindingModel> materialEditionBM = new List<EditionMaterialBindingModel>();
            for (int i = 0; i < editionMaterials.Count; ++i)
            {
                materialEditionBM.Add(new EditionMaterialBindingModel
                {
                    Number = editionMaterials[i].Number,
                    EditionNumber = editionMaterials[i].EditionNumber,
                    MaterialNumber = editionMaterials[i].MaterialNumber,
                    Count = editionMaterials[i].Count
                });
            }
            string name = textBoxName.Text;
            int coast = Convert.ToInt32(textBoxCoast.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Edition/UpdElement", new EditionBindingModel
                {
                    Number = id.Value,
                    EditionName = name,
                    Coast = coast,
                    EditionMaterials = materialEditionBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Edition/AddElement", new EditionBindingModel
                {
                    EditionName = name,
                    Coast = coast,
                    EditionMaterials = materialEditionBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}