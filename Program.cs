using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Entity Framework e a conexão com o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Adicionar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Rota de login
app.MapPost("/login", async (AppDbContext context, [FromBody] LoginDto login) =>
{
    var admin = await context.Administradores.SingleOrDefaultAsync(a => a.Username == login.Username);

    if (admin == null || !BCrypt.Net.BCrypt.Verify(login.Password, admin.PasswordHash))
    {
        return Results.Unauthorized();
    }

    // Gerar JWT (configuração completa precisa ser feita)
    var token = "GeneratedTokenHere";

    return Results.Ok(new { Token = token });
})
    .Produces(200)
    .Produces(401)
    .WithName("Login")
    .WithSummary("Loga um administrador e gera um token JWT.");

// CRUD de veículos
app.MapGet("/veiculos", async (AppDbContext context) =>
{
    return await context.Veiculos.ToListAsync();
})
    .Produces<List<Veiculo>>()
    .WithName("GetVeiculos")
    .WithSummary("Obtém todos os veículos.");

app.MapPost("/veiculos", [Authorize(Roles = "Admin")] async (AppDbContext context, Veiculo veiculo) =>
{
    if (string.IsNullOrEmpty(veiculo.Marca) || string.IsNullOrEmpty(veiculo.Modelo) || veiculo.Ano <= 0)
    {
        return Results.BadRequest("Dados inválidos.");
    }

    context.Veiculos.Add(veiculo);
    await context.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
})
    .Produces<Veiculo>(201)
    .Produces(400)
    .WithName("CreateVeiculo")
    .WithSummary("Cria um novo veículo.");

app.MapPut("/veiculos/{id}", [Authorize(Roles = "Admin")] async (AppDbContext context, int id, Veiculo updatedVeiculo) =>
{
    var veiculo = await context.Veiculos.FindAsync(id);
    if (veiculo == null) return Results.NotFound();

    veiculo.Marca = updatedVeiculo.Marca;
    veiculo.Modelo = updatedVeiculo.Modelo;
    veiculo.Ano = updatedVeiculo.Ano;

    await context.SaveChangesAsync();
    return Results.Ok(veiculo);
})
    .Produces<Veiculo>(200)
    .Produces(404)
    .WithName("UpdateVeiculo")
    .WithSummary("Atualiza um veículo existente.");

app.MapDelete("/veiculos/{id}", [Authorize(Roles = "Admin")] async (AppDbContext context, int id) =>
{
    var veiculo = await context.Veiculos.FindAsync(id);
    if (veiculo == null) return Results.NotFound();

    context.Veiculos.Remove(veiculo);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
    .Produces(204)
    .Produces(404)
    .WithName("DeleteVeiculo")
    .WithSummary("Remove um veículo.");

app.Run();

public record LoginDto(string Username, string Password);

public class Veiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
}
