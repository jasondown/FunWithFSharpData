#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "..//packages/FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.dll"

open FSharp.Data
open FSharp.Charting

type WorldBank = WorldBankDataProvider<"World Development Indicators", Asynchronous=true>

let data = WorldBank.GetDataContext()

let canPops = 
    [| data.Countries.Canada.Indicators.``Population, total``
       data.Countries.Canada.Indicators.``Population, female``
       data.Countries.Canada.Indicators.``Population, male`` |]

let usPops =
    [| data.Countries.``United States``.Indicators.``Population, total``
       data.Countries.``United States``.Indicators.``Population, female``
       data.Countries.``United States``.Indicators.``Population, male`` |]

let ukPops =
    [| data.Countries.``United Kingdom``.Indicators.``Population, total``
       data.Countries.``United Kingdom``.Indicators.``Population, female``
       data.Countries.``United Kingdom``.Indicators.``Population, male`` |]

let getPopsChart (country : string) (populationInterval : float) (pops : Async<Runtime.WorldBank.Indicator> array) =
    [| for p in pops -> p |]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Array.map (fun p -> Chart.Column(p, Name = sprintf "%s (%s)" p.Name country ))
    |> Chart.Combine
    |> Chart.WithXAxis (Title = "Year", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Angle = -45, Interval = 5.0))
    |> Chart.WithYAxis (Title = "Population", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Interval = populationInterval, Format = "N0"))
    |> Chart.With3D()
    |> Chart.WithTitle (sprintf "Population per year - %s" country, InsideArea = false)
    |> Chart.WithLegend(Title = "Legend", Docking = ChartTypes.Docking.Right, InsideArea = false)

// Test it out
[ (canPops  |> getPopsChart "CAN" 5_000_000.0)
  (usPops   |> getPopsChart "US" 50_000_000.0)
  (ukPops   |> getPopsChart "UK" 10_000_000.0) ] 
|> Chart.Rows
|> Chart.Show