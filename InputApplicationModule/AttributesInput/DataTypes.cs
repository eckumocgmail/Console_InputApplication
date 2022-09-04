using System.Collections.Generic;

public class DataTypes
{
    public static Dictionary<string, string> DATATYPERS = new Dictionary<string, string>() {
        {"int",         "Целое число"},
        {"float",       "Вещественное число"},
        {"date",        "Дата"},
        {"datetime",    "Дата время"},
        {"varchar",     "Текстовый"},
        {"varbinary",   "Бинарный"}
    };

    
    public static string InputTypesSelectControlAttribute
    {
        get
        {
            string result = "";
            foreach(var key in INPUTTYPES.Keys)
            {
                result += $",{key}";
            }
            return result.Substring(1);
        }
    }
    public static Dictionary<string, string> INPUTTYPES = new Dictionary<string, string>() {
        {"number",      "Числа"},
        {"text",        "Текст"},
        {"password",    "Пароль"},
        {"email",       "Электронная почта"},
        {"url",         "URL"},
        {"file",        "Файл"},
        {"color",       "Цвет"},
        {"image",       "Изображение"},
        {"icon",        "Иконка"},
    };



    
}