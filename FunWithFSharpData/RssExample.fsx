#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
#r "System.Xml.Linq.dll"
open FSharp.Data

[<Literal>]
let SampleFeed = "http://www.jason-down.com/feed/"

type RssFeed = XmlProvider<SampleFeed>

type Article =
    { Title         : string
      Description   : string
      Link          : string }

let hr = "-----------------------------------------------------------------------------------"

let getFeed (url : string) = RssFeed.Load(url).Channel

let getRecentArticles n (rss : RssFeed.Channel) =
    rss.Items
    |> Seq.truncate n
    |> Seq.map (fun i -> { Title = i.Title; Description = i.Description; Link = i.Link })
    |> Seq.toArray

let printArticles articles =
    articles
    |> Array.iter (fun a -> printfn "%s\r\n%s\r\n%s\r\n%s\r\n%s\r\n" hr a.Title a.Description a.Link hr)

let printRecentArticles url n =
    url |> getFeed |> getRecentArticles n |> printArticles

// Fun with partial function application
let wiredTopStories = printRecentArticles "https://www.wired.com/feed/rss"
let myBlog = printRecentArticles "http://www.jason-down.com/feed/"

// Test it out
wiredTopStories 3
wiredTopStories 10
myBlog 5
myBlog 1000 // Should not error; grabs as many as availabe