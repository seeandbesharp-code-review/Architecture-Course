using ChineseRaffleApi.Controllers;
using ChineseRaffleApi.Controllers.DI;
using ChineseRaffleApi.Data;
using ChineseRaffleApi.Repository;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services;
using ChineseRaffleApi.Services.DI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<MyContext>(options =>
//        options.UseSqlServer("Server=localhost;Database=ChineseRaffle;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True"));
builder.Services.AddDbContext<MyContext>(options =>
        options.UseSqlServer("Server=srv2\\pupils;Database=ChineseRaffle_216197806;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True"));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserController,UserController>();
builder.Services.AddScoped<IDonorRepo,DonorRepo>();
builder.Services.AddScoped<IDonorService,DonorService>();
//builder.Services.AddScoped<IDonorController,DonorController>();
builder.Services.AddScoped<IGiftRepo,GiftRepo>();
builder.Services.AddScoped<IGiftService,GiftService>();
builder.Services.AddScoped<ITicketRepo, TicketRepo>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketController, TicketController>();
builder.Services.AddScoped<IBasketRepo, BasketRepo>();
builder.Services.AddScoped<IBasketService, BasketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
