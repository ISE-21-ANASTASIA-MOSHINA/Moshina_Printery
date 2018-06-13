using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                    var response = APIClient.GetRequest("api/Edition/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var mebel = APIClient.GetElement<EditionViewModel>(response);
                        textBoxName.Text = mebel.EditionName;
                        textBoxCoast.Text = mebel.Coast.ToString();
                        editionMaterials = mebel.EditionMaterials;
                        LoadData();
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
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        EditionNumber = editionMaterials[i].EditionNumber,
                        MaterialNumber = editionMaterials[i].MaterialNumber,
                        Count = editionMaterials[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Edition/UpdElement", new EditionBindingModel
                    {
                        Number = id.Value,
                        EditionName = textBoxName.Text,
                        Coast = Convert.ToInt32(textBoxCoast.Text),
                        EditionMaterials = editionMaterialBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Edition/AddElement", new EditionBindingModel
                    {
                        EditionName = textBoxName.Text,
                        Coast = Convert.ToInt32(textBoxCoast.Text),
                        EditionMaterials = editionMaterialBM
                    });
                }
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