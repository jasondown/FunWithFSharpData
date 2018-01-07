
#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "..//packages/FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.dll"

open FSharp.Data
open FSharp.Charting

[<Literal>]
let WikiSource = @"https://en.wikipedia.org/wiki/List_of_men%27s_major_championships_winning_golfers"

type Golfer =
    { Name : string
      Wins : int }

type Wiki = HtmlProvider<WikiSource>

let getGolfers (wiki : Wiki) =
    wiki.Tables.``By golfer``.Rows
    |> Seq.map (fun g -> { Name = g.Golfer.Substring((g.Golfer.Length/2)+1); Wins = g.Total })

let getHighTotalWins () = 
    Wiki.Load(WikiSource) 
    |> getGolfers 
    |> Seq.filter (fun g -> g.Wins > 4)

let highTotalWinsChart () = 
    getHighTotalWins ()
    |> Seq.map (fun g -> (g.Name, g.Wins))
    |> Chart.Column
    |> Chart.WithStyling (Color = System.Drawing.Color.Green)
    |> Chart.WithTitle (Text = "Golf Major Winners (5 or More Wins)")
    |> Chart.WithXAxis (Title = "Golfer", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Angle = -45, Interval = 1.0))
    |> Chart.WithYAxis (Title = "Total Major Wins", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Interval = 2.0))

highTotalWinsChart () |> Chart.Show