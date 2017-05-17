namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp.RestApi

module Member =
    let tests =
        let member' = 
            let firstName = Data.FirstName.random()
            let lastName = Data.LastName.random()
            {   Id = None
                Type = MemberType.PrimaryAccount
                OwnerId = None
                FirstName = firstName
                LastName = lastName
                Email = Data.Email.create (sprintf "%s.%s" firstName lastName)
                Phone = Data.Phone.random()
                Password = Data.Password.random 10
                ReadOnly = None 
                DateOfBirth = "1979-10-15"
                TimeZone = "utc"
                ZipCode = null
                PreferedLanguage = "en" 
                PreferedUnit = Some DistanceUnit.KiloMeter
                ShowAllDrivers = None 
                ShowAllVehicles = None 
                HideRoutedAddresses = None
                HideVisitedAddresses = None 
                HideNonFutureAddresses = None }

        testList "Member" [
            test "Create/Update/Get/Delete" {
                let createResult = Member.create demoApiKey member'
                Expect.equal (createResult |> Result.isOk) true "Create should be Ok"

                let getResult = 
                    createResult
                    |> Result.andThen(fun m -> Member.get demoApiKey m.Id.Value)

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"

                let updateResult = 
                    createResult
                    |> Result.andThen(fun m -> 
                        Member.update demoApiKey { m with FirstName = Data.FirstName.random() })

                Expect.equal (updateResult |> Result.isOk) true "Get should be Ok"

                let deleteResult = 
                    createResult
                    |> Result.andThen(fun m -> Member.delete demoApiKey m.Id.Value)
                
                Expect.equal (deleteResult |> Result.isOk) true "Delete should be Ok"
            }]