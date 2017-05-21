namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp

module AvoidanceZone =
    let tests =
        testList "AvoidanceZone" [
            test "Get" {
                let getResult = AvoidanceZone.Get(take = 10)

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"

                getResult
                |> Result.iter(fun zones -> 
                    let zone = zones.[0]
                    let oneResult = AvoidanceZone.Get(zone.Id)

                    Expect.equal (oneResult |> Result.isOk) true "Get one should be Ok"
                    )
            } 
            
            test "Add & Delete" {
                let addResult = AvoidanceZone.Add("abcdef", "ffad46", { Type = "poly"; Data = [||]})
                Expect.equal (addResult |> Result.isOk) true "Add should be Ok"

                addResult
                |> Result.iter(fun inst ->
                    let delResult = inst.Delete()
                    Expect.equal (delResult |> Result.isOk) true "Delete should be Ok")
                }
                
                ]