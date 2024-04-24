using CantoLiterário___projeto_LivroOnline.Context;
using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Services;
using CantoLiterário___projeto_LivroOnline.Services.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");


//adiciona o contexto para o EF poder funcionar como um serviço interno
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connection));


//adiociona um injeção de dependência nas classes user e role para gerenciar permissões e usuarios, diz qual DB usar 
builder.Services.AddIdentity<User, IdentityRole>()
          .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ICreateImageService, CreateImageService>();
builder.Services.AddScoped<IMapper,Mapper>();

builder.Services.Configure<IdentityOptions>(options =>
{

    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
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


app.MapControllerRoute(
      name: "AdminArea",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


