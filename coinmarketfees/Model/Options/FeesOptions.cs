using CommandLine;

namespace coinmarketfees.Model.Options
{
    [Verb("fees", HelpText = "Get a list of coin transfer fees for a given exchange.")]
    public class FeesOptions
    {
        [Option('e', "Exchange", Required = true, HelpText = "Name of the (source) exchange.")]
        public string ExchangeSrc { get; set; }

        [Option('t', "TargetExchange", Required = false, HelpText = "Name of the (target) exchange ")]
        public string ExchangeDst { get; set; }
    }
}
