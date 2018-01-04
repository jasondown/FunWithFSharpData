#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data

[<Literal>]
let Season1 = @"https://en.wikipedia.org/wiki/Vikings_(season_1)"

[<Literal>]
let Season2 = @"https://en.wikipedia.org/wiki/Vikings_(season_2)"

[<Literal>]
let Season3 = @"https://en.wikipedia.org/wiki/Vikings_(season_3)"

// Seasons 4 and 5 have a different format. Will deal with them later.
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

type EpisodeInfo = HtmlProvider<Season1>

let getEpisodes seasonNum (seasons : EpisodeInfo) =
    seasons.Tables.Episodes.Rows
    |> Seq.chunkBySize 2 
    |> Seq.map (fun e -> { Title = e.[0].Title |> String.collect (fun c -> if c = '"' then "" else string c)
                           Season = seasonNum
                           Number = e.[0].``No. in season`` |> int
                           Description = e.[1].Title })
    |> Seq.toArray

let printEpisodes (episodes : Episode array) =
    episodes
    |> Array.iter (fun e -> printfn "%A\r\n" e)

let s1 = EpisodeInfo.Load(Season1) |> getEpisodes 1
let s2 = EpisodeInfo.Load(Season2) |> getEpisodes 2
let s3 = EpisodeInfo.Load(Season3) |> getEpisodes 3

// Test
[| s1; s2; s3 |] |> Array.concat |> printEpisodes

