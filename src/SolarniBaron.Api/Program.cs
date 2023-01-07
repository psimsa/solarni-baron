using SolarniBaron.Api.Models;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomain();

builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.MapControllers();
app.MapGet("/api/ote/{date}",
        async (DateOnly date, IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> query) =>
        {
            var result = await query.Get(new GetPricelistQuery(date));
            if (result.Status == ResponseStatus.Error)
            {
                return Results.NotFound();
            }

            var response = new ApiResponse<GetPricelistQueryResponse>(result, result.Status, "");
            return Results.Ok(response);
        })
    .WithOpenApi()
    ;

app.Run();
