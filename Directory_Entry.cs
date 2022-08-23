using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    internal class Directory_Entry
    {
        public char[] dir_name = new char[11];
        public byte dir_attr;
        public byte[] dire_empty = new byte[12];
        public int dir_first_cluster;
        public int dir_file_size;
        public static int directory_num = 5;
        public static int cluster_index;
        public static int last_cluster = -1;
        public Directory_Entry()
        {

        }
        public Directory_Entry(char[] name, byte attr, byte[] empty, int f_cluster, int size)
        {
            dir_name = name;
            dir_attr = attr;
            dire_empty = empty;
            dir_first_cluster = f_cluster;
            dir_file_size = size;
        }

        
       

    }

}
