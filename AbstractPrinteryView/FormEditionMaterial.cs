using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;
using PrinterySVC.ViewModel;
using PrinterySVC.Inteface;

namespace AbstractPrinteryView
{
    public partial class FormEditionMaterial : Form
    {

        [Dependency]
        public new IUnityContainer Container { get; set; }

        public EditionMaterialViewModel Model { set { model = value; } get { return model; } }

        private readonly IMaterialSVC service;

        private EditionMaterialViewModel model;

        public FormEditionMaterial(IMaterialSVC service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormEditionMaterial_Load(object sender, EventArgs e)
        {
            try
            {
                List<MaterialViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBoxMaterial.DisplayMember = "MaterialName";
                    comboBoxMaterial.ValueMember = "Number";
                    comboBoxMaterial.DataSource = list;
                    comboBoxMaterial.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxMaterial.Enabled = false;
                comboBoxMaterial.SelectedValue = model.MaterialNumber;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMaterial.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
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

        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
