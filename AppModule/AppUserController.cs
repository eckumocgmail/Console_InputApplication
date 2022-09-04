using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public static class AppUserController
{
    public static async Task SingleSelect( this HttpContext http, string[] options )
    {
            
        await http.Response.WriteAsJsonAsync(@" <ul class=""list-group"">");   
        foreach(var option in options)         
            await http.Response.WriteAsJsonAsync($@" <li class=""list-group-item"">{option}</li>");                 
        await http.Response.WriteAsJsonAsync(@"</ul> ");
        await Task.CompletedTask;
    }
}