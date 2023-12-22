using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;


namespace IcoMerge;

// Rewrite of code from: https://github.com/Willy-Kimura
//https://gist.github.com/Willy-Kimura/d3d3541dee057c583f39005b25df65c8
class IconToBitmaps
{
    public static void UnpackPngIcon(FileStream inputStream, string outputFolder, string filePrefix, string fileExtension)
    {

        long readLength = inputStream.Length;
        int[] bytes = new int[readLength];

        for (int i = 0; i < readLength; i++)
        {
            int b = inputStream.ReadByte();
            bytes[i] = b;
        }
        int numImages = bytes[4];
        Debug.WriteLine("Icons in file " +  numImages);

        for (int i = 0;i < numImages;i++)
        {
            //get icon attributes
            int listingStart = 6 + (i * 16);
            int width = bytes[listingStart + 0];
            if (width == 0) { width = 256; }
            
            int height = bytes[listingStart + 1];
            if (height == 0) { height = 256; }

            int size = bytes[listingStart+8];
            size += bytes[listingStart + 9]* 256;
            size += bytes[listingStart + 10] * 256 * 256;
            size += bytes[listingStart + 11] * 256 * 256 * 256;
            int offset = bytes[listingStart + 12];
            offset += bytes[listingStart + 13] * 256;
            offset += bytes[listingStart + 14] * 256 * 256;
            offset += bytes[listingStart + 15] * 256 * 256 * 256;
            Debug.WriteLine($"Icon, {width}x{height}, size: {size}b, starting at byte {offset}");

            //Save files
            string fileName = $"{filePrefix}-{i}-{width}.{fileExtension}";
            string filePath = Path.Combine(outputFolder, fileName);
            using (FileStream output_stream = new FileStream(filePath, System.IO.FileMode.OpenOrCreate))
            {
                Debug.WriteLine($"Saving PNG-packed icon to PNG: {filePath}, size: {size} from {offset} to {offset+size} out of {bytes.Length} b");
                using (BinaryWriter bitmap_writer = new BinaryWriter(output_stream))
                {
                    for (int j = 0; j < size; j++)
                    {
                        bitmap_writer.Write((byte)bytes[offset+j]);
                    }
                    bitmap_writer.Flush();
                }
                output_stream.Close();
            }
            
        }
    }

    

    public static void LoadFiles(string input_icon, string outputFolder, string filePrefix, string fileExtension, bool UnpackAsPNG)
    {
        if (UnpackAsPNG)
        {
            FileStream input_stream = new FileStream(input_icon, System.IO.FileMode.Open);
            UnpackPngIcon(input_stream, outputFolder, filePrefix, fileExtension);// output_stream);
            input_stream.Close();
        }
        else
        {
            UnpackIcoIcon(input_icon, outputFolder, filePrefix, fileExtension);
        }
        
    }

    public static void UnpackIcoIcon(string sourceFile, string outputFolder, string filePrefix, string fileExtension)
    {
        int[] sizes = { 16, 32, 24, 48, 64, 256 };
        try
        {
            for (int j = 0; j < sizes.Length; j++)
            {
                Icon? icon = Icon.ExtractIcon(sourceFile, 0, sizes[j]);
                if (icon != null)
                {
                    string fileName = $"{filePrefix}-{j}-{sizes[j]}.{fileExtension}";
                    string filePath = Path.Combine(outputFolder, fileName);
                    Debug.WriteLine($"Saving ICO-packed icon to PNG {filePath}: {icon.Width}x{icon.Height}");
                    icon.ToBitmap().Save(filePath);
                }
            } 
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
