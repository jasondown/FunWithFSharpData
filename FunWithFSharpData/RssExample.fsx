#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "System.Xml.Linq.dll"
open FSharp.Data

[<Literal>]
let RssFeedUrl = "http://www.jason-down.com/feed/"

type RssFeed = XmlProvider<RssFeedUrl>

type Article =
    { Title         : string
      Description   : string
      Link          : string }

let feed = RssFeed.GetSample()

let getRecentArticles (n : int) (rss : RssFeed.Channel) =
    rss.Items
    |> Seq.take n
    |> Seq.map (fun i -> { Title = i.Title; Description = i.Description; Link = i.Link })
    |> Seq.toArray

let printArticles articles =
    articles
    |> Array.iter (fun a -> printfn "Title: %s\n\rDescription: %s\n\rLink: %s\n\r" a.Title a.Description a.Link)

//Test most recent blog posts
feed.Channel |> getArticlesFromFeed 5 |> printArticles