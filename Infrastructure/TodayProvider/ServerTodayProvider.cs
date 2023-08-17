namespace QuoteOfTheDay.Infrastructure.TodayProvider;

public class ServerTodayProvider : ITodayProvider
	{
    public DateTime Today => DateTime.Today;
}