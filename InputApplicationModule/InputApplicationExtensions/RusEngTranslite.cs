public class RusEngTranslite
{
    public static string TransliteToLatine(string rus)
    {
        if( rus == null)
        {
            return null;
        }
        string latine = "";
        for(int i=0; i<rus.Length; i++)
        {
            latine += map(rus[i]);
        }
        return latine;
    }

    private static string map(char v)
    { 
        switch ((v+"").ToLower())
        {
            case "а": return "a";
            case "б": return "b";
            case "в": return "v";
            case "г": return "g";
            case "д": return "d";
            case "е": return "e";
            case "ё": return "yo";
            case "ж": return "j";
            case "з": return "z";
            case "и": return "i";
            case "й": return "yi";
            case "к": return "k";
            case "л": return "l";
            case "м": return "m";
            case "н": return "n";
            case "о": return "o";
            case "п": return "p";
            case "р": return "r";
            case "с": return "s";
            case "т": return "t";
            case "у": return "u";
            case "ф": return "f";
            case "х": return "h";
            case "ц": return "c";
            case "ш": return "sh";
            case "щ": return "sh";
            case "ъ": return "'";
            case "ы": return "u";
            case "ь": return "'";
            case "э": return "a";
            case "ю": return "y";
            case "я": return "ya";
            default: return v+"";
        }
    }
}