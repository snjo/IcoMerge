# IcoMerge
*by Andreas Aakvik Gogstad*  
*2023*

![image](screenshot.png)

Converts multiple PNG files into a single multi-resolution ICO file

-Can merge several PNGs with different resolutions into one ICO file.

-Can export all icons inside an ICO file into PNGs

Exporting using the option "Unpack ICO-formatted icon file" loads images from the ICO and saves the bitmaps to new files (using intermediate bitmap objects). This is the recommended option, and works for any icon.

Exporting with "Unpack PNG-formatted icon file" only works for icons assembled with PNG images, and dumps the raw files used when creating the original file. This will not work for traditional ICO files.
