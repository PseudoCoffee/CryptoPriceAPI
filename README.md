# CryptoPriceAPI
A small API to fetch and cache prices of cryptocurrency.

API checks first checks if the price is not already cached into a PostgreSQL database hosted in a different container.

Websites currently used to fetch prices
- Bitfinex (example: https://api-pub.bitfinex.com/v2/candles/trade:1h:tBTCUSD/hist?start=1672531200000&end=1672534800000&limit=1)
- Bitstamp (example: https://www.bitstamp.net/api/v2/ohlc/btcusd/?step=3600&limit=1&start=1672531200)

## How to run it
1. Clone the repo.
2. Make sure you have docker installed.
3. In the root level of the repo run `docker-compose up`.

## How it works
1. Call to this API.
2. Check if the price is already cached in database.
3. If it is, return it.
4. Else send request to external API to fetch price.
5. Take the reply and obtain a price.
6. Return the price.
