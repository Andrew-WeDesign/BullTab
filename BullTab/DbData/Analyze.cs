using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullTab.DbData
{
    public class Analyze
    {
        public static async Task AnalyzeDailyOHLC(List<Ticker> symbols)
        {
            DateTime dYesterday;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
            {
                dYesterday = DateTime.Today.AddDays(-1);
            }
            else if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                dYesterday = DateTime.Today.AddDays(-2);
            }
            else if (DateTime.Now > DateTime.Today.AddHours(20))
            {
                dYesterday = DateTime.Today;
            }
            else
            {
                dYesterday = DateTime.Today.AddDays(-1);
            }

            using (var context = new BullContext())
            {
                foreach (Ticker symbol in symbols)
                {
                    DailyOHLC dbCheck = context.DailyOHLCs
                        .Where(x => x.UnqCode == $"{symbol.Symbol}{dYesterday}")
                        .FirstOrDefault();
                    if (dbCheck == null) //$"{symbol.Symbol}{dYesterday}"
                    {
                        Console.WriteLine($"Unable to find data to analyze symbol {symbol.Symbol}.");
                    }
                    else
                    {
                        if (dbCheck.Open > dbCheck.SMA9Daily && dbCheck.Close > dbCheck.SMA9Daily)
                        {
                            BullList bList = new BullList();

                            bList.Symbol = dbCheck.Symbol;
                            bList.Reason = $"Reason: \nUptrend on SMA9Daily, entire candle closed above the SMA";
                            bList.DateAdded = DateTime.Today;

                            context.BullLists.Add(bList);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
