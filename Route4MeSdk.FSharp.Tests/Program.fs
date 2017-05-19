open System
open Expecto
open Route4MeSdk.FSharp.Tests

[<Tests>]
let tests =
    [ Member.tests
      Vehicle.tests 
      ]
    |> testList "Route4MeSdk.FSharp.Tests"

[<EntryPoint>]
let main args =
    runTestsWithArgs defaultConfig args tests