using System.IO.Compression;
using System.Text;
using LibraryMinimalAPI.Context;
using LibraryMinimalAPI.Endpoints;
using LibraryMinimalAPI.Service;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options => {

    options.EnableForHttps = true;
    options.Providers.Clear();
    options.Providers.Add<BrotliCompressionProvider>();
    
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => {

    options.Level = CompressionLevel.Optimal;

});

// adding extra json file.
builder.Configuration.AddJsonFile("appsettings.Local.json", true, true);

//jwt bearer install
builder.Services.AddAuthentication().AddJwtBearer(options => {

    options.TokenValidationParameters = new() {

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Ozgun Munar",
        ValidAudience = "Ozgun Munar",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Dummy secret keyDummy secret keyDummy secreDummy secret keyDummy secret keyt keyDummy secret key"))

    };

});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    
    options.UseInMemoryDatabase("MyDb");

});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseResponseCompression();

app.UseBookEndpoints();

app.Run();