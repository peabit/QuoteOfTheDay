namespace QuoteOfTheDay.Infrastructure.TodayProvider;

public interface ITodayProvider
{
    DateTime Today { get; }
}