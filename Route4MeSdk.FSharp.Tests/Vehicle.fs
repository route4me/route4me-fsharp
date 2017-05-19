namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp

module Vehicle =
    let tests =
        testList "Vehicle" [
            test "Get" {
                let getResult = Vehicle.Get()

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
            } ]