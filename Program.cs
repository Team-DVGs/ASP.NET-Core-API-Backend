using Do_an_mon_hoc.Models;
using Do_an_mon_hoc.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;
using System.Text.Unicode;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, 
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000"
                                              ).AllowAnyHeader()
                                               .AllowAnyMethod();
                      });
});
// DB Configuration
// Add services to the container.
builder.Services.AddControllersWithViews();

// DB Configuration
builder.Services.AddDbContext<MiniMarketContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("local"));
    

});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

//AutoMapper Config
builder.Services.AddAutoMapper(typeof(AutoMapperConfigProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // You can add other log providers as needed
});


builder.Services.AddCoreAdmin();
/*
Host.CreaeDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseUrls("http://192.168.98.10:5020", "http://192.168.98.10:5020");
        });
*/


var app = builder.Build();

app.MapDefaultControllerRoute();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCoreAdminCustomTitle("Quản lý cửa hàng Minimarket");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*
app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=Products}/{action=GetProducts}/{id?}"); // Adjust the controller and action names
*/
app.Run();
