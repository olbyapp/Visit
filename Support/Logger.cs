using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Visit.Models;

namespace Visit.Support
{
    internal class Logger : Interfaces.ILogger
    {
        private readonly ObservableCollection<LogObject> LogQueue;
        bool InWork = false;
        private readonly char separator;
        private string folder;


        public Logger()
        {
            separator = Path.DirectorySeparatorChar;
            folder = Path.Combine(Directory.GetCurrentDirectory() + separator + "Logs");

            LogQueue = new ObservableCollection<LogObject>();
            LogQueue.CollectionChanged += LogQueue_CollectionChanged;
        }


        private void LogQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!InWork)
            {
                InWork = true;
                QueueWorker();
            }
        }



        /// <summary>
        /// Статическая функция лога. По дефолту, пишет данные по пути %appdata%\Monitoring\Monitoring.log
        /// Caret  - добавляет указанное количество переносов после записи основного значения.
        /// </summary>
        public void Log(string Func, string Str, string Type = "Info", string FIleLog = "full.log", short Caret = 1, bool IgnoreFull = false)
        {

            ChekFileSize(FIleLog);
            if (FIleLog != "full.log")
            {
                if (!IgnoreFull)
                    AddQueueLog(Func, Str, Type, "full.log", Caret);
                AddQueueLog(Func, Str, Type, FIleLog, Caret);
            }
            else
                AddQueueLog(Func, Str, Type, FIleLog, Caret);

            switch (Type.ToLower())
            {
                case "error":
                case "fatal":
                    AddQueueLog(Func, Str, Type, "error.log", Caret);
                    if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                    {
                        //using (SentrySdk.Init("https://213a054ad49b458e9ab50d65df8befc8@o517649.ingest.sentry.io/5625914"))
                        //{
                        //    SentrySdk.CaptureMessage(Str, SentryLevel.Error);
                        //}
                    }
                    break;
            }
        }
        public void Log(string Func, Exception ex, string Type = "error", string FIleLog = "full.log", short Caret = 1)
        {

            ChekFileSize(FIleLog);
            ChekFileSize("error.log");
            AddQueueLog(Func, "Error: " + ex.Message + "\n" + ex.StackTrace, Type, "full.log", Caret);
            AddQueueLog(Func, "Error: " + ex.Message + "\n" + ex.StackTrace, Type, "error.log", Caret);

            if (Type.ToLower() != "warn" && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                //using (SentrySdk.Init("https://213a054ad49b458e9ab50d65df8befc8@o517649.ingest.sentry.io/5625914"))
                //{
                //    SentrySdk.CaptureException(ex);
                //}
            }
        }



        /// <summary>
        /// Статическая функция, которая бэкапит файл логов, если он достуг указанного размера.
        /// </summary>
        private void ChekFileSize(string FIleLog, long logSize = 30000000) //больше 30 мб
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Logs" + separator + FIleLog);
            if (File.Exists(path))
            {
                long si = new FileInfo(path).Length;
                if (si > logSize)
                {
                    File.Delete(path);
                }
            }
        }
        /// <summary>
        /// Статическая функция, которая пишел лог в указаный файл лога.
        /// </summary>
        private void WriteLog(string Func, string Str, string Type, string FIleLog, int Caret = 1)
        {
            string path = Path.Combine(folder + separator + FIleLog);
            Directory.CreateDirectory(folder);
            try
            {
                StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append), Encoding.UTF8);  //Create a file to write to.
                if (Str == "LET`s GO MUTHERFUCKERs")
                {
                    sw.WriteLine("\n\n************************STARTED************************");
                    Console.WriteLine("\n\n************************STARTED************************");
                }
                string UniversalDT = DateTime.Now.AddHours(3).ToString("dd/MM/yyyy HH:mm:ss");

                sw.WriteLine("{0} - [{2}] {1} : {3}", UniversalDT, Func, Type, Str);


                if (FIleLog == "full.log")
                    Console.WriteLine("{0} - [{2}] {1} : {3}", UniversalDT, Func, Type, Str);

                if (Caret > 1)   //Формирует переносы, в случае если значение переменной больше 1.
                    while (Caret != 0)
                    {
                        sw.WriteLine("");

                        if (FIleLog == "full.log")
                            Console.WriteLine(" ");
                        Caret--;
                    }

                sw.Close();


            }
            catch (Exception ex)
            {
                //string UniversalDT = DateTime.Now.AddHours(3).ToString("dd/MM/yyyy HH:mm:ss");
                //Console.WriteLine("{0} - [Logger.Fatal] : {2} \n {3}", UniversalDT,Type, ex.Message,ex.StackTrace);
            }
        }
        private void WriteLog(LogObject log)
        {
            string path = Path.Combine(folder + separator + log.FileLog);
            Directory.CreateDirectory(folder);
            try
            {
                StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append), Encoding.UTF8);  //Create a file to write to.
                if (log.Msg == "LET`s GO MUTHERFUCKERs")
                {
                    sw.WriteLine("\n\n************************STARTED************************");
                    Console.WriteLine("\n\n************************STARTED************************");
                }
                string UniversalDT = log.Date.ToString("dd/MM/yyyy HH:mm:ss");

                sw.WriteLine("{0} - [{2}] {1} : {3}", UniversalDT, log.PointFunc, log.Type, log.Msg);

                if (log.FileLog == "full.log")
                    Console.WriteLine("{0} - [{2}] {1} : {3}", UniversalDT, log.PointFunc, log.Type, log.Msg);

                if (log.Caret > 1)   //Формирует переносы, в случае если значение переменной больше 1.
                    while (log.Caret != 0)
                    {
                        sw.WriteLine("");

                        if (log.FileLog == "full.log")
                            Console.WriteLine(" ");
                        log.Caret--;
                    }

                sw.Close();


            }
            catch (Exception ex)
            {
                //string UniversalDT = DateTime.Now.AddHours(3).ToString("dd/MM/yyyy HH:mm:ss");
                //Console.WriteLine("{0} - [Logger.Fatal] : {2} \n {3}", UniversalDT,Type, ex.Message,ex.StackTrace);
            }
        }
        private void AddQueueLog(string Func, string Str, string Type, string FIleLog, short Caret = 1)
        {
            LogQueue.Add(new LogObject()
            {
                Date = DateTime.Now.AddHours(3),
                PointFunc = Func,
                Caret = Caret,
                FileLog = FIleLog,
                Msg = Str,
                Type = Type
            });
        }
        private void QueueWorker()
        {
            while (LogQueue.Count != 0)
            {
                try
                {
                    var log = LogQueue.FirstOrDefault();
                    if (log == default)
                        throw new Exception();

                    WriteLog(log);

                    LogQueue.Remove(log);
                }
                catch (Exception ex)
                {
                    if (LogQueue.Count != 0)
                        LogQueue.Remove(LogQueue.FirstOrDefault());
                }

            }
            InWork = false;
        }

    }
}
