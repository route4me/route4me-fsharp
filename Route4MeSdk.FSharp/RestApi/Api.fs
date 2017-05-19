namespace Route4MeSdk.FSharp

open System
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Collections.Generic
open FSharpExt

module Api = 
    let demoApiKey = "11111111111111111111111111111111"

    let getConverters() = [|
        new JsonConverter.Option() :> JsonConverter
        new JsonConverter.Boolean() :> JsonConverter 
        |]

    let convertResponse(response:HttpResponse) = 
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
                
    