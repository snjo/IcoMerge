using IcollatorForever;
using System.Diagnostics;
//using Image = SixLabors.ImageSharp.Image;

namespace IcoMerge;

public partial class Form1 : Form
{

    string[] icoFiles = new string[0];
    string[] pngFiles = new string[0];
    string[] imageFiles = new string[0];

    List<IIconEntry> entries = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void ConvertPngToIco(string[] pngFiles)
    {
        foreach (string file in pngFiles)
        {
            if (File.Exists(file))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(file);
                int height = img.Height;
                int width = img.Width;
                img.Dispose();
                string outFile = file + "_c.ico";
                PngIconConverter.Convert(file, outFile, height);
            }
        }
    }


    private void LoadIcoImages(string[] files)
    {
        entries.Clear();
        foreach (string file in files)
        {
            if (File.Exists(file))
            {
                Debug.WriteLine($"Loading file {file}");

                AddFile(file, File.ReadAllBytes(file));
            }
            else
            {
                Debug.WriteLine($"Can't find file {file}");
            }
        }
    }

    private void SaveIcon(string filePath)
    {
        if (entries.Count > 0)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    IconUtils.WriteToStream(entries, stream);
                    stream.WriteTo(fs);
                    Debug.WriteLine("Saving icon to file: " +  filePath);

                }
            }
        }
    }

    private bool TryAddIcoFile(string filename, byte[] bytes)
    {
        try
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            { 
                IcollatorForever.Icon icon = new IcollatorForever.Icon(filename, stream);
                entries.AddRange(icon.Entries); 
                entries = entries.OrderBy(e => e.Description).ToList();
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);

        }
        return false;
    }

    public bool AddFile(string filename, byte[] fileBytes)
    {
        string extension = System.IO.Path.GetExtension(filename).ToLower();
        bool success = false;

        if (extension == ".ico")
        {
            success = TryAddIcoFile(filename, fileBytes);            
        }
        else
        {
            Debug.WriteLine("File is in wrong format (not .ico): " + filename);
        }

        if (success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void buttonMerge_Click(object sender, EventArgs e)
    {
        DialogResult result = saveFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            icoFiles = imageFiles;
            LoadIcoImages(icoFiles);
            SaveIcon(saveFileDialog1.FileName);
        }
    }

    private void buttonConvertToIco_Click(object sender, EventArgs e)
    {
        pngFiles = imageFiles;
        ConvertPngToIco(pngFiles);
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        DialogResult result = openFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            imageFiles = openFileDialog1.FileNames;
            Debug.WriteLine("Files selected: " + imageFiles.Length);
        }
        textBox1.Text = "";
        foreach (string filename in imageFiles)
        {
            textBox1.Text += filename + Environment.NewLine;
        }
    }
}
