using System.Diagnostics;

namespace IcoMerge
{
    // Rewrite of code from: https://github.com/Willy-Kimura
    //https://gist.github.com/Willy-Kimura/d3d3541dee057c583f39005b25df65c8
    class IconToBitmaps
    {
        public static void UnpackIcon(FileStream inputStream, string outputPath)
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

                if (outputPath.Length > 0)
                {
                    string filePath = outputPath + i + ".png";
                    using (FileStream output_stream = new FileStream(filePath, System.IO.FileMode.OpenOrCreate))
                    {
                        Debug.WriteLine($"Saving to file: {filePath}, size: {size} from {offset} to {offset+size} out of {bytes.Length} b");
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
        }

        

        public static void LoadFiles(string input_icon, string output_image)
        {
            FileStream input_stream = new FileStream(input_icon, System.IO.FileMode.Open);
            //FileStream? output_stream = null;
            //if (output_image.Length > 0)
            //{
            //    output_stream = new FileStream(output_image, System.IO.FileMode.OpenOrCreate);
            //}

            UnpackIcon(input_stream, output_image);// output_stream);

            input_stream.Close();
            //if (output_stream != null)
            //{
            //    output_stream.Close();
            //}
        }
    }
}
