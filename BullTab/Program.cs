using BullTab.DbData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BullTab
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            List<Ticker> symbols = new List<Ticker>();
            symbols = Symbols.GetSymbols();
            Analyze.AnalyzeDailyOHLC(symbols).Wait();
        }
    }
}
