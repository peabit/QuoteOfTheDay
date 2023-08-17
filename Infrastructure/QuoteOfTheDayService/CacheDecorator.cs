using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using QuoteOfTheDay.Domain.QuoteOfTheDayService;
using QuoteOfTheDay.Infrastructure.TodayProvider;

namespace QuoteOfTheDay.Infrastructure.QuoteOfTheDayService;

public class CacheDecorator : IQuoteOfTheDayService
{
    private readonly IDistributedCache _cache;
    private readonly IQuoteOfTheDayService _decorated;
    private readonly ITodayProvider _todayProvider;

    public CacheDecorator(IDistributedCache cache, IQuoteOfTheDayService decorated, ITodayProvider todayProvider)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _todayProvider = todayProvider ?? throw new ArgumentNullException(nameof(todayProvider));
    }

    public async Task<Domain.QuoteOfTheDayService.Quote> GetAsync()
    {
        Domain.QuoteOfTheDayService.Quote? quote = null;
        var quoteJson = await _cache.GetStringAsync("QuoteOfTheDay");

        if (quoteJson is null)
        {
            quote = await _decorated.GetAsync();
            quoteJson = JsonSerializer.Serialize(quote);

            var expirationDate = _todayProvider.Today.AddDays(1);

            await _cache.SetStringAsync(
                key: "QuoteOfTheDay",
                value: quoteJson,
                options: new DistributedCacheEntryOptions() { AbsoluteExpiration = expirationDate }
            );
        }
        else
        {
            quote = JsonSerializer.Deserialize<Domain.QuoteOfTheDayService.Quote>(quoteJson);
        }

        return quote!;
    }
}