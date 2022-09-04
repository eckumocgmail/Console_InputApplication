
using System;
using System.Collections.Generic;

using static InputApplicationProgram;


namespace ConsoleApp { 

    using System;
    using System.IO;
    using System.Linq;
    using AngleSharp.Html.Parser; 
    using System.Reflection;
    using System.Collections.Generic;
   
 
    /****************************************************
      1.Разработать на c# консольное приложение,        *
	    которое должно отслеживать появление новых      *
	    текстовых файлов в заданном каталоге.           */
    public class SysProgram 
    {

        #region Program
        /// <summary>
        /// Program Точка входа
        /// </summary>        
        public static void Run(params string[] args)
        {
            Action<string[]> runner =
                (args.Contains("/auto")) ?
                    GetAutomaticRunner() :
                    GetInteractiveRunner();
            runner(args);
        }

        /// <summary>
        /// Перехват сообщения сод сведения об ошибке
        /// </summary>        
        public static void OnErrorMessage(ErrorEventArgs error)
        {
            Warn($"Сообщение: {error.GetException().Message}");
            Warn($"Метод:     {error.GetException().Source}");
            Warn($"Справка:   {error.GetException().HelpLink}");
            Warn($"Трассиовка:");
            //Warn($" {error.GetException().ToDocument().ToJsonOnScreen()}");
        }

        /// <summary>
        /// Возвращает операцию выполнения консоли в интерактивном режиме
        /// (режим взаимодействия с пользователем)
        /// </summary>   
        public static Action<string[]> GetInteractiveRunner()
        {
            return (args) => { 
                Info("\n\tВключен интерактивный режим.");
                if (args.Count() == 0)
                {
                    
                    _ProgramWorkDirectory = ReadLine("\n\tУкажите путь к рабочему каталогу: \n\t(!) Введите '' для использования директории по умолчанию( D:\\System-Config\\ )");
                    if (String.IsNullOrWhiteSpace(_ProgramWorkDirectory))
                    {
                        _ProgramWorkDirectory = GetProgramDirectoryDefault();
                    }
                }
                else
                {
                    _ProgramWorkDirectory = args[0];
                }
                OnUserStartup();
            };
        }

        /// <summary>
        /// Вовзвращает операцию выполнения в режиме заданном через параметр
        /// </summary>
        public static Action<string[]> GetRunner(bool interactive)
        {            
            if (interactive)
            {
                return GetInteractiveRunner();

            }
            else
            {
                return GetAutomaticRunner();
            }
        }


        /// <summary>
        /// Возв. опер. "Выполнение в автоматическом режиме"
        /// </summary>        
        public static Action<string[]> GetAutomaticRunner()
        {
            return (args) =>
            {
                Info(new string[] { "\n\tИнтерактивный режим выключен." });
                if (args.Count() == 0)
                {
                    _ProgramWorkDirectory = ReadLine("Путь к рабочей директории");
                }
                else
                {
                    _ProgramWorkDirectory = args[0];
                }
                OnTerminalStartup();
            };
        }


        /// <summary>
        /// Выводит шаблон, для формирования команды исполнения
        /// </summary>
        public static string GetCommandPattern()
            => $"{Assembly.GetExecutingAssembly().Location} []";


        /// <summary>
        /// Выполнение в режиме автоматизации
        /// </summary>        
        public static void OnTerminalStartup()
        {
            ProgramConfiguration.ReadRules();
            ProgramDirectory.WatchDir(GetProgramWorkDirectory(),
            (sender, evt) => {                 
                AfterChanged(sender, evt);
            },
            (sender, error) =>
            {
                OnErrorMessage(error);
            });
        }

        /// Выполнение при выборе 'Запустить программу'.
        /// В соответвиями с требованиями поставленной задачи 
        /// прогамма должна прослушивать события файловой системы        
        /// </summary>        
        public static void OnProgramWatch() => OnTerminalStartup();

        /// <summary>
        /// Выполнение в режиме взаимодействия с пол. 
        /// выполняет переход консоли к гланому меню,
        /// где пользователь выбирает следующую операцию.        
        /// </summary>
        public static void OnUserStartup()
        {
            NextState("OnUserStartup");    
            
            Info("\n\tВыберите действие: ");
            switch (SingleSelect(new string[] {
                    "запустить программу",
                    "настроить программу",
                    "завершить сеанс"
                }))
            {
                case "запустить программу":
                    Clear();
                    OnProgramWatch();
                    break;
                case "настроить программу":
                    Clear();
                    OnProgramSetup();
                    break;
                case "завершить сеанс":
                    break;
            };
        }         
        #endregion

        #region ProgramConfiguration
        /// <summary>
        /// Выполнение при выборе 'Настроить программу'
        /// </summary>
        public static void OnProgramSetup() => ProgramConfiguration.Run(GetProgramConfigurationFile());
        public static string GetProgramConfigurationFile() => ProgramConfiguration.GetProgramConfigurationFile();
        #endregion

        #region ProgramHistory
        public static void NextState(string state) => ProgramHistory.NextState(state);
        #endregion

        #region ProgramLogger
        public static void Warn(params string[] args) => Warn(args);
        public static void Info(params string[] args) => Info(args);
        public static void Log(params string[] args) => Log(args);
        #endregion

        #region ProgramConfiguration
        public static string _ProgramWorkDirectory { get => ProgramConfiguration._ProgramWorkDirectory; set => ProgramConfiguration._ProgramWorkDirectory = value; }
        public static string GetProgramWorkDirectory() => ProgramConfiguration.GetProgramWorkDirectory();
        public static string GetProgramDirectoryDefault() => ProgramConfiguration.GetProgramDirectoryDefault();
        #endregion

        #region ProgramDialog
        public static string ReadLine(string vs) => ProgramDialog.ReadLine(vs);
        public static string SingleSelect(string[] vs) => ProgramDialog.SingleSelect(vs);
        #endregion

        /// <summary>
        /// True-пробрасывает исключения из операции в процесс
        /// </summary>
        public static bool UseThrowableOperationd = false;

        /// <summary>
        /// True-пробрасывает исключения из из процесса в среду исполнения
        /// </summary>

        public static bool UseThrowableProcess = false;

        /// <summary>
        ///     При проектировании приложения предусмотреть возможность расширения как количества 
        /// операций, выполняемых над имеющимся типом файла, так и количества распознаваемых
        /// типов файлов.
        /// </summary>
        /// <return> состояние файла=кол-во успешно выполненых операций </return>
        /********************************************************
         * При появлении нового файла необходимо: */        
        public static void AfterChanged(object sender, FileSystemEventArgs e)
        {
            string filepath = e.FullPath;
            string type = e.ChangeType.ToString();
            
            Log($"\n\n[{DateTime.Now}][{ProgramDirectory.GetFileSize(filepath)}][{type}][{filepath}]");
            try
            {
                int ctn = 0;
                foreach (var Pattern in ProgramConfiguration.RuleSet.Keys.ToList())
                {
                    if( PathIsMatchPattern(filepath, Pattern) == false )
                    {
                        Log($"Правило [{Pattern}] не применяется к [{filepath}]");
                    }
                    else
                    {
                        Log($"Правило [{Pattern}] применяется к [{filepath}]");
                        foreach (var ToDo in ProgramConfiguration.RuleSet[Pattern])
                        {
                            try
                            {                               
                                var names = ProgramCall.Parse(ToDo);
                                string action = ToDo.Replace(" ", ".");
                                ProgramCall.Call(action, new Dictionary<string, string>() {
                                    { "ChangeType", type },
                                    { "FilePath", filepath },
                                });
                            }
                            catch (Exception ex)
                            {
                                Warn("Исключение при выполнении операции №" + ctn + ex.Message);
                                if (UseThrowableOperationd)
                                    throw new Exception("Исключение при выполнении операции №" + ctn + ex.Message, ex);
                            }
                            finally
                            {
                                ctn++;
                            }
                        }
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (UseThrowableProcess)
                    throw new Exception("Исключение в ходе обработки " + ex.Message, ex);
            }                        
        }


        


        /// <summary>
        /// Проверяем соответвует путь шаблону или нет (2 случая):
        /// 1) *.*
        /// 2) *.[EXT]
        /// </summary>
        public static bool PathIsMatchPattern(string filepath, string pattern)
        {
            if(pattern == "*.HTML" || pattern == "*.CSS")
            {
                string ext = pattern.Substring(pattern.IndexOf('*') + 1);
                var result = filepath.ToUpper().EndsWith(ext);
                return result;
            }
            else if (pattern == "*.*")
            {           
                var result = filepath.ToUpper().EndsWith(".HTML") == false &&
                                filepath.ToUpper().EndsWith(".CSS") == false;
                return result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

       
    }

    
}

 