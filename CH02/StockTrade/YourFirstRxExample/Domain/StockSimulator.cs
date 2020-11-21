using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstRxExample
{
    /// <summary>
    /// This simulator will emit a batch of StockTicks every two seconds.
    /// each time, a single item will be selected and updated with a "drastic change" (more than 10%)
    /// </summary>
    public  class StockSimulator
    {
        private readonly StockTicker stockTicker;
        private IEnumerable<StockTick> ticks;
        private int itemToDrasticUpdate = 0;

        public StockSimulator(StockTicker stockTicker)
        {
            this.stockTicker = stockTicker;
            ticks = new[]
            {
                new StockTick() {QuoteSymbol = "MSFT", Price = 53.49M},
                new StockTick() {QuoteSymbol = "INTC", Price = 32.68M},
                new StockTick() {QuoteSymbol = "ORCL", Price = 41.48M},
                new StockTick() {QuoteSymbol = "CSCO", Price = 28.33M},
            };
        }

        public async Task Run()
        {
            Console.WriteLine($"SIM entry Thread Nr: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            while (true)
            {
                UpdatePrices();
                PrintPrices();
                await Emit();
                await Task.Delay(2000);
            }
        }

        private async Task Emit()
        {
            Console.WriteLine($"Emit: entry Thread Nr: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Emitting...");
            foreach (var stockTick in ticks)
            {
                await stockTicker.NotifyAsync(stockTick);
            }
        }

        private void PrintPrices()
        {
            Console.WriteLine($"PrintPrices: entry Thread Nr: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Next series to emit:");
            Console.WriteLine("\t");
            foreach (var stockTick in ticks)
            {
                Console.WriteLine("{{{0} - {1}}}, ", stockTick.QuoteSymbol, stockTick.Price);
            }
            Console.WriteLine();

        }

        private void UpdatePrices()
        {
            Console.WriteLine($"UpdatePrices: entry Thread Nr: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            ticks = ticks.Select((tick, i) =>
            {
                var changePercentage = itemToDrasticUpdate == i ? 1.2M : 1.1M;
                return new StockTick() { Price = tick.Price * changePercentage, QuoteSymbol = tick.QuoteSymbol };
            }).ToList();

            itemToDrasticUpdate++;
            itemToDrasticUpdate %= ticks.Count();
        }
    }
}