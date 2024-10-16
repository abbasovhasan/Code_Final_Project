using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>()) // FluentValidation
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // JSON serileştirme ayarları
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories to DI
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

// Add Services to DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add CORS (optional, if required for cross-domain requests)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS if added
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

// Global exception handler middleware (optional)
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) => {
    return Results.Problem("An error occurred.");
});

app.MapControllers();

app.Run();
