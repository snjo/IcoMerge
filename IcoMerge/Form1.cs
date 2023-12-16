using IcollatorForever;
using System.Diagnostics;
//using Image = SixLabors.ImageSharp.Image;

namespace IcoMerge;

public partial class Form1 : Form
{

    string[] icoFiles = { @"img\icon16.ico", @"img\icon32.ico", @"img\icon64.ico", @"img\icon256.ico" };
    string[] pngFiles = { @"img\icon16.png", @"img\icon32.png", @"img\icon64.png", @"img\icon256.png" };
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

        Debug.WriteLine("Entries: " + entries.Count);
    }

    private void SaveIcon(string filePath)
    {
        Debug.WriteLine("SaveIcon 1");
        if (entries.Count > 0)
        {
            Debug.WriteLine("SaveIcon 2");
            using (MemoryStream stream = new MemoryStream())
            {
                Debug.WriteLine("SaveIcon 3");
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    Debug.WriteLine("SaveIcon 4");
                    IconUtils.WriteToStream(entries, stream);
                    Debug.WriteLine("SaveIcon 5");
                    stream.WriteTo(fs);
                    Debug.WriteLine("SaveIcon 6");
                }
            }
        }
    }

    private bool TryAddIcoFile(string filename, byte[] bytes)
    {
        Debug.WriteLine("TryAddIcoFile 1");
        try
        {
            Debug.WriteLine("TryAddIcoFile 2");
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Debug.WriteLine("TryAddIcoFile 3");
                IcollatorForever.Icon icon = new IcollatorForever.Icon(filename, stream);
                Debug.WriteLine("TryAddIcoFile 4, ");
                entries.AddRange(icon.Entries);
                Debug.WriteLine("TryAddIcoFile 5");
                entries = entries.OrderBy(e => e.Description).ToList();
                Debug.WriteLine("TryAddIcoFile 6");
            }
            Debug.WriteLine("Added ico file");
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
        Debug.WriteLine("AddFile 1");
        string extension = System.IO.Path.GetExtension(filename).ToLower();

        bool success = false;

        if (extension == ".ico")
        {
            Debug.WriteLine("AddFile 2");
            success = TryAddIcoFile(filename, fileBytes);
            Debug.WriteLine("AddFile 3");
            Debug.WriteLine("Add ico, success: " + success);
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
            Debug.WriteLine("Entries: " + entries.Count);
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
