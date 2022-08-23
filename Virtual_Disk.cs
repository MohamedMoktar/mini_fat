using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    internal class Virtual_Disk
    {   
        public static FileStream Disk;
        public void initialz(string path)
        {
            Mini_FAT inti = new Mini_FAT();
            if (!File.Exists(path))
            {
                Disk = new FileStream(path, FileMode.OpenOrCreate,FileAccess.ReadWrite);
                byte[] cluster0 = new byte[1024];
                Disk.Write(cluster0);
                inti.Prepare_FAT();
                inti.Write_FAT();
            }
            else
            {
                Disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                inti.Read_FAT();
            }
        }

        public static void Write_Cluster(int cluster_index, byte[] text)
        {
            Disk.Seek(cluster_index * 1024, SeekOrigin.Begin);
            Disk.Write(text);
            Disk.Flush();
        }

        public static byte[] Read_Cluster(int cluster_index)
        {
            Disk.Seek(cluster_index * 1024, SeekOrigin.Begin);
            byte[]b=new byte[1024];
            Disk.Read(b,0,1024);
            return b;
        }
    }
}
