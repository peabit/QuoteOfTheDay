using System;

namespace QuoteOfTheDay.Domain.QuoteOfTheDayService;

public interface IQuoteOfTheDayService
{
    Task<Quote> GetAsync();
}