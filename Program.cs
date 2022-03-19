using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using TMonitBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTConfig"));
string connectionString = builder.Configuration.GetConnectionString("SqlServerTMonit");
builder.Services.AddDbContext<DatabaseContext>(
    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:Secret"]);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, //这将使用我们在 appsettings 中添加的 secret 来验证 JWT token 的第三部分，并验证 JWT token 是由我们生成的
        IssuerSigningKey = new SymmetricSecurityKey(key), //将密钥添加到我们的 JWT 加密算法中
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = false
    };
});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DatabaseContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbctx = scope.ServiceProvider.GetService<DatabaseContext>();
    await dbctx.Database.MigrateAsync();
}

// Configure the HTTP request pipeline on dev.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
