using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Repository;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserTypeRepository, UserTypeRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

StripeConfiguration.ApiKey = "sk_test_51NB50cFwFW4jFuKbVOkPKbBM4kvIcFl2oW3srq8YQ0PyfyBtemClyvnIzqDk2Sf0yFojWEI7WEngHzO8iKyamSWX00Jw1eZcks";
builder.Services.AddCors();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>()
//        .AddDefaultTokenProviders();


//var jwtSecretKey = "TajniKljucZaGenerisanjeJWTTokena"; // Možete generisati svoj tajni ključ

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireClaim("usertype_id", "1"));
    options.AddPolicy("Kupac", policy => policy.RequireClaim("usertype_id", "2"));
});

//builder.Services.AddScoped<IClaimsTransformation, UserTypeClaimsProvider>();



builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200")
           .WithOrigins("http://127.0.0.1:4200")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
