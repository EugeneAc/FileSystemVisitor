using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            visor.FilteredDirectoryFinded += (s, e) => Console.WriteLine("Filtered Dir Finded " + e.FoundedName);
            visor.FilteredFileFinded += (s, e) =>
            {
                var sender = (FileSystemVisitor)s;
                Console.WriteLine("Filtered File Finded " + e.FoundedName);
                if (e.FoundedName.Contains("1"))
                {
                    sender.StopSerach = true;
                    Console.WriteLine("!");
                }
            };
            Console.WriteLine(" ");

            foreach (var file in visor)
            {
                Console.WriteLine(file);
            }
            Console.ReadLine();
        }

    }
}
