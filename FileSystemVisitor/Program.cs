using System;

namespace FileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {

            var visor = new FileSystemVisitor(@"d:\Test", name => name.Contains("d"));
            visor.Start += (s, e) => Console.WriteLine("Started");
            visor.Finish += (s, e) => Console.WriteLine("Finished");
            visor.FileFinded += (s, e) => Console.WriteLine("File Finded " + e.FoundName);
            visor.DirectoryFinded += (s, e) => Console.WriteLine("Dir Finded " + e.FoundName);
            visor.FilteredDirectoryFinded += FilteredDirFound;
            visor.FilteredFileFinded += FilteredFileFound;

            Console.WriteLine(" ");

            foreach (var file in visor)
            {
                Console.WriteLine(file);
            }
            Console.ReadLine();
        }
        static void FilteredFileFound (ref bool stopsearch, ref bool exclude, string name)
        {
            if (name.Contains("1"))
            {
                exclude = true;
                Console.WriteLine("!");
            }
        }

        static void FilteredDirFound(ref bool stopsearch, ref bool exclude, string name)
        {
            Console.WriteLine(name);
        }

    }
}
