using coinmarketfees.Model;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace coinmarketfees
{
    class Program
    {
        private static HtmlParser htmlParser = new HtmlParser();
        private static HtmlClient htmlClient = new HtmlClient();

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => Run(o));
        }

        private static List<FeeInfo> GetFeesInfo(int exchangeId)
        {
            int page = 1;
            List<FeeInfo> fees = new List<FeeInfo>();
            bool notEmpty;
            do
            {
                var html = htmlClient.GetFeesInfo(exchangeId, page++);
                var partial = htmlParser.GetFeeInfo(html);
                notEmpty = partial.Any();
                fees.AddRange(partial);
                Thread.Sleep(100);
            } while (notEmpty);
            return fees;
        }

        private static void Run(Options options)
        {
            var htmlParser = new HtmlParser();
            var htmlClient = new HtmlClient();
            switch (options.Action.ToLower())
            {
                case "getexchanges":
                    {
                        var exchanges = htmlParser.GetExchanges(htmlClient.GetExchanges()).OrderBy(k => k.Name).ToList();
                        exchanges.ForEach(e => Console.WriteLine($"{e.Name} [{e.Symbol}]"));
                        break;
                    }
                case "getfees":
                    {
                        if (string.IsNullOrEmpty(options.Exchange))
                        {
                            return;
                        }
                        if (!string.IsNullOrEmpty(options.Exchange2))
                        {
                            var e1 = htmlClient.GetExchange(options.Exchange);
                            var e2 = htmlClient.GetExchange(options.Exchange2);
                            var exchange1 = htmlParser.GetExchange(e1);
                            var exchange2 = htmlParser.GetExchange(e2);

                            var fiKraken = GetFeesInfo(exchange1.Id);
                            var fiBinance = GetFeesInfo(exchange2.Id);

                            var common = fiKraken.Where(bc => fiBinance.Any(cc => cc.CoinSymbol == bc.CoinSymbol));
                            common = common.Where(k => k.Price > 0).OrderBy(k => k.Price);
                            foreach (var e in common)
                            {
                                Console.WriteLine($"{e.CoinName} [{e.CoinSymbol}][{e.Price}]");
                            }
                            return;
                        }
                        else
                        {
                            var kraken = htmlClient.GetExchange(options.Exchange);
                            var exchange = htmlParser.GetExchange(kraken);
                            var exchanges = GetFeesInfo(exchange.Id);
                            exchanges.ForEach(e => Console.WriteLine($"{e.CoinName} [{e.CoinSymbol}][{e.Price}]"));
                            break;
                        }
                    }
                default:
                    break;

            }
        }
    }
}
