﻿namespace IcoMerge
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            buttonConvertToIco = new Button();
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            textBox1 = new TextBox();
            saveFileDialog1 = new SaveFileDialog();
            SuspendLayout();
            // 
            // buttonConvertToIco
            // 
            buttonConvertToIco.Location = new Point(12, 155);
            buttonConvertToIco.Name = "buttonConvertToIco";
            buttonConvertToIco.Size = new Size(112, 47);
            buttonConvertToIco.TabIndex = 1;
            buttonConvertToIco.Text = "Save to ICO file";
            buttonConvertToIco.UseVisualStyleBackColor = true;
            buttonConvertToIco.Click += buttonConvertToIco_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "ico";
            openFileDialog1.Filter = "Images|*.ico;*.png|Icon|*.ico|PNG|*.png|All files|*.*";
            openFileDialog1.Multiselect = true;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(112, 23);
            button1.TabIndex = 2;
            button1.Text = "Select PNG files";
            button1.UseVisualStyleBackColor = true;
            button1.Click += LoadFiles_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 41);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(230, 108);
            textBox1.TabIndex = 3;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "ico";
            saveFileDialog1.Filter = "Icon|*ico|All files|*.*";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(254, 211);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(buttonConvertToIco);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonConvertToIco;
        private OpenFileDialog openFileDialog1;
        private Button button1;
        private TextBox textBox1;
        private SaveFileDialog saveFileDialog1;
    }
}
