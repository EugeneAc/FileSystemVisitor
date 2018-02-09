using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FileSystemVisitor
{
//    Создайте класс FileSystemVisitor, который позволяет обходить дерево папок в файловой системе, начиная с указанной точки.Указанный класс должен:
//•	Возвращать все найденные файлы и папки в виде линейной последовательности, для чего реализовать свой итератор(по возможности используя оператор yield)
//•	Давать возможность задать алгоритм фильтрации найденных файлов и папок в момент создания экземпляра FileSystemVisitor(через специальный перегруженный конструктор). Алгоритм должен задаваться в виде делегата/лямбды
//•	Генерировать уведомления(через механизм событий) о этапах своей работы.В частности, должны быть реализованы следующие события
//o   Start и Finish (для начала и конца поиска)
//o FileFinded/DirectoryFinded для всех найденных файлов и папок до фильтрации, и FilteredFileFinded/filteredDirectoryFinded для файлов и папок прошедших фильтрацию.Данные события должны позволять(через установку специальных флагов в переданных параметрах):
//	прервать поиск
//	исключить файлы/папки из конечного списка

    class FileSystemVisitor : IEnumerable<string>
    {
        private string _startAddress;
        private Func<string, bool> _searchPredicate;

        public event EventHandler Start;
        public event EventHandler Finish;

        public class FindedProgressArgs
        {
            public string FoundedName { get; internal set; }
        }

        public event EventHandler<FindedProgressArgs> FileFinded;
        public event EventHandler<FindedProgressArgs> DirectoryFinded;

        public event EventHandler<FindedProgressArgs> FilteredFileFinded;
        public event EventHandler<FindedProgressArgs> FilteredDirectoryFinded;

        public bool StopSerach { get; set; }
        public bool Exclude { get; set; }

        public FileSystemVisitor(string startingaddress, Func<string, bool> searchPredicate)
        {
            _startAddress = startingaddress;
            _searchPredicate = searchPredicate;
        }

        private IEnumerable<string> FindFiles()
        {
            Start?.Invoke(this, null);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(_startAddress);
            var fileList = System.IO.Directory.GetFileSystemEntries(_startAddress, "*.*", SearchOption.AllDirectories)
                .Where((s) => 
                {
                    if (StopSerach==true)
                    {
                        return false;
                    }
                    if (Directory.Exists(s))
                    {
                        DirectoryFinded?.Invoke(this, new FindedProgressArgs { FoundedName = s });
                        return CheckConditions(s,true);
                    }
                    else
                    {
                        FileFinded?.Invoke(this, new FindedProgressArgs { FoundedName = s });
                        return CheckConditions(s);
                    }
                }).ToList();
            Finish?.Invoke(this, null);
            StopSerach = false;
            return fileList;
        }

        private bool CheckConditions(string s, bool isdir=false)
        {
            if (_searchPredicate.Invoke(s))
            {
                if (isdir)
{
                    FilteredDirectoryFinded?.Invoke(this, new FindedProgressArgs { FoundedName = s });
                }
                else
                {
                    FilteredFileFinded?.Invoke(this, new FindedProgressArgs { FoundedName = s });
                }

                if (Exclude == true)
                {
                    Exclude = false;
                    return false;
                }
                else return true;
            }
            else
                return false;
        }

        private IEnumerable<string> _fileList { get { return FindFiles(); } }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var file in _fileList)
            {
                yield return file;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return _fileList.GetEnumerator();
        }
    }
}
