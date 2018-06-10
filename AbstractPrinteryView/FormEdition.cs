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
                    var edition = Task.Run(() => APIClient.GetRequestData<EditionViewModel>("api/Edition/Get/" + id.Value)).Result;
                    textBoxName.Text = edition.EditionName;
                    textBoxPrice.Text = edition.Cost.ToString();
                    editionMaterials = edition.EditionMaterials;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Edition/UpdElement", new EditionBindingModel
                {
                    Number = id.Value,
                    EditionName = name,
                    Price = price,
                    EditionMaterials = editionMaterialBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Edition/AddElement", new EditionBindingModel
                {
                    EditionName = name,
                    Price = price,
                    EditionMaterials = editionMaterialBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
