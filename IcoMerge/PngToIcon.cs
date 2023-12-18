using System.Diagnostics;

namespace IcoMerge
{
    // Rewrite of code from: https://github.com/Willy-Kimura
    //https://gist.github.com/Willy-Kimura/d3d3541dee057c583f39005b25df65c8
    class PngToIcon
    {
        /* input image with width = height is suggested to get the best result */
        /* png support in icon was introduced in Windows Vista */
        public static bool Convert(List<FileStream> inputStreams, Stream output_stream)//, bool keep_aspect_ratio = false)
        {
            List<pngStream> inputBitmaps = new List<pngStream>();
            foreach (Stream inputStream in inputStreams)
            {
                inputBitmaps.Add(new pngStream((Bitmap)Bitmap.FromStream(inputStream)));
            }
            
            if (inputBitmaps.Count > 0)
            {
                Debug.WriteLine($"PngToIcon, merging {inputBitmaps.Count} images");
                BinaryWriter icon_writer = new BinaryWriter(output_stream);
                if (output_stream != null && icon_writer != null)
                {
                    // 0-1 reserved, 0
                    icon_writer.Write((byte)0);
                    icon_writer.Write((byte)0);

                    // 2-3 image type, 1 = icon, 2 = cursor
                    icon_writer.Write((short)1);

                    int count = inputStreams.Count;
                    // 4-5 number of images
                    icon_writer.Write((short)count);

                    int fileOffset = 6 + (16 * count);

                    foreach (pngStream inputStream in inputBitmaps)
                    {

                        inputStream.new_bit = new Bitmap(inputStream.bitmap, new Size(inputStream.width, inputStream.height));
                        inputStream.mem_data = new MemoryStream();
                        inputStream.new_bit.Save(inputStream.mem_data, System.Drawing.Imaging.ImageFormat.Png);

                        // image entry 1
                        // 0 image width
                        icon_writer.Write((byte)inputStream.width);
                        // 1 image height
                        icon_writer.Write((byte)inputStream.height);

                        // 2 number of colors
                        icon_writer.Write((byte)0);

                        // 3 reserved
                        icon_writer.Write((byte)0);

                        // 4-5 color planes
                        icon_writer.Write((short)0);

                        // 6-7 bits per pixel
                        icon_writer.Write((short)32);

                        int dataLength = (int)inputStream.mem_data.Length;
                        // 8-11 size of image data
                        icon_writer.Write((int)dataLength);

                        // 12-15 offset of image data
                        //icon_writer.Write((int)(6 + 16));
                        icon_writer.Write((int)fileOffset);
                        fileOffset += (int)dataLength;
                        Debug.WriteLine($"PngToIcon, added image, w: {inputStream.width}, offset: {fileOffset}, size: {dataLength}");
                    }

                    foreach (pngStream inputStream in inputBitmaps)
                    {
                        // write image data
                        // png data must contain the whole png data file
                        if (inputStream.mem_data != null)
                        {
                            //s.Write(Data, 0, Data.Length);
                            icon_writer.Write(inputStream.mem_data.ToArray(), 0, (int)inputStream.mem_data.Length);
                            Debug.WriteLine($"PngToIcon, writing to file, length: {inputStream.mem_data.Length}");
                        }
                    }

                    icon_writer.Flush();

                    return true;
                }
                
                return false;
            }
            return false;
        }

        public static bool Convert(List<string> input_images, string output_icon)
        {
            Debug.WriteLine($"PngToIcon, saving {input_images.Count} images to {output_icon}");
            List<FileStream> inputStreams = new List<FileStream>();
            foreach (string img in input_images)
            {
                inputStreams.Add(new System.IO.FileStream(img, System.IO.FileMode.Open));
            }
            FileStream output_stream = new FileStream(output_icon, System.IO.FileMode.OpenOrCreate);

            bool result = Convert(inputStreams, output_stream);

            foreach (FileStream stream in inputStreams)
            {
                stream.Close();
            }
            
            output_stream.Close();

            return result;
        }
    }

    public class pngStream(Bitmap bmp)
    {
        public Bitmap bitmap { get; } = bmp;
        public int width
        {
            get { return bitmap.Width; }
        }
        public int height
        {
            get { return bitmap.Height; }
        }
        public Bitmap? new_bit;
        public MemoryStream? mem_data;
    }
}
