using CommandLine;

namespace coinmarketfees.Model
{
    public class Options
    {
        [Option('a', "Action", Required = true, HelpText = "API key for Toggl service.")]
        public string Action { get; set; }

        [Option('e', "Exchange", Required = false, HelpText = "API key for Toggl service.")]
        public string Exchange { get; set; }

        [Option('t', "TargetExchange", Required = false, HelpText = "API key for Toggl service.")]
        public string Exchange2 { get; set; }
    }
}
