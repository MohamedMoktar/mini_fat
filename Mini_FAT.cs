using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    internal class Mini_FAT
    {
        public static int[] FAT = new int[1024];
        public static int first_cluster = FAT[0];
        public void Prepare_FAT()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (i == 0 || i == 4)
                {
                    FAT[i] = -1;
                }
                else if (i == 1 || i == 2 || i == 3)
                {
                    FAT[i] = i + 1;
                }
                else
                {
                    FAT[i] = 0;
                }
            }
        }
        //CopyBuffer
        private static void copyBuffer(byte[] src, byte[] dest, int srcOffset, int srcEnd)
        {
            int c = 0;
            for (int i = srcOffset; i < srcEnd; i++)
            {
                dest[c] = src[i];
                c++;
            }
        }

        public void Write_FAT()
        {
            byte[] bytes = new byte[FAT.Length * sizeof(int)];
            Buffer.BlockCopy(FAT, 0, bytes, 0, bytes.Length);
            for (int i = 0; i < 4; i++)
            {
                byte[] buffer = new byte[1024];
                copyBuffer(bytes, buffer, i * 1024, (i + 1) * 1024);
                Virtual_Disk.Write_Cluster(i + 1, buffer);
            }
        }

        public void Read_FAT()
         {
            List<byte> ListOfBytes = new List<byte>();
            for (int i = 1; i <= 4; i++)
            {
                ListOfBytes.AddRange(Virtual_Disk.Read_Cluster(i));   
            }
            byte[] b = ListOfBytes.ToArray();
            System.Buffer.BlockCopy(FAT, 0, b, 0, b.Length);
        }

        public void Print_FAT()
        {
            Read_FAT();
            for (int i = 0; i < FAT.Length; i++)
            {
                Console.WriteLine(FAT[i]);
            }
        }

        public static int get_empty_place()
        {
            int i;
            if (FAT[FAT.Length] != -1)
            {
                for (i = 0; i < FAT.Length; i++)
                {
                    if (FAT[i] == 0)
                    {
                        break;
                    }
                }
                return FAT[i];
            }
            else
            {
                Array.Clear(FAT, FAT[0], FAT.Length);
                return (first_cluster);
            }

        }

        public static void Set_Cluster_Pointer(int Cluster_Index, int value)
        {
            FAT[Cluster_Index] = value;

        }

        public static int Get_Cluster_Pointer(int Cluster_Index)
        {
            return FAT[Cluster_Index];
        }
    }
}
