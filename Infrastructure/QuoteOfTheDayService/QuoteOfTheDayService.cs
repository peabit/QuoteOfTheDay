using Grpc.Net.Client;
using QuoteOfTheDay.Domain.QuoteOfTheDayService;


namespace QuoteOfTheDay.Infrastructure.QuoteOfTheDayService;

public class QuoteOfTheDayService : IQuoteOfTheDayService
{
    private readonly RandomQuoteProvider.RandomQuoteProviderClient _client;

    public QuoteOfTheDayService()
    {
        _client = new RandomQuoteProvider.RandomQuoteProviderClient(
            GrpcChannel.ForAddress("http://localhost:50051")
        );
    }


    public async Task<Domain.QuoteOfTheDayService.Quote> GetAsync()
    {
        var quote = await _client.GetAsync(new Empty());
        return new Domain.QuoteOfTheDayService.Quote(quote.Text, quote.Author);
    }
}