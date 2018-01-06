// ---------------------------------------------------------------------
// I'm accessing the IEX API, thus must include the following:
// ---------------------------------------------------------------------
//      “Data provided for free by IEX.”
//      Terms of service: https://iextrading.com/api-exhibit-a/
// ---------------------------------------------------------------------

#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data

// This sample data could be provided in a file, but I just copied 
// the Json directly from https://api.iextrading.com/1.0/stock/aapl/quote
[<Literal>]
let sampleQuote = """
{
   "symbol":"AAPL",
   "companyName":"Apple Inc.",
   "primaryExchange":"Nasdaq Global Select",
   "sector":"Technology",
   "calculationPrice":"tops",
   "open":172.61,
   "openTime":1514989800330,
   "close":172.26,
   "closeTime":1514926800008,
   "high":174.55,
   "low":172.16,
   "latestPrice":172.27,
   "latestSource":"IEX real time price",
   "latestTime":"3:54:20 PM",
   "latestUpdate":1515012860328,
   "latestVolume":24955790,
   "iexRealtimePrice":172.27,
   "iexRealtimeSize":100,
   "iexLastUpdated":1515012860328,
   "delayedPrice":172.432,
   "delayedPriceTime":1515011963820,
   "previousClose":172.26,
   "change":0.01,
   "changePercent":0.00006,
   "iexMarketPercent":0.02418,
   "iexVolume":603431,
   "avgTotalVolume":26643368,
   "iexBidPrice":172.27,
   "iexBidSize":100,
   "iexAskPrice":176.71,
   "iexAskSize":100,
   "marketCap":876347137120,
   "peRatio":18.73,
   "week52High":177.2,
   "week52Low":114.76,
   "ytdChange":0
}
"""

type StockQuote = JsonProvider<sampleQuote>

[<Literal>]
let url = "https://api.iextrading.com/1.0/stock"

let getQuoteEndPoint symbol =
    sprintf "%s/%s/quote" url symbol

let getLatestPrice (quote : StockQuote.Root) =
    sprintf "%s - $%.2f" quote.CompanyName quote.LatestPrice
    
let printLatestPrice (quote : StockQuote.Root) =
    printfn "%s" <| getLatestPrice quote

let printLatestPrices (quotes : StockQuote.Root list) =
    quotes
    |> List.sortByDescending (fun q -> q.LatestPrice)
    |> List.iter (fun q -> printLatestPrice q)

let getQuotes (symbols : string list) =
    symbols 
    |> List.map (fun s -> StockQuote.Load(getQuoteEndPoint s))

let getQuotes' (symbols : string list) =
    symbols
    |> List.map (fun s -> StockQuote.AsyncLoad(getQuoteEndPoint s))
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Array.toList

let getTechGiants () = ["aapl";"msft";"goog";"fb";"amzn";"intc";"orcl";"ibm";"dvmt";"ebay";"baba";"hpe";"csco"] |> getQuotes
let getTechGiants' () = ["aapl";"msft";"goog";"fb";"amzn";"intc";"orcl";"ibm";"dvmt";"ebay";"baba";"hpe";"csco"] |> getQuotes'

// Synchronously
#time
for i = 0 to 9 do
    getTechGiants() |> printLatestPrices
#time

// Asynchronously
#time
for i = 0 to 9 do
    getTechGiants'() |> printLatestPrices
#time

