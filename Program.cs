using BlogMVC.ConfigurationModels;
using BlogMVC.Data;
using BlogMVC.Models;
using BlogMVC.Repositories;
using BlogMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBlogRepository<Blog>, BlogRepository>();
builder.Services.AddScoped<IBlogRepository<Post>, PostRepository>();
builder.Services.AddScoped<IBlogRepository<Comment>, CommendRepository>();
builder.Services.AddScoped<IBlogRepository<Tag>, TagRepository>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailService>();

builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<ImageService>();


builder.Services.AddRazorPages();


var app = builder.Build();

using(var scop = app.Services.CreateScope())
{
    var SeedService = scop.ServiceProvider.GetRequiredService<SeedService>();
    await SeedService.ManageDataAsyn();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "default",
    pattern: "{controller=Blogs}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
