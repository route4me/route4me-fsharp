namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp

module Activity =
    let tests =
        testList "Activity" [
            test "Get" {
                let getResult = Activity.Get()

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
            } 
            
            ]