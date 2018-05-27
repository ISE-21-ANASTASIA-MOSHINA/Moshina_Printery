using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractPrinteryView
{
    public partial class FormMaterial : Form
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormMaterial()
        {
            InitializeComponent();
        }

        private void FormComponent_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Material/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var component = APIClient.GetElement<MaterialViewModel>(response);
                        textBoxName.Text = component.MaterialName;
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

       

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
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
                        response = APIClient.PostRequest("api/Material/UpdElement", new MaterialBindingModel
                        {
                            Number = id.Value,
                            MaterialName = textBoxName.Text
                        });
                    }
                    else
                    {
                        response = APIClient.PostRequest("api/Material/AddElement", new MaterialBindingModel
                        {
                            MaterialName = textBoxName.Text
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
}
