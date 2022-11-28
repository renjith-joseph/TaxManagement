using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Serialization;
using TaxManagement.Models;
using TaxManagement.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
//Enable CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//JSON Serializer
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
    .Json.ReferenceLoopHandling.Ignore)
   .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
   = new DefaultContractResolver());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("BasicAuthentication").
            AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
            ("BasicAuthentication", null);


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaxRepository, TaxRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();
app.UseHttpsRedirection();




app.MapControllers();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Tax}/{action=Index}/{id?}");


app.Run();
