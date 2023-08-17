using QuoteOfTheDay.Domain.QuoteOfTheDayService;
using QuoteOfTheDay.Infrastructure.QuoteOfTheDayService;
using QuoteOfTheDay.Infrastructure.TodayProvider;

var dt = DateTime.Today;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITodayProvider, ServerTodayProvider>();
builder.Services.AddSingleton<IQuoteOfTheDayService, QuoteOfTheDayService>();
builder.Services.Decorate<IQuoteOfTheDayService, CacheDecorator>();

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = "localhost";
    opts.InstanceName = "local";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/quote", async (IQuoteOfTheDayService quoteService) =>
{
    var quote = await quoteService.GetAsync();
    return Results.Ok(quote);
});

app.Run();