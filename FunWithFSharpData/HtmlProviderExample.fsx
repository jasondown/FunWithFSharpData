#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data

[<Literal>]
let Season1 = @"https://en.wikipedia.org/wiki/Vikings_(season_1)"

[<Literal>]
let Season2 = @"https://en.wikipedia.org/wiki/Vikings_(season_2)"

[<Literal>]
let Season3 = @"https://en.wikipedia.org/wiki/Vikings_(season_3)"

//[<Literal>]
//let Season4 = @"https://en.wikipedia.org/wiki/Vikings_(season_4)"

//[<Literal>]
//let Season5 = @"https://en.wikipedia.org/wiki/Vikings_(season_5)"

type Episode =
    { Title       : string
      Description : string
      Season      : int
      Number      : int
    }

let s1 = new HtmlProvider<Season1>()
let s1eps = 
    s1.Tables.Episodes.Rows 
    |> Seq.chunkBySize 2 
    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
                           Season = 1
                           Number = e.[0].``No. in season`` |> int
                           Description = e.[1].Title })
    |> Seq.toArray

let s2 = new HtmlProvider<Season2>()
let s2eps = 
    s2.Tables.Episodes.Rows 
    |> Seq.chunkBySize 2 
    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
                           Season = 2
                           Number = e.[0].``No. in season`` |> int
                           Description = e.[1].Title })
    |> Seq.toArray

let s3 = new HtmlProvider<Season3>()
let s3eps = 
    s3.Tables.Episodes.Rows 
    |> Seq.chunkBySize 2 
    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
                           Season = 3
                           Number = e.[0].``No. in season`` |> int
                           Description = e.[1].Title })
    |> Seq.toArray

// Format has changed fro season 4 and 5-------------------------------------------

//let s4 = new HtmlProvider<Season4>()
//let s4eps = 
//    s4.Tables.Episodes.Rows
//    |> Seq.chunkBySize 2 
//    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
//                           Season = 4
//                           Number = e.[0].``No. in season`` |> int
//                           Description = e.[1].Title })
//    |> Seq.toArray

//let s5 = new HtmlProvider<Season5>()
//let s5eps = 
//    s5.Tables.Episodes.Rows 
//    |> Seq.chunkBySize 2 
//    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
//                           Season = 5
//                           Number = e.[0].``No. in season`` |> int
//                           Description = e.[1].Title })
//    |> Seq.toArray

let printEpisodes (episodes : Episode array) =
    episodes
    |> Array.iter (fun e -> printfn "%A\r\n" e)

// Test
[| s1eps; s2eps; s3eps |] |> Array.concat |> printEpisodes

