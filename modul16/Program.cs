using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modul16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к отслеживаемой директории:");
            string directoryPath = Console.ReadLine();

            Console.WriteLine("Введите путь к лог-файлу:");
            string logFilePath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Директория не существует. Программа будет закрыта.");
                return;
            }

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = directoryPath;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;

                watcher.Filter = "*.*";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Нажмите 'q' для выхода.");
                while (Console.Read() != 'q') ;
            }

            void OnChanged(object sender, FileSystemEventArgs e)
            {
                LogChange($"Изменён: {e.FullPath}");
            }

            void OnCreated(object sender, FileSystemEventArgs e)
            {
                LogChange($"Создан: {e.FullPath}");
            }

            void OnDeleted(object sender, FileSystemEventArgs e)
            {
                LogChange($"Удалён: {e.FullPath}");
            }

            void OnRenamed(object sender, RenamedEventArgs e)
            {
                LogChange($"Переименован: {e.OldFullPath} в {e.FullPath}");
            }

            void OnError(object sender, ErrorEventArgs e)
            {
                LogChange($"Ошибка: {e.GetException().Message}");
            }

            void LogChange(string message)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(logFilePath, true))
                    {
                        sw.WriteLine($"{DateTime.Now}: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи в лог-файл: {ex.Message}");
                }
            }
        }
    }
}
