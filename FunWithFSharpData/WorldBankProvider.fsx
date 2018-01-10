#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "..//packages/FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.dll"

open FSharp.Data
open FSharp.Charting

let data = WorldBankData.GetDataContext()

let countries =
    [| data.Countries.Canada
       data.Countries.``United States``
       data.Countries.China
       data.Countries.India |]

[ for c in countries -> 
    c.Indicators.``Population, female`` ]
|> Async.Parallel
|> Async.RunSynchronously
|> Array.map Chart.Line
|> Chart.Combine