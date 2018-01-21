#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data
open System

[<Literal>]
let allApis = "https://www.googleapis.com/discovery/v1/apis"

type GoogleApis = JsonProvider<allApis>
let apis = GoogleApis.GetSample()

type ApiSummary = {
    Title : string
    Description : string
    Documentation : string
    }

let getApiSummaries (apis : GoogleApis.Root) =
    apis.Items
    |> Array.map ( fun a ->
        let desc = match a.Description with
                   | Some d -> d
                   | None -> ""
        let doc = match a.DocumentationLink with
                  | Some d -> d
                  | None -> ""
        { Title = a.Title
          Description = desc
          Documentation = doc
        })

let printApiSummaries (apis : ApiSummary array) =
    apis
    |> Array.iter (fun a ->
        printfn "--------------------"
        printfn "Title: %s" a.Title
        printfn "Description: %s" a.Description 
        printfn "Documentation: %s" a.Documentation
        printfn "--------------------")

// Print out the api summaries
apis |> (getApiSummaries >> printApiSummaries)