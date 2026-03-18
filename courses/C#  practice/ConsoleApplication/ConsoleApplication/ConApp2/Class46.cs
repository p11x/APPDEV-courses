using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //System.IO     -- DriveInfo
    internal class Class46
    {
        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo d in drives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label                       : {0}", d.VolumeLabel);
                    Console.WriteLine("  File system                        : {0}", d.DriveFormat);
                    Console.WriteLine("  Available space to current user    : {0} GB", d.AvailableFreeSpace / (1024 * 1024 * 1024));
                    Console.WriteLine("  Total available space              : {0} GB", d.TotalFreeSpace / (1024 * 1024 * 1024));
                    Console.WriteLine("  Total size of drive                : {0} GB", d.TotalSize / (1024 * 1024 * 1024));
                }
            }
        }
    }
}
