namespace Route4MeSdk.FSharp.Example

open System
open Route4MeSdk.FSharp
open FSharpExt

module Main = 
    let apiKey = RestApi.demoApiKey

    let getResult =
        let memberId = 45844
        RestApi.User.get apiKey memberId

    let createResult =
        getResult
        |> Result.andThen(fun user -> 
            RestApi.User.create apiKey 
                { user with 
                    Id = Some 35353
                    Email = "mycustomemail@microsoft.com"
                    Password = "test213" })
        
    let deleteResult =
        getResult
        |> Result.andThen(fun user -> 
            RestApi.User.delete apiKey (user.Id |> Option.withDefault -1) )

    ignore deleteResult