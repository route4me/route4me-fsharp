namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp

module Territory =
    let tests =
        testList "Territory" [
            test "Get" {
                let getResult = Territory.Get(take = 10)

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"

                getResult
                |> Result.iter(fun territories -> 
                    let territory = territories.[0]
                    let oneResult = Territory.Get(territory.Id)

                    Expect.equal (oneResult |> Result.isOk) true "Get one should be Ok"
                    )
            } 
            
            test "Add & Delete" {
                let addResult = Territory.Add("abcdef", "ffad46", { Type = "poly"; Data = [||]})
                Expect.equal (addResult |> Result.isOk) true "Add should be Ok"

                addResult
                |> Result.iter(fun terr ->
                    let delResult = terr.Delete()
                    Expect.equal (delResult |> Result.isOk) true "Delete should be Ok")
                }
                
                ]