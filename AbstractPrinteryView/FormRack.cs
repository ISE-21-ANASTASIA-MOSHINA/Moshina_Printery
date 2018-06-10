using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrinterySVC.ViewModel;
using PrinterySVC.BindingModel;
using System.Net.Http;

namespace AbstractPrinteryView
{
    public partial class FormRack : Form
    {

        public int Number { set { id = value; } }

        private int? id;

        public FormRack()
        {
            InitializeComponent();
        }

        private void FormRack_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Rack/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var stock = APIClient.GetElement<RackViewModel>(response);
                        textBoxName.Text = stock.RackName;
                        dataGridView.DataSource = stock.RackMaterial;
                        dataGridView.Columns[0].Visible = false;
                        dataGridView.Columns[1].Visible = false;
                        dataGridView.Columns[2].Visible = false;
                        dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Rack/UpdElement", new RackBindingModel
                    {
                        Number = id.Value,
                        RackName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Rack/AddElement", new RackBindingModel
                    {
                        RackName = textBoxName.Text
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
