using ASP.NET_CA_Project.Database;
using ASP.NET_CA_Project.Middlewares;
using ASP.NET_CA_Project.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

// Add DB context
builder.Services.AddDbContext<ShopDBContext>(options => {
    var conn_str = "";

    // check for windows/mac(Unix)
    if (System.Environment.OSVersion.Platform == PlatformID.Unix)
    {
        conn_str = builder.Configuration.GetConnectionString("sql_server_mac");
    }
    else
    {
        conn_str = builder.Configuration.GetConnectionString("sql_server_windows");
    }

    options.UseLazyLoadingProxies().UseSqlServer(conn_str);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.UseMiddleware<SessionTracker>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

InitDB(app.Services);

app.Run();

void InitDB(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    ShopDBContext db = scope.ServiceProvider.GetRequiredService<ShopDBContext>();

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    DBHelper.InjectTestData(db);
}

