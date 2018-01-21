
#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data
open System

[<Literal>]
let allApis = "https://www.googleapis.com/discovery/v1/apis"

type GoogleApis = JsonProvider<allApis>

let apis = GoogleApis.GetSample()

let printApiDetails (apis : GoogleApis.Root) =
    apis.Items
    |> Array.iter (fun a -> printfn "%A" a)

apis |> printApiDetails