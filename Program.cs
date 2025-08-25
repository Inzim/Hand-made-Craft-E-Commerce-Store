using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CRAFTOPIA.EntityFramwork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
  {
      options.SignIn.RequireConfirmedAccount = true;
  })
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Jwt:Issuer",
            ValidAudience = "Jwt:Issuer",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });
   


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var AllowAnyOrigin = "AllowAnyOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAnyOrigin,
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
/*app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});*/
app.UseStaticFiles();
app.UseRouting();
app.UseCors(AllowAnyOrigin);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
