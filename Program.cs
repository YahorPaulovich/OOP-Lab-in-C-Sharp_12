using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace StreamingClassesFileSystem//Вариант 14
{/*№ 13 Работа с потоковыми классами и файловой системой*/
    class Program
    {/*Задание
Каждый класс в данном проекте должен начинаться (Префикс) с ваших
инициалов ФИО (AVF, JK,….). Предусмотреть обработку ошибок.
1. Создать класс XXXLog. Он должен отвечать за работу с текстовым файлом
xxxlogfile.txt. в который записываются все действия пользователя и
соответственно методами записи в текстовый файл, чтения, поиска нужной
информации.
a. Используя данный класс выполните запись всех
последующих действиях пользователя с указанием действия,
детальной информации (имя файла, путь) и времени
(дата/время)
2. Создать класс XXXDiskInfo c методами для вывода информации о
a. свободном месте на диске
b. Файловой системе
c. Для каждого существующего диска - имя, объем, доступный
объем, метка тома.
d. Продемонстрируйте работу класса
3. Создать класс XXXFileInfo c методами для вывода информации о
конкретном файле
a. Полный путь
b. Размер, расширение, имя
c. Время создания
d. Продемонстрируйте работу класса
4. Создать класс XXXDirInfo c методами для вывода информации о конкретном
директории
a. Количестве файлов
b. Время создания
c. Количестве поддиректориев
d. Список родительских директориев
e. Продемонстрируйте работу класса
5. Создать класс XXXFileManager. Набор методов определите
самостоятельно. С его помощью выполнить следующие действия:
a. Прочитать список файлов и папок заданного диска. Создать
директорий XXXInspect, создать текстовый файл
xxxdirinfo.txt в нем и сохранить туда эту информацию.
Создать копию файла и переименовать его. Удалить
первоначальный файл.
b. Создать еще один директорий XXXFiles. Скопировать в
него все файлы с заданным расширением из заданного
пользователем директория. Переместить XXXFiles в
XXXInspect.
c. Сделайте архив из файлов директория XXXFiles.
Разархивируйте его в другой директорий.
6. Найдите и выведите сохраненную информацию в файле xxxlogfile.txt о
действиях пользователя за определенный день/ диапазон времени/по
ключевому слову. Посчитайте количество записей в нем. Удалите часть
информации, оставьте только записи за текущий час.*/
        static async Task Main(string[] args)
        {
            var PEDlog = new PEDLog();
            PEDlog.StartWatch(@"C:\Users\Egor\Desktop\");
            await PEDlog.WriteToTextFile();
            await PEDlog.ReadingFromATextFile();
            await PEDlog.FindingTheRightInformation("Text");
            Console.WriteLine(PEDlog.FilePath);

            var PEDDiskinfo = new PEDDiskInfo();
            PEDDiskinfo.DisplayFreeDiskSpace("C");
            PEDDiskinfo.DisplayFileSystemInformation("C");
            PEDDiskinfo.DisplayInformation();

            var PEDFileinfo = new PEDFileInfo(@"C:\Users\Egor\Desktop\Работа с потоковыми классами и файловой системой\PEDlogfile.txt");
            PEDFileinfo.DisplayFullWay();
            PEDFileinfo.DisplaySizeExtensionName();
            PEDFileinfo.DisplayCreationTime();

            var PEDDirinfo = new PEDDirInfo(@"C:\Users\Egor\");
            Console.WriteLine($"a. количество файлов: {PEDDirinfo.NumberOfFiles}");
            Console.WriteLine($"b. время создания всего каталога:: {PEDDirinfo.TimeOfCreation}");
            Console.WriteLine($"c. количестве поддиректориев: {PEDDirinfo.NumberOfSubdirectories}");
            PEDDirinfo.ParentDirectoryList();

            var PEDFilemanager = new PEDFileManager("C");
            await PEDFilemanager.SaveToFile();

            //Создание копии файла
            File.Copy("C:\\Users\\Egor\\Desktop\\PEDInspect\\peddirinfo.txt", @"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfo.txt", true);
            Console.WriteLine("Создание копии файла...");
            PEDFilemanager.CreateNewDirAndMoveOld(@"C:\Users\Egor\Desktop\Новая папка", ".rar");
            FileInfo fileInfo = new FileInfo(@"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfoRenamed.txt");
            FileInfo fileInfo2 = new FileInfo(@"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfo.txt");
            if (!fileInfo.Exists || !fileInfo2.Exists)
            {
                // Переименование файла
                File.Move(@"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfo.txt", @"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfoRenamed.txt");
                Console.WriteLine("Переименование файла...");
            }

            // Удаление первоначального файла
            File.Delete(@"C:\Users\Egor\Desktop\PEDInspect\peddirinfo.txt");
            Console.WriteLine("Удаление первоначального файла...");

            // Разархивация файла в другой директорий
            Console.WriteLine("Разархивация файла в другой директорий...");
            string sourceFile = @"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfoRenamed.txt"; // исходный файл
            string compressedFile = @"C:\Users\Egor\Desktop\PEDInspect\PEDFiles\peddirinfoRenamed.gz"; // сжатый файл
            string targetFile = @"C:\Users\Egor\Desktop\PEDInspect\peddirinfoRenamed_new.txt"; // восстановленный файл
            PEDFilemanager.CompressAndDecompress(sourceFile, compressedFile, targetFile);

            /*6. Вывод сохраненной информации в файле PEDlogfile.txt о действиях пользователя*/
            Console.WriteLine("6. Вывод сохраненной информации в файле PEDlogfile.txt о действиях пользователя...\n");

            // Вывод информации о действиях пользователя за определенный день
            Console.WriteLine("Вывод информации о действиях пользователя за определенный день...");
            await PEDlog.FindingTheRightInformation("00.00.2020");

            // Вывод информации по диапазону времени
            Console.WriteLine("\nВывод информации по диапазону времени...");
            await PEDlog.FindingTheRightInformation("00.00.00.00");

            // Вывод информации по ключевому слову
            Console.WriteLine("\nВывод информации по ключевому слову...");
            await PEDlog.FindingTheRightInformation("peddirinfoRenamed.txt");

            // Подсчёт количества записей
            Console.WriteLine("\nПодсчёт количества записей...");
            string Path = PEDlog.FilePath + @"\PEDlogfile.txt";
            Console.WriteLine($"Количество записей: {File.ReadAllLines(Path).Length}");

            // Удаление повторяющейся части информации
            Console.WriteLine("\nУдаление повторяющейся части информации...");
            try
            {
                string[] lines = File.ReadAllLines(Path, System.Text.Encoding.Default);
                var list = new List<string>(lines);
                var uniqueStrings = list.Distinct();
                File.WriteAllLines(Path, uniqueStrings, System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadKey();
        }
    }

    #region PEDLog
    // Отвечает за работу с текстовым файлом PEDlogfile.txt
    #endregion
    public class PEDLog//1.
    {
        public string FilePath { get; private set; }
        public string SearchedInformation { get; set; }
        public string Folder { get; private set; }
        public string Text { get; set; }
        public int Count { get; private set; } = 0;
        public PEDLog() { }
        public void StartWatch(string Folder)
        {
            if (Folder != " ")
            {
                DirectoryInfo dirInf = new DirectoryInfo(Folder);
                if (!dirInf.Exists)
                {
                    dirInf.Create();
                    this.Folder = Folder;
                }
                else
                {
                    this.Folder = Folder;
                }
            }
            else
            {
                this.Folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                this.Folder = Path.Combine(this.Folder, "Работа с потоковыми классами и файловой системой");
                DirectoryInfo dirInf = new DirectoryInfo(this.Folder);
                if (!dirInf.Exists)
                {
                    dirInf.Create();
                }
            }

            FileSystemWatcher watcher = new FileSystemWatcher(this.Folder);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
        }
        private async void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Text = $"Файл: {e.OldName} переименован в {e.Name}. Время: {DateTime.Now.ToLocalTime()}";
            await Task.Run(async () =>
            {
                await WriteToTextFile();
            });
        }
        private async void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Text = $"Файл: {e.Name} удалён. Время: {DateTime.Now.ToLocalTime()}";
            await Task.Run(async () =>
            {
                await WriteToTextFile();
            });
        }
        private async void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Text = $"Файл: {e.Name} создан. Время: {DateTime.Now.ToLocalTime()}";
            await Task.Run(async () =>
            {
                await WriteToTextFile();
            });
        }

        #region Метод записи

        #endregion
        public async Task WriteToTextFile()
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FilePath = Path.Combine(FilePath, "Работа с потоковыми классами и файловой системой");
            DirectoryInfo dirInf = new DirectoryInfo(FilePath);
            if (!dirInf.Exists)
            {
                dirInf.Create();
            }

            await Task.Run(async () =>
            {
                string Path = FilePath + @"\PEDlogfile.txt";
                try
                {
                    using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
                    {
                        await sw.WriteLineAsync(Text);
                    }
                    //Console.WriteLine("Запись прошла успешно!");
                }
                catch { }
            });
        }
        #region Метод чтения

        #endregion
        public async Task ReadingFromATextFile()
        {
            Count = 0;
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FilePath = System.IO.Path.Combine(FilePath, "Работа с потоковыми классами и файловой системой");
            DirectoryInfo dirInf = new DirectoryInfo(FilePath);
            if (!dirInf.Exists)
            {
                dirInf.Create();
            }

            string Path = FilePath + @"\PEDlogfile.txt";
            if (File.Exists(Path))
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        using (StreamReader sr = new StreamReader(Path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = await sr.ReadLineAsync()) != null)
                            {
                                Console.WriteLine($"{line}");
                                Count++;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #region Метод поиска нужной информации

        #endregion
        public async Task FindingTheRightInformation(string SearchedInformation)
        {
            this.SearchedInformation = SearchedInformation;
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FilePath = System.IO.Path.Combine(FilePath, "Работа с потоковыми классами и файловой системой");
            DirectoryInfo dirInf = new DirectoryInfo(FilePath);
            if (!dirInf.Exists)
            {
                dirInf.Create();
            }

            string Path = FilePath + @"\PEDlogfile.txt";
            if (File.Exists(Path))
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        using (StreamReader sr = new StreamReader(Path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = await sr.ReadLineAsync()) != null)
                            {
                                if (line.Contains(this.SearchedInformation))
                                {
                                    Console.WriteLine($" {line}");
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
    public class PEDDiskInfo//2.
    {
        private string HDD { get; set; }
        public PEDDiskInfo(){ }
       
        public void DisplayFreeDiskSpace(string HDD)
        {
            this.HDD = HDD + @":\";
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.Name == this.HDD)
                {
                    if (drive.IsReady)
                    {
                        Console.WriteLine($@"Диск {this.HDD}, свободное пространство: {drive.TotalFreeSpace}");
                    }
                }
            }
            Console.WriteLine();
        }
        public void DisplayFileSystemInformation(string HDD)
        {
            this.HDD = HDD + @":\";
            if (Directory.Exists(this.HDD))
            {
                Console.WriteLine("Подкаталоги:");
                string[] dirs = Directory.GetDirectories(this.HDD);
                foreach (string s in dirs)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
                Console.WriteLine("Файлы:");
                string[] files = Directory.GetFiles(this.HDD);
                foreach (string s in files)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
            }

        }
        public void DisplayInformation()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
        }

    }
    public class PEDFileInfo//3.
    {
        private string SpecificFile { get; set; }
        public string FileName { get; set; }
        public string FullWay { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; set; }
        public DateTime FileCreationTime { get; set; }
        public PEDFileInfo(string SpecificFile)
        {
            this.SpecificFile = SpecificFile;
            FileInfo fileInf = new FileInfo(this.SpecificFile);
            if (fileInf.Exists)
            {
                FullWay = fileInf.FullName;
                FileSize = fileInf.Length;
                FileExtension = fileInf.Extension;
                FileName = fileInf.Name;
                FileCreationTime = fileInf.CreationTime;
            }
        }

        //a.Полный путь //FullWay
        public void DisplayFullWay()
        {
            Console.WriteLine($"Полное имя файла: {FullWay}.");
        }

        //b.Размер, расширение, имя //FileSize, FileExtension, FileName
        public void DisplaySizeExtensionName()
        {
            Console.WriteLine($"Размер: {FileSize}; \nРасширение: {FileExtension}; \nИмя файла: {FileName}.");
        }

        //c.Время создания //FileCreationTime
        public void DisplayCreationTime()
        {
            Console.WriteLine($"Время создания: {FileCreationTime}.");
        }
    }
    public class PEDDirInfo//4.
    {       
        private string SpecificDirectory { get; set; }
        public int NumberOfFiles { get; set; } = 0;
        public int NumberOfSubdirectories { get; set; } = 0;
        public DateTime TimeOfCreation { get; set; }
        public PEDDirInfo(string SpecificDirectory)
        {
            this.SpecificDirectory = SpecificDirectory;
            if (Directory.Exists(this.SpecificDirectory))
            {
                string[] files = Directory.GetFiles(this.SpecificDirectory);
                foreach (string s in files)
                {
                    NumberOfFiles += 1;
                }
                DirectoryInfo dirInfo = new DirectoryInfo(this.SpecificDirectory);
                TimeOfCreation = dirInfo.CreationTime;
                string[] dirs = Directory.GetDirectories(this.SpecificDirectory);
                foreach (string s in dirs)
                {
                    NumberOfSubdirectories += 1;
                }
            }
        }

        //d. Список родительских директориев
        public void ParentDirectoryList()//GetParent(path): получение родительского каталога
        {
            if (Directory.Exists(this.SpecificDirectory))
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(this.SpecificDirectory));
                foreach (string s in dirs)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
    public class PEDFileManager//5.
    {
        private string HDD { get; set; }
        public string FilePath { get; private set; }
        public string FilePath2 { get; set; }
        private string Dir { get; set; }
        public string Extension { get; set; }
        public string SourceFile { get; set; }
        public string CompressedFile { get; set; }
        public string TargetFile { get; set; }
        public PEDFileManager(string HDD)
        {
            this.HDD = HDD + @":\";
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FilePath = Path.Combine(FilePath, @"PEDInspect");
            FilePath2 = @"C:\Users\Egor\Desktop\PEDInspect\PEDFiles";
            string path = @"C:\Users\Egor\Desktop\Новая папка";
            DirectoryInfo dirInf = new DirectoryInfo(FilePath);
            DirectoryInfo dirInf2 = new DirectoryInfo(FilePath2);
            DirectoryInfo dirInf3 = new DirectoryInfo(path);
            if (!dirInf.Exists | !dirInf2.Exists)
            {
                dirInf.Create();
                dirInf2.Create();
                dirInf3.Create();
            }
        }
        public async Task SaveToFile()
        {
            await Task.Run(async () =>
            {
                if (Directory.Exists(this.HDD))
                {
                    string Path = FilePath + @"\peddirinfo.txt";
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
                        {
                            await sw.WriteLineAsync("Подкаталоги:");
                            string[] dirs = Directory.GetDirectories(this.HDD);
                            foreach (string d in dirs)
                            {
                                await sw.WriteLineAsync(d);
                            }
                            await sw.WriteLineAsync("\n");

                            await sw.WriteLineAsync("Файлы:");
                            string[] files = Directory.GetFiles(this.HDD);
                            foreach (string f in files)
                            {
                                await sw.WriteLineAsync(f);
                            }
                            await sw.WriteLineAsync("\n");
                        }
                        Console.WriteLine("Запись прошла успешно!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }
        public void CreateNewDirAndMoveOld(string Dir, string Extension)
        {
            this.Dir = Dir;
            this.Extension = Extension;

            try
            {
                Console.WriteLine("Копирование всех файлов с заданным расширением...");
                string[] files = Directory.GetFiles(this.Dir);
                foreach (string item in files)
                {
                    if (item.Contains(this.Extension))
                    {
                        FileInfo fileInf = new FileInfo(item);
                        if (fileInf.Exists)
                        {
                            fileInf.MoveTo(FilePath2 + fileInf.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }          
        }

        public void CompressAndDecompress(string SourceFile, string CompressedFile, string TargetFile)
        {
            this.SourceFile = SourceFile;// исходный файл
            this.CompressedFile = CompressedFile;// сжатый файл
            this.TargetFile = TargetFile;// восстановленный файл

            // создание сжатого файла
            Compress(this.SourceFile, this.CompressedFile);
            // чтение из сжатого файла
            Decompress(this.CompressedFile, this.TargetFile);
        }
        public void Compress(string SourceFile, string CompressedFile)
        {
            this.SourceFile = SourceFile;
            this.CompressedFile = CompressedFile;

            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(this.SourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(this.CompressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            this.SourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }
        public void Decompress(string CompressedFile, string TargetFile)
        {
            this.CompressedFile = CompressedFile;
            this.TargetFile = TargetFile;

            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(this.CompressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(this.TargetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", this.TargetFile);
                    }
                }
            }
        }
    }
}
