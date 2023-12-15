using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using IcollatorForever;
using MiscUtil.Conversion;
using MiscUtil.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
//using Image = SixLabors.ImageSharp.Image;

namespace IcoMerge;

public partial class Form1 : Form
{

    string[] icoFiles = { @"img\icon16.ico", @"img\icon32.ico", @"img\icon64.ico", @"img\icon256.ico" };
    string[] pngFiles = { @"img\icon16.png", @"img\icon32.png", @"img\icon64.png", @"img\icon256.png" };
    string[] imageFiles = new string[0];

    #region variables
    List<IIconEntry> entries = new();


    #endregion
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
                string outFile = file + "_convert.ico";
                img.Save(outFile, System.Drawing.Imaging.ImageFormat.Icon);
                Debug.WriteLine(outFile);
            }
        }
    }

    private void LoadImages(string[] files)
    {
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
        if (entries.Count > 0)
        {
            using (MemoryStream stream = new MemoryStream())
            {

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    IconUtils.WriteToStream(entries, stream);
                    stream.WriteTo(fs);
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

        string extension = System.IO.Path.GetExtension(filename).ToLower();

        bool success = false;

        if (extension == ".ico")
        {
            success = TryAddIcoFile(filename, fileBytes);
            Debug.WriteLine("Add ico, success: " + success);
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
            LoadImages(icoFiles);
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
