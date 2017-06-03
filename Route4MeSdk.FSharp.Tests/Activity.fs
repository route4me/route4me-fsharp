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

            test "Get with Member Query" {
                let getResult = Activity.Get(memberId = "1")

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
            }

            test "Get with ActivityType Query" {
                let getResult = Activity.Get(activityType = ActivityType.MemberDeleted)

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
            } 

            test "Log activity message" {
                let getResult = Activity.LogMessage("1", "Bla")

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
            } 
            
            ]