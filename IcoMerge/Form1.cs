using System.Diagnostics;

namespace IcoMerge;

public partial class Form1 : Form
{

    string[] icoFiles = new string[0];
    string[] pngFiles = new string[0];
    string[] imageFiles = new string[0];


    public Form1()
    {
        InitializeComponent();
    }

    private void ConvertPngToIco(string[] pngFiles, string targetFile)
    {
        PngToIcon.Convert(new List<string>(pngFiles), targetFile);
    }

    private void buttonConvertToIco_Click(object sender, EventArgs e)
    {
        DialogResult result = saveFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            ConvertPngToIco(imageFiles, saveFileDialog1.FileName);
        }
    }

    private void LoadFiles_Click(object sender, EventArgs e)
    {
        DialogResult result = openFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            imageFiles = openFileDialog1.FileNames;
            //Debug.WriteLine("Files selected: " + imageFiles.Length);
        }
        textBox1.Text = "";
        foreach (string filename in imageFiles)
        {
            textBox1.Text += Path.GetFileName(filename) + Environment.NewLine;
        }
    }

    private void buttonUnpackICO_Click(object sender, EventArgs e)
    {
        
        DialogResult result = folderBrowserDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            foreach (string file in imageFiles)
            {
                string filePrefix = Path.GetFileNameWithoutExtension(file);
                IconToBitmaps.LoadFiles(file, folderBrowserDialog1.SelectedPath, filePrefix, "png", radioButton2.Checked);//saveFileDialog1.FileName);
            }
        }

    }
}
