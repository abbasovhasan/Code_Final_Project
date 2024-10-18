using LoginAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// FluentValidation'ı Controllers ile birlikte ekleyelim
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterDtoValidator>()) // FluentValidation
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // JSON serileştirme ayarları
    });

// Swagger/OpenAPI desteği ekleyin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext'i yapılandırın (SQL Server ile)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper'ı ekleyelim
builder.Services.AddAutoMapper(typeof(MappingProfile)); // MappingProfile'ı ekliyoruz

// JWT Authentication yapılandırması
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // JWT Bearer seçeneklerini yapılandırın
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Issuer'ı doğrula
        ValidateAudience = true, // Audience'ı doğrula
        ValidateLifetime = true, // Token süresini doğrula
        ValidateIssuerSigningKey = true, // İmza anahtarını doğrula
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Geçerli Issuer
        ValidAudience = builder.Configuration["Jwt:Audience"], // Geçerli Audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // İmza anahtarı
    };
});

// Uygulama servislerini kaydedin
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic repository kullanımı
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Yetkilendirmeyi ekleyin
builder.Services.AddAuthorization();

var app = builder.Build();

// HTTP istek hattını yapılandırın

// Geliştirme ortamındaysanız Swagger'ı etkinleştirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmesi (Opsiyonel)
app.UseHttpsRedirection();

// CORS politikası (Opsiyonel, Cross-Origin istekler için)
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication(); // Kimlik doğrulamayı kullan
app.UseAuthorization(); // Yetkilendirmeyi kullan

app.MapControllers(); // Controller'ları eşle

app.Run(); // Uygulamayı çalıştır