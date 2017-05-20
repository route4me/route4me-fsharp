namespace Route4MeSdk.FSharp

open System
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Collections.Generic
open FSharpExt

type Api() = 
    static let demoApiKey = "11111111111111111111111111111111"

    static let getConverters() = [|
        new JsonConverter.Option() :> JsonConverter
        new JsonConverter.Boolean() :> JsonConverter 
        |]

    static let convertResponse(response:HttpResponse) = 
        match response.Body with 
            | Text value -> 
                if response.StatusCode = 200 then   
                    Ok value
                else 
                    let errors = 
                        let dict = JsonConvert.DeserializeObject<Dictionary<string, obj>>(value)
                        let array = dict.["errors"] :?> JArray
                        array.ToObject<string[]>()

                    Error <| ApiError(response.StatusCode, errors)
            | Binary _ -> raise <| new NotSupportedException()   

    static let addApiKey query (apiKey:string option) = 
        match apiKey with
        | Some v -> ("api_key", v) :: query
        | None -> ("api_key", demoApiKey) :: query

    static member Action(url, query, method, apiKey) = 
        let ammendedQuery = addApiKey query apiKey
        Http.Request(String.join "/" url, query = ammendedQuery, httpMethod = method, silentHttpErrors = true)
        |> convertResponse

    static member Action(url, query, method, apiKey, value) =
        let ammendedQuery = addApiKey query apiKey
        let json = JsonConvert.SerializeObject(value, getConverters())
        Http.Request(String.join "/" url, query = ammendedQuery, httpMethod = method, silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
        |> convertResponse

    static member Get(url, query, apiKey) = 
        Api.Action(url, query, "GET", apiKey)

    static member Post(url, query, apiKey) = 
        Api.Action(url, query, "POST", apiKey)

    static member Post(url, query, apiKey, value) = 
        Api.Action(url, query, "POST", apiKey, value)

    static member Put(url, query, apiKey) = 
        Api.Action(url, query, "PUT", apiKey)

    static member Put(url, query, apiKey, value) = 
        Api.Action(url, query, "PUT", apiKey, value)

    static member Delete(url, query, apiKey) = 
        Api.Action(url, query, "DELETE", apiKey)

    static member Delete(url, query, apiKey, value) = 
        Api.Action(url, query, "DELETE", apiKey, value)

    static member Deserialize<'T>(json) =
        JsonConvert.DeserializeObject<'T>(json, getConverters())