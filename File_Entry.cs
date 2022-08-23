using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    internal class File_Entry : Directory_Entry
    {
        string contanit=" ";
        Directory1 parent;
        byte[] container = new byte[1024 * 1024];
        public void Delete(File_Entry opject)
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
        public File_Entry Get_Directory(File_Entry opject)
        {
            opject.dire_empty = this.dire_empty;
            opject.dir_name = this.dir_name;
            opject.dir_first_cluster = this.dir_first_cluster;
            opject.dir_file_size = this.dir_file_size;
            opject.dir_attr = this.dir_attr;
            return opject;
        }
        public int Get_Size(File_Entry opject)
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
        public void write(string new_containt)
        {
           
            for (int i = 0; i < new_containt.Length; i++)
            {
                container[container.Length] = Convert.ToByte(new_containt[i]);
            }
            int fc = 0;
            if (fc == 0)
            {
                cluster_index = Mini_FAT.get_empty_place();
            }
            else
            {
                cluster_index = fc;
                //empty this file

            }

            for (int i = 0; i < 1024; i++)
            {
                if (cluster_index != 0)
                {
                    Virtual_Disk.Write_Cluster(cluster_index, container);
                    Mini_FAT.set_ClusterPointer(cluster_index, -1);
                }
                if (last_cluster != 0)
                {
                    Mini_FAT.set_ClusterPointer(last_cluster, cluster_index);
                    last_cluster = cluster_index;
                }
                cluster_index = Mini_FAT.get_empty_place();
            }
            contanit = contanit + new_containt;

        }

        public char[] Read()
        {
            char[] the_string = new char[1024 * 0124];
            for (int i = 0; i < container.Length; i++)
            {
                the_string[i] = Convert.ToChar(container[i]);
            }
            return the_string;

        }
        public void print()
        {
            Console.WriteLine(this.dir_name);
            Console.WriteLine(contanit);
        }
























    }
}
