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
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using PrinterySVC.BindingModel;

namespace AbstractPrinteryView
{
    public partial class FormPutOnRack : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IRackSVC serviceS;

        private readonly IMaterialSVC serviceC;

        private readonly IMainSVC serviceM;

        public FormPutOnRack(IRackSVC serviceS, IMaterialSVC serviceC, IMainSVC serviceM)
        {
            InitializeComponent();
            this.serviceS = serviceS;
            this.serviceC = serviceC;
            this.serviceM = serviceM;
        }

        private void FormPutOnRack_Load(object sender, EventArgs e)
        {

            try
            {
                List<MaterialViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxMaterial.DisplayMember = "MaterialName";
                    comboBoxMaterial.ValueMember = "Number";
                    comboBoxMaterial.DataSource = listC;
                    comboBoxMaterial.SelectedItem = null;
                }
                List<RackViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxRack.DisplayMember = "RackName";
                    comboBoxRack.ValueMember = "Number";
                    comboBoxRack.DataSource = listS;
                    comboBoxRack.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (comboBoxRack.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceM.PutMaterialOnRack(new RackMaterialBindingModel
                {
                    MaterialNumber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
                    RackNumber = Convert.ToInt32(comboBoxRack.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
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

        private void comboBoxRack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
