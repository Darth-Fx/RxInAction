using System;
using System.Threading.Tasks;

namespace FirstRxExample
{
    public class StockTicker : IStockTicker
    {
        public event EventHandler<StockTick> StockTick = delegate {};
        
        public void Notify(StockTick tick)
        {
            StockTick(this, tick);
        }

        public async Task NotifyAsync(StockTick tick)
        {
            await Task.Run(() => StockTick(this, tick));
        }
    }

    public class MyHandlerInfo
    {
    }
}