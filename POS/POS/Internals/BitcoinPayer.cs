using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.Internals
{
    public static class BitcoinPayer
    {
        public static async Task<bool> CheckPaymentAsync(string addr, double price)
        {
            var t = Task<bool>.Factory.StartNew(() =>
            {
                int times = 0;

                while (true)
                {
                    Thread.Sleep(5000);

                    if (new Info.Blockchain.API.BlockExplorer.BlockExplorer().GetAddress(addr).FinalBalance == price)
                    {
                        return true;
                    }
                    
                    times++;

                    if (times == 10)
                    {
                        return false;
                    }
                }
            });

            return await t;
        }
    }
}