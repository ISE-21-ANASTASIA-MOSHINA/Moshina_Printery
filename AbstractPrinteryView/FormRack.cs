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
    public partial class FormRack : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Number { set { id = value; } }

        private readonly IRackSVC service;

        private int? id;

        public FormRack(IRackSVC service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormRack_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    RackViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.RackName;
                        dataGridView.DataSource = view.RackMaterial;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new RackBindingModel
                    {
                        Number = id.Value,
                        RackName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new RackBindingModel
                    {
                        RackName = textBoxName.Text
                    });
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
    }
}
