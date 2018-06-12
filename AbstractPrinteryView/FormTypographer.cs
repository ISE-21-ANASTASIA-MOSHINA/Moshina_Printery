using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractPrinteryView
{
    public partial class FormTypographer : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ITypographerSVC service;

        private int? id;

        public FormTypographer(ITypographerSVC service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormTypographer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    TypographerViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxFIO.Text = view.TypographerFIO;
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
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new TypographerBildingModel
                    {
                        Number = id.Value,
                        TypographerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    service.AddElement(new TypographerBildingModel
                    {
                        TypographerFIO = textBoxFIO.Text
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
