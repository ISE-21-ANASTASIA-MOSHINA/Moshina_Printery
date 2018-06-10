using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using PrinterySVC.ViewModel;
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

namespace AbstractPrinteryView
{
    public partial class FormTakeOrderInWork : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Number { set { id = value; } }

        private readonly ITypographerSVC serviceT;

        private readonly IMainSVC serviceM;

        private int? id;

        public FormTakeOrderInWork(ITypographerSVC serviceT, IMainSVC serviceM)
        {
            InitializeComponent();
            this.serviceT = serviceT;
            this.serviceM = serviceM;
        }

        private void FormTakeOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                List<TypographerViewModel> listI = serviceT.GetList();
                if (listI != null)
                {
                    comboBoxTypographer.DisplayMember = "TypographerFIO";
                    comboBoxTypographer.ValueMember = "Number";
                    comboBoxTypographer.DataSource = listI;
                    comboBoxTypographer.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxTypographer.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceM.TakeBookingInWork(new BookingBindingModel
                {
                    Number = id.Value,
                    TypographerNumber = Convert.ToInt32(comboBoxTypographer.SelectedValue)
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
    }
}
