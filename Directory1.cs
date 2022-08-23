
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    internal class Directory1 : Directory_Entry
    {
        public List<Directory_Entry> entries;
        public Directory1 parent;
        
        public void write_Directory()
        {
            byte[] container = new byte[directory_num * 32];
            //--------------convertion-----------------//
            for (int i = 0; i < directory_num; i++)
            {
                byte[] dir_attribute = new byte[32];
                dir_attribute[dir_attribute.Length] = Convert.ToByte(dir_name);
                dir_attribute[dir_attribute.Length] = Convert.ToByte(dir_attr);
                dir_attribute[dir_attribute.Length] = Convert.ToByte(dire_empty);
                dir_attribute[dir_attribute.Length] = Convert.ToByte(dir_first_cluster);
                dir_attribute[dir_attribute.Length] = Convert.ToByte(dir_file_size);

                for (int j = container.Length; j < dir_attribute.Length; j++)
                {
                    int index = 0;
                    container[j] = dir_attribute[index];
                    index++;
                }
                Array.Clear(dir_attribute, 0, dir_attribute.Length);
            }
            //------------------storing-------------------//
            List<byte[]> in_container = new List<byte[]>();
            for (int i = 1; i == 1024; i++)
            {
                for (int j = 1; j == 1024; j++)
                {
                    in_container[i][j] = container[j];
                }
            }
            //-------------geting place--------------------//
            int fc = 0;
            if (fc == 0)
            {
                cluster_index = Mini_FAT.get_empty_place();
            }
            else
            {
                cluster_index = fc;
            }

            for (int i = 0; i < 1024; i++)
            {
                if (cluster_index != 0)
                {
                    Virtual_Disk.Write_Cluster(cluster_index, in_container[i]);
                    Mini_FAT.Set_Cluster_Pointer(cluster_index, -1);
                }
                if (last_cluster != 0)
                {
                    Mini_FAT.Set_Cluster_Pointer(last_cluster, cluster_index);
                    last_cluster = cluster_index;
                }
                cluster_index = Mini_FAT.get_empty_place();
            }
        }
        public static void Read_Directory()
        {
            int FC = 0;
            if (FC != 0)
            {
                List<Directory_Entry> directories;  //ununderstand this error
                int Cluster_Index = FC;
                int Next = Mini_FAT.Get_Cluster_Pointer(Cluster_Index);
                List<byte> LS = new List<byte>();
                do
                {
                    LS.AddRange(Virtual_Disk.Read_Cluster(Cluster_Index));
                    if (Cluster_Index != -1)
                    {
                        Next = Mini_FAT.Get_Cluster_Pointer(Cluster_Index);
                        Cluster_Index = Next;
                    }
                }
                while (Cluster_Index != -1);
            }
        }


        public int Search(char[] name)
            {
            foreach (Directory_Entry i in entries)
            {
                int index = 0;
                if (name == dir_name)
                {
                    return index;
                    break;
                }
                index++;
            }
            return -1;
        }

        public Directory_Entry Get_Directory(Directory_Entry opject)
        {
            opject.dire_empty = this.dire_empty;
            opject.dir_name = this.dir_name;
            opject.dir_first_cluster = this.dir_first_cluster;
            opject.dir_file_size = this.dir_file_size;
            opject.dir_attr = this.dir_attr;
            return opject;
        }

        public void Remove(char[] name)
        {
            foreach (Directory_Entry i in entries)
            {
                int index = 0;
                if (name == dir_name)
                {
                    entries.RemoveAt(index);
                    break;
                }
                index++;
            }
            write_Directory();
        }

        public void Delete(Directory_Entry opject)
        {
            int index = opject.dir_first_cluster;
            int next = Mini_FAT.Get_Cluster_Pointer(index);
            do
            {
                Mini_FAT.Set_Cluster_Pointer(index, 0);
                index = next;
                if (index != -1)
                    next = Mini_FAT.Get_Cluster_Pointer(index);
            }
            while (index != -1);
            parent.Remove(opject.dir_name);
           parent. write_Directory();
        }

        public void Update(Directory_Entry new_opject, Directory_Entry old_opject)
        {
            int index = Search(old_opject.dir_name);
            entries.RemoveAt(index);
            entries.Insert(index, new_opject);
        }

        public int Get_Size(Directory_Entry opject)
        {
            int counter = 0;
            int index = opject.dir_first_cluster;
            int next = Mini_FAT.Get_Cluster_Pointer(index);
            while (true)
            {
                index = next;
                if (index != -1)
                {
                    next = Mini_FAT.Get_Cluster_Pointer(index);
                }
                else if (index == -1)
                {
                    break;
                }
                counter++;
            }
            return counter * 1024;
        }

        public void Add_Directory(Directory_Entry opject)
        {
            int size = Get_Size(opject);
            int list_size = 0;
            if (size % 1024 == 0)
            {
                size = size / 1024;
            }
            else
            {
                size = (size / 1024) + 1;
            }
            for (int i = 0; i < entries.Count; i++)
            {
                list_size = list_size + Get_Size(entries[i]);
            }
            if (list_size % 1024 == 0)
            {
                list_size = list_size / 1024;
            }
            else
            {
                list_size = (list_size / 1024) + 1;
            }
            if (1024 - list_size > size)
            {
                entries.Add(opject);
            }
            else
            {
                Console.WriteLine("sorry there is no space");
            }

        }



    }
}
