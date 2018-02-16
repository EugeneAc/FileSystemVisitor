using System;
using System.Net.Sockets;

namespace FileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {

            var visor = new FileSystemVisitor(@"D:\D1-D2\01. Advanced C#\01. Introduction to types", name => name.Contains("d"));
            visor.Start += (s, e) => Console.WriteLine("Started");
            visor.Finish += (s, e) => Console.WriteLine("Finished");
            visor.FileFinded += (s, e) => Console.WriteLine("File Finded " + e.FoundName);
            visor.DirectoryFinded += (s, e) => Console.WriteLine("Dir Finded " + e.FoundName);
            //visor.FilteredDirectoryFinded += FilteredDirFound;
            visor.FilteredDirectoryFinded += (s, e) =>
            {
                Console.WriteLine("Filtered Dir Finded " + e.FoundName);
            };
            //visor.FilteredFileFinded += FilteredFileFound;
            visor.FilteredFileFinded += (s, e) =>
            {
                Console.WriteLine("Filtered file Finded " + e.FoundName);
                if (e.FoundName.Contains("1"))
                {
                    e.StopSearch = true;
                };
            };


            foreach (var file in visor)
            {
                Console.WriteLine("Final item ");
                Console.WriteLine(file);
            }
            Console.ReadLine();
        }
        static void FilteredFileFound (ref bool stopsearch, ref bool exclude, string name)
        {
            if (name.Contains("vv"))
            {
                exclude = true;
                Console.WriteLine(name + "- excluded");
            }
        }

        static void FilteredDirFound(ref bool stopsearch, ref bool exclude, string name)
        {
            Console.WriteLine(name);
        }

    }
}
