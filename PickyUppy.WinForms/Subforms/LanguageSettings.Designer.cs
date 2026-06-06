namespace PickyUppy.WinForms.Subforms
{
    partial class LanguageSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageSettings));
            CB_Language = new ComboBox();
            B_OK = new Button();
            SuspendLayout();
            // 
            // CB_Language
            // 
            CB_Language.FormattingEnabled = true;
            CB_Language.Location = new Point(12, 3);
            CB_Language.Name = "CB_Language";
            CB_Language.Size = new Size(158, 23);
            CB_Language.TabIndex = 0;
            CB_Language.SelectedIndexChanged += CB_Language_SelectedIndexChanged;
            // 
            // B_OK
            // 
            B_OK.DialogResult = DialogResult.OK;
            B_OK.Location = new Point(11, 28);
            B_OK.Name = "B_OK";
            B_OK.Size = new Size(160, 25);
            B_OK.TabIndex = 1;
            B_OK.Text = "OK";
            B_OK.UseVisualStyleBackColor = true;
            // 
            // LanguageSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(181, 57);
            Controls.Add(B_OK);
            Controls.Add(CB_Language);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LanguageSettings";
            Text = "LanguageSettings";
            FormClosing += LanguageSettings_FormClosing;
            Load += LanugageSettings_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox CB_Language;
        private Button B_OK;
    }
}
