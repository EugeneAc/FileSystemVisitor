using System;

namespace FileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {

            var visor = new FileSystemVisitor(@"d:\Test", name => name.Contains("txt"));
            visor.Start += (s, e) => Console.WriteLine("Started");
            visor.Finish += (s, e) => Console.WriteLine("Finished");
            visor.FileFinded += (s, e) => Console.WriteLine("File Finded " + e.FoundedName);
            visor.DirectoryFinded += (s, e) => Console.WriteLine("Dir Finded " + e.FoundedName);
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
            if (name.Contains("gggg"))
            {
                stopsearch = true;
                Console.WriteLine("!");
            }
        }

        static void FilteredDirFound(ref bool stopsearch, ref bool exclude, string name)
        {
            Console.WriteLine(name);
        }

    }
}
