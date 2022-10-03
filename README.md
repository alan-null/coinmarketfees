# coinmarketfees

This is CLI client for [coinmarketfees](https://coinmarketfees.com/)

## Usage

- get a list of exchanges

  `.\coinmarketfees.exe exchanges`

- get a list of coin transfer fees for a single exchange

    `.\coinmarketfees.exe fees -e kraken`

- get a list of coin transfer fees for two exchanges (contains only fees for coins supported by both )

    `.\coinmarketfees.exe fees -e kraken -t kucoin`