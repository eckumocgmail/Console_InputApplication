using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


    /// <summary>
    /// Отчет о тестировании
    /// </summary>
    public class TestingReport 
    {
        public string Name { get; set; }
        public bool Failed { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public List<string> Messages { get; set; }

        public Dictionary<string, TestingReport> SubReports { get; set; }


        public TestingReport()
        {
           this.SubReports = new Dictionary<string, TestingReport>();
        }

        /// <summary>
        /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
        /// </summary>
        /// <returns></returns>
        public int GetVersion()
        {            
            return (from r in this.SubReports.Values where r.Failed == false select r).Count();
        }


        /// <summary>
        /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
        /// </summary>
        /// <returns></returns>
        public Version GetRealisticVersion()
        {
            if(this.SubReports.Count == 0)
            {
                return new Version(1, this.Failed ? 0 : 1);
            }                
            return new Version(
                (from r in this.SubReports.Values where r.Failed==false select r).Count(), 
                this.SubReports.Count);
        }

        /// <summary>
        /// Количественный номер версии, показывает кол-во выполненых проверок
        /// </summary>
        /// <returns></returns>
        public Version GetMaximalisticVersion()
        {
            if (this.SubReports.Count == 0)
            {
                return new Version(1, this.Failed ? 0 : 1);
            }
            return new Version((from r in this.SubReports.Values where r.Failed == false select r).Count(), this.SubReports.Count);
        }


        /// <summary>
        /// Метод получчения числовой информации о результатх тестирования 
        /// </summary>
        /// <returns> числовая информация о результатах тестирования </returns>
        public string GetStat()
        {            
            if( this.SubReports.Count() == 0)
            {
                return this.Failed ? "0" : "1";
            }
            else
            {
                int inc = 0;
                foreach (var p in this.SubReports)
                {
                    if (p.Value.Failed)
                    {
                        break;
                    }
                    else
                    {
                        inc++;
                    }
                }
                return $"{this.SubReports.Count}-{inc}";
            }            
        }


        /// <summary>
        /// Составление текстового документа, содержащего информацию о результатах тестирования
        /// </summary>
        /// <param name="isTopReport"> true, если отчет составлен на верхнем уровне </param>
        /// <returns> теккстовый документ </returns>
        public string ToDocument(int level=0)
        {
            string document = "";
            foreach (string message in Messages)
            {
                for(int i=0; i<=level; i++)
                {
                    document += "    ";
                }
                document += message + "\n";
            }
            int number = 1;
            foreach( var pair in this.SubReports)
            {
                for (int i = 0; i <= level; i++)
                {
                    document += "    ";
                }
                
                document += //$"{GetRealisticVersion().ToString()}: "+pair.Key + "\n";
                    $"{number}/{this.SubReports.Count()}: [{pair.Key}]" + pair.Value.Name + "\n";
                document += pair.Value.ToDocument(level+1);
                number++;
            }
            return document;
        }


        /// <summary>
        /// Преобразование в текстовый формат
        /// </summary>
        /// <returns> текстовые данные </returns>
        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
