namespace AbstractPrinteryView
{
    partial class FormTakeOrderInWork
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxTypographer = new System.Windows.Forms.ComboBox();
            this.labelTypographer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(298, 83);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(190, 83);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 28);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxTypographer
            // 
            this.comboBoxTypographer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypographer.FormattingEnabled = true;
            this.comboBoxTypographer.Location = new System.Drawing.Point(154, 38);
            this.comboBoxTypographer.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxTypographer.Name = "comboBoxTypographer";
            this.comboBoxTypographer.Size = new System.Drawing.Size(288, 24);
            this.comboBoxTypographer.TabIndex = 11;
            this.comboBoxTypographer.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypographer_SelectedIndexChanged);
            // 
            // labelTypographer
            // 
            this.labelTypographer.AutoSize = true;
            this.labelTypographer.Location = new System.Drawing.Point(43, 42);
            this.labelTypographer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTypographer.Name = "labelTypographer";
            this.labelTypographer.Size = new System.Drawing.Size(99, 17);
            this.labelTypographer.TabIndex = 10;
            this.labelTypographer.Text = "Исполнитель:";
            // 
            // FormTakeOrderInWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 141);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxTypographer);
            this.Controls.Add(this.labelTypographer);
            this.Name = "FormTakeOrderInWork";
            this.Text = "Отправить  в производсво";
            this.Load += new System.EventHandler(this.FormTakeOrderInWork_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxTypographer;
        private System.Windows.Forms.Label labelTypographer;
    }
}