namespace IcoMerge
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
            buttonMergeToIco = new Button();
            buttonConvertToIco = new Button();
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            textBox1 = new TextBox();
            saveFileDialog1 = new SaveFileDialog();
            SuspendLayout();
            // 
            // buttonMergeToIco
            // 
            buttonMergeToIco.Location = new Point(12, 155);
            buttonMergeToIco.Name = "buttonMergeToIco";
            buttonMergeToIco.Size = new Size(100, 47);
            buttonMergeToIco.TabIndex = 0;
            buttonMergeToIco.Text = "Save merged ICO";
            buttonMergeToIco.UseVisualStyleBackColor = true;
            buttonMergeToIco.Click += buttonMerge_Click;
            // 
            // buttonConvertToIco
            // 
            buttonConvertToIco.Location = new Point(303, 155);
            buttonConvertToIco.Name = "buttonConvertToIco";
            buttonConvertToIco.Size = new Size(89, 47);
            buttonConvertToIco.TabIndex = 1;
            buttonConvertToIco.Text = "Convert PNGs to ICOs";
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
            button1.Size = new Size(89, 23);
            button1.TabIndex = 2;
            button1.Text = "Open Files";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 41);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(380, 108);
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
            ClientSize = new Size(415, 211);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(buttonConvertToIco);
            Controls.Add(buttonMergeToIco);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonMergeToIco;
        private Button buttonConvertToIco;
        private OpenFileDialog openFileDialog1;
        private Button button1;
        private TextBox textBox1;
        private SaveFileDialog saveFileDialog1;
    }
}
