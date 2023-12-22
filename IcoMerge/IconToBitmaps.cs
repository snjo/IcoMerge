using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;


namespace IcoMerge;

// Rewrite of code from: https://github.com/Willy-Kimura
//https://gist.github.com/Willy-Kimura/d3d3541dee057c583f39005b25df65c8
class IconToBitmaps
{
    public static List<IconProperties> ReadIconDirectory(int[] bytes, int numberOfIcons)
    {
        List<IconProperties> iconProperties = new List<IconProperties>();

        for (int i = 0;i < numberOfIcons; i++)
        {
            IconProperties icon = new IconProperties();
            //get icon attributes
            int listingStart = 6 + (i * 16);
            int width = bytes[listingStart + 0];
            if (width == 0) { width = 256; }
            
            int height = bytes[listingStart + 1];
            if (height == 0) { height = 256; }

            //imageSize = Math.Max(width, height);

            int size = bytes[listingStart+8];
            size += bytes[listingStart + 9]* 256;
            size += bytes[listingStart + 10] * 256 * 256;
            size += bytes[listingStart + 11] * 256 * 256 * 256;
            int offset = bytes[listingStart + 12];
            offset += bytes[listingStart + 13] * 256;
            offset += bytes[listingStart + 14] * 256 * 256;
            offset += bytes[listingStart + 15] * 256 * 256 * 256;
            Debug.WriteLine($"Icon, {width}x{height}, size: {size}b, starting at byte {offset}");     
            icon.index = i;
            icon.offset = offset;
            icon.size = size;
            icon.width = width;
            icon.height = height;
            iconProperties.Add( icon );
        }
        return iconProperties;
    }

    public static (int[] bytes, int numberOfIcons) ReadIconToBytes(FileStream inputStream)
    {
        long readLength = inputStream.Length;
        int[] bytes = new int[readLength];

        for (int i = 0; i < readLength; i++)
        {
            int b = inputStream.ReadByte();
            bytes[i] = b;
        }
        int numberOfIcons = bytes[4];

        Debug.WriteLine("Icons in file " + numberOfIcons);

        return (bytes, numberOfIcons);
    }

    public static void SaveFileFromBytes(int[] bytes, int imageSize, int index, int offset, int fileSize, string outputFolder, string filePrefix, string fileExtension)
    {
        //Save files
        string fileName = $"{filePrefix}-{index}-{imageSize}.{fileExtension}";
        string filePath = Path.Combine(outputFolder, fileName);
        using (FileStream output_stream = new FileStream(filePath, System.IO.FileMode.OpenOrCreate))
        {
            Debug.WriteLine($"Saving PNG-packed icon to PNG: {filePath}, size: {imageSize} from {offset} to {offset + fileSize} out of {bytes.Length} b");
            using (BinaryWriter bitmap_writer = new BinaryWriter(output_stream))
            {
                for (int j = 0; j < fileSize; j++)
                {
                    bitmap_writer.Write((byte)bytes[offset + j]);
                }
                bitmap_writer.Flush();
            }
            output_stream.Close();
        }
    }

    

    public static void Convert(string input_icon, string outputFolder, string filePrefix, string fileExtension, bool UnpackAsPNG)
    {
        FileStream input_stream = new FileStream(input_icon, System.IO.FileMode.Open);
        (int[] bytes, int num) = ReadIconToBytes(input_stream);
        List<IconProperties> iconList = ReadIconDirectory(bytes, num);// output_stream);
        if (UnpackAsPNG)
        {
            Debug.WriteLine("Save using PNG packed icon");
            foreach (IconProperties icon in iconList)
            {
                SaveFileFromBytes(bytes, icon.width, icon.index, icon.offset, icon.size, outputFolder, filePrefix, fileExtension);
            }
            input_stream.Close();
        }
        else
        {
            input_stream.Close();
            Debug.WriteLine("Save using ICO packed icon");
            List<int> sizes = new List<int>();
            foreach(IconProperties icon in iconList)
            {
                sizes.Add(icon.width);
            }
            UnpackIcoIcon(input_icon, sizes, outputFolder, filePrefix, fileExtension);
        }
    }

    public static void UnpackIcoIcon(string sourceFile, List<int> sizes, string outputFolder, string filePrefix, string fileExtension)
    {
        Debug.Write($"Saving ICO-packed icon {sourceFile} to PNG with sizes: ");
        foreach (int size in sizes)
        {
            Debug.Write($"{size} ");
        }
        Debug.WriteLine("");

        try
        {
            //for (int i = 0; i < sizes.Count; i++)
            //{
                for (int j = 0; j < sizes.Count; j++)
                {
                    Icon? icon = Icon.ExtractIcon(sourceFile, 0, sizes[j]);
                    //Icon icon = Icon.ExtractAssociatedIcon(sourceFile);
                    Debug.WriteLine($"Icon {sourceFile}, size {sizes[j]}, null: {icon == null}");

                    if (icon != null)
                    {
                        string fileName = $"{filePrefix}-{j}-{sizes[j]}.{fileExtension}";
                        string filePath = Path.Combine(outputFolder, fileName);
                        Debug.WriteLine($"Saving ICO-packed icon to PNG {filePath}: {icon.Width}x{icon.Height}");
                        icon.ToBitmap().Save(filePath);
                    }
                    else
                    {
                        Debug.WriteLine("Icon is null");
                    }
                }
            //}
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public class IconProperties
    {
        public int index;
        public int width;
        public int height;
        public int offset;
        public int size;
    }
}
