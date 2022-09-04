using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class InputTypes: HashSet<string>
{
    
    public static string PrimitiveCollection = "PrimitiveCollection";
    public static string StructureCollection = "StructureCollection";
    public static string Percent = "Percent";
    public static string Number = "Number";
    public static string Custom = "Custom";
    public static string Date = "Date";
    public static string DateTime = "DateTime";
    public static string Time = "Time";
    public static string Duration = "Duration";
    public static string Xml = "Xml";
    public static string Image = "Image";
    public static string PostalCode = "PostalCode";
    public static string CreditCard = "CreditCard";
    public static string Currency = "Currency";
    public static string Icon = "Icon";
    public static string Color = "Color";
    public static string Email = "Email";
    public static string File = "File";
    public static string Month = "Month";
    public static string Password = "Password";    
    public static string Phone = "Phone";
    public static string Url = "Url";
    public static string Week = "Week";
    public static string Year = "Year";
    public static string Text = "Text";
    public static string MultilineText = "MultilineText";


    public IEnumerable<string> GetAll()
        => new List<string>()
        {
            Text,MultilineText,Year,Week,Month,Url,Phone,Password,File,Email,Color,Icon,Currency,
            PostalCode,Image,Xml,Duration,Time,DateTime,Custom,Number,Percent, StructureCollection,PrimitiveCollection
        };




}