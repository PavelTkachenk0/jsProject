using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("JsProject"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
            builder =>
            {
                builder.WithOrigins("http://lab1")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowOrigin"); 

app.UseEndpoints(endpoints =>
{  
    endpoints.MapControllers();
});

app.Run();
