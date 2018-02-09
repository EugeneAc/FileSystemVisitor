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
            int?[] myarray = new int?[5] { null, 2, 3, 4, 5 };
            Console.WriteLine(Get2ndMaxNumber(myarray));
            foreach (var el in myarray)
            {
                Console.WriteLine(el);
            }

            var worker = new BCLWorker();
            worker.Progress += worker_Progress;
            worker.DoWork();
            Console.WriteLine("");
            var visor = new FileSystemVisitor(@"d:\Test", name => name.Contains("txt"));
            visor.Start += (s,e)=> Console.WriteLine("Started");
            visor.Finish += (s, e) => Console.WriteLine("Finished");
            visor.FileFinded += (s, e) => Console.WriteLine("File Finded "+e.FoundedName);
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

        static private void worker_Progress(object sender, BCLWorker.WorkerProgressArgs e)
        {
            Console.WriteLine(e.ProgressPercent);
        }

        static int? Get2ndMaxNumber(int?[] inarray)
        {
            try
            {
                var internalarray = inarray;
                int maxindex = Array.IndexOf(internalarray, inarray.Max());
                internalarray[maxindex] = null;
                throw new NullReferenceException();
                return internalarray.Max();
            }
            catch (Exception)
            {
                return 1;
            }
            finally
            {
                var internalarray = inarray;
                int maxindex = Array.IndexOf(internalarray, inarray.Max());
                internalarray[maxindex] = null;
                
            }
           
        }
    }

    public class BCLWorker
    {
        public class WorkerProgressArgs
        {
            public int ProgressPercent { get; internal set; }
        }

        public event EventHandler<WorkerProgressArgs> Progress;

        protected virtual void OnProgress(WorkerProgressArgs args)
        {
            Progress?.Invoke(this, args);
        }

        public void DoWork()
        {
            for (int i = 0; i < 10; i++)
            {
                OnProgress(
                    new WorkerProgressArgs { ProgressPercent = i * 10 });
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    [TestClass]
    public class BCLEvents
    {
        [TestMethod]
        public void TestMethod1()
        {
            var worker = new BCLWorker();
            worker.Progress += worker_Progress;
            worker.DoWork();
        }

        private void worker_Progress(object sender, BCLWorker.WorkerProgressArgs e)
        {
            Console.WriteLine(e.ProgressPercent);
        }
    }
}
