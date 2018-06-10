using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AbstractPrinteryView
{
    public partial class FormEdition : Form
    {

        public int Number { set { id = value; } }

        private int? id;

        private List<EditionMaterialViewModel> editionMaterials;

        public FormEdition()
        {
            InitializeComponent();
        }

        private void FormEdition_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Edition/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var edition = APIClient.GetElement<EditionViewModel>(response);
                        textBoxName.Text = edition.EditionName;
                        textBoxPrice.Text = edition.Cost.ToString();
                        editionMaterials = edition.EditionMaterials;
                        LoadData();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                editionMaterials = new List<EditionMaterialViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (editionMaterials != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = editionMaterials;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormEditionMaterial();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.EditionNamber = id.Value;
                    }
                    editionMaterials.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormEditionMaterial();
                form.Model = editionMaterials[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    editionMaterials[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        editionMaterials.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (editionMaterials == null || editionMaterials.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Edition/UpdElement", new EditionBindingModel
                    {
                        Number= id.Value,
                        EditionName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        EditionMaterials = editionMaterialBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Edition/AddElement", new EditionBindingModel
                    {
                        EditionName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        EditionMaterials = editionMaterialBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
