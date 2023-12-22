using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcoMerge
{
    public class PngStream(Bitmap bmp)
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
