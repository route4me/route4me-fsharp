namespace Route4MeSdk.FSharp.Example

open System
open Route4MeSdk.FSharp

module Main = 
    let apiKey = RestApi.demoApiKey

    let getUsers() =
        let memberId = 45844
        let response = RestApi.User.get apiKey memberId
        ignore response

    let createUser() = 
        let memberId = 45844
        RestApi.User.