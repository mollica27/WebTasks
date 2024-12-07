using Microsoft.EntityFrameworkCore;
using WebTasks.Data;
using System.Globalization;

var defaultCulture = new CultureInfo("en-US");
defaultCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-ddTHH:mm:ss";
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<WebTasksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebTasksContext")
        ?? throw new InvalidOperationException("Connection string 'WebTasksContext' not found.")));



builder.Services.AddControllersWithViews();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles(); 

app.UseRouting(); 

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 


app.MapControllerRoute(
    name: "modals",
    pattern: "modals/{controller}/{action}/{id?}"); 

app.Run();
