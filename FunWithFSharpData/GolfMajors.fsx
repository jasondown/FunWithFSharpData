#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "..//packages/FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.dll"

open FSharp.Data
open FSharp.Charting
open System
open System.Drawing

[<Literal>]
let WikiSource = @"https://en.wikipedia.org/wiki/List_of_men%27s_major_championships_winning_golfers"

type Golfer =
    { Name : string
      Wins : int
      WinningSpan : int * int 
      WinningSpanLength : int }

type Wiki = HtmlProvider<WikiSource>

let parseSpan (span : string) =
    let years = span.Split '-'
    match years with
    | [|y1; y2|] -> (int(y1), int(y2))
    | [|y|] -> (int(y), int(y))
    | _ -> failwith "Bad span"

let getLength (span : int * int) =
    snd span - fst span

let containsNumber (text : string) =
    text |> Seq.exists (fun c -> Char.IsDigit c)

let parseName (golfer : string) =
    // HTML contains a combination of a sort key and title
    // e.g. Woods, TigerTiger Woods
    // Also need to handle a special with JR and SR (still to do)
    // e.g. Morris, Tom 1Tom Morris Sr.
    //      Morris, Tom 2Tom Morris Jr.
    // TODO: Probably have to deal with Names Like David Morland IV and Davis Love III
    match golfer |> containsNumber with
    | true -> golfer.Substring(golfer.Length/2)
    | false -> golfer.Substring((golfer.Length/2) + 1)

let getGolfers (wiki : Wiki) =
    wiki.Tables.``By golfer``.Rows
    |> Seq.map (fun g -> let span = g.``Winning span`` |> parseSpan
                         { Name = g.Golfer |> parseName
                           Wins = g.Total 
                           WinningSpan = span
                           WinningSpanLength = span |> getLength })

let getTotalWins (min : int) = 
    Wiki.Load(WikiSource) 
    |> getGolfers 
    |> Seq.filter (fun g -> g.Wins >= min)

let getTotalSpan (min : int) =
    Wiki.Load(WikiSource)
    |> getGolfers
    |> Seq.filter (fun g -> g.WinningSpanLength >= min)

let totalWinsChart (min : int) = 
    getTotalWins min
    |> Seq.map (fun g -> (g.Name, g.Wins))
    |> Chart.Column
    |> Chart.WithDataPointLabels (Label = "#VAL")
    |> Chart.WithStyling (Color = Color.Green)
    |> Chart.WithTitle (Text = sprintf "Golf Major Winners (%i or More Wins)" min)
    |> Chart.WithXAxis (Title = "Golfer", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Angle = -45, Interval = 1.0))
    |> Chart.WithYAxis (Title = "Total Major Wins", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Interval = 2.0))

let winSpanRangeChart (min : int) =
    getTotalSpan min
    |> Seq.sortBy (fun g -> fst g.WinningSpan)
    |> Seq.map (fun g -> (g.Name, fst g.WinningSpan, snd g.WinningSpan))
    |> Chart.RangeColumn
    |> Chart.WithDataPointLabels (Label = "#VAL")
    |> Chart.WithTitle (Text = sprintf "Golf Major Winning Span (%i or More Years)" min)
    |> Chart.WithXAxis (Title = "Golfer", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Angle = -45, Interval = 1.0))
    |> Chart.WithYAxis (Title = "Year", TitleFontSize = 14.0, LabelStyle = ChartTypes.LabelStyle(Interval = 5.0))
    |> Chart.WithYAxis (Max = 2020.0, Min = 1850.0)

7 |> totalWinsChart |> Chart.Show
5 |> winSpanRangeChart |> Chart.Show