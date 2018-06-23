﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrinterySVC.ViewModel;

namespace AbstractPrinteryView
{
    public partial class FormEditionMaterial : Form
    {

        public EditionMaterialViewModel Model { set { model = value; } get { return model; } }

        private EditionMaterialViewModel model;

        public FormEditionMaterial()
        {
            InitializeComponent();
        }

        private void FormEditionMaterial_Load(object sender, EventArgs e)
        {
            try
            {
                comboBoxMaterial.DisplayMember = "MaterialName";
                comboBoxMaterial.ValueMember = "Number";
                comboBoxMaterial.DataSource = Task.Run(() => APIClient.GetRequestData<List<MaterialViewModel>>("api/Material/GetList")).Result;
                comboBoxMaterial.SelectedItem = null;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxMaterial.Enabled = false;
                comboBoxMaterial.SelectedValue = model.MaterialNamber;
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
                        MaterialNamber = Convert.ToInt32(comboBoxMaterial.SelectedValue),
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
