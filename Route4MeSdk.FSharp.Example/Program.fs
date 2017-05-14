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
                    Id = 35353
                    Email = "mycustomemail@microsoft.com"
                    Password = "test213" })

    let updateResult =
        getResult
        |> Result.andThen(fun user -> 
            RestApi.User.update apiKey 
                { user with 
                    Email = "mycustomemail@microsoft.com" })
        
    let deleteResult =
        getResult
        |> Result.andThen(fun user -> 
            RestApi.User.delete apiKey user.Id)

    ignore (createResult, updateResult, deleteResult)