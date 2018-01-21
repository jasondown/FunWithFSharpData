#r "../packages/FSharp.Data.2.4.3/lib/net45/FSharp.Data.dll"
open FSharp.Data
open System
open System.IO
open System.Text

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

let generateHtml (apis : ApiSummary array) =
    let html = new StringBuilder()
    html.Append(@"<?xml version=""1.0"" encoding=""iso-8859-1""?>" + Environment.NewLine +
                @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" + Environment.NewLine +
                @"<html lang=""en"" xml:lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">" + Environment.NewLine +
                @"<head />" + Environment.NewLine +
                @"<body>" + Environment.NewLine +
                @"<h1 style=""text-align: center;"">Google API List</h1>" + Environment.NewLine +
                @"<table style=""border-color: green;"" border=""2"">" + Environment.NewLine +
                @"<tbody>" + Environment.NewLine +
                @"<tr>" + Environment.NewLine +
                @"<td style=""text-align: center;""><strong>Title</strong></td>" + Environment.NewLine +
                @"<td style=""text-align: center;""><strong>Description</strong></td>" + Environment.NewLine +
                @"<td style=""text-align: center;""><strong>Documenation Link</strong></td></tr>") |> ignore
    apis
    |> Array.iter (fun a ->
        html.Append(@"<tr>" + Environment.NewLine +
                    (sprintf @"<td>%s</td>" a.Title) + Environment.NewLine +
                    (sprintf @"<td>%s</td>" a.Description) + Environment.NewLine +
                    (sprintf @"<td><a href=""%s"" target=""_blank"" rel=""noopener"">%s</a></td></tr>" a.Documentation a.Documentation) + Environment.NewLine) |> ignore)
    html.Append(@"</tbody>" + Environment.NewLine +
                @"</table>" + Environment.NewLine +
                @"<p>&nbsp;</p>" + Environment.NewLine +
                @"</body>" + Environment.NewLine +
                @"</html>") |> ignore
    html.ToString()

let saveHtml (filePath : string) (html : string) =
    Directory.CreateDirectory(Path.GetDirectoryName filePath) |>ignore
    File.WriteAllText(filePath, html)
    printfn "Saved to %s" filePath

// Create Api Summary webpage
apis 
|> (getApiSummaries >> generateHtml) 
|> saveHtml (__SOURCE_DIRECTORY__ + @"\SampleOutput\GoogleApis.html")
