namespace Route4MeSdk.FSharp.Tests

open Expecto
open FSharpExt
open Route4MeSdk.FSharp

module Member =
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

    let tests =
        testList "Member" [
            test "Create, Get, Update & Delete" {
                let createResult = Member.Create(member')
                Expect.equal (createResult |> Result.isOk) true "Create should be Ok"

                let getResult = 
                    createResult
                    |> Result.andThen(fun m -> Member.Get(m.Id.Value))

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"

                let updateResult = 
                    createResult
                    |> Result.andThen(fun m -> 
                        Member.Update({ m with FirstName = Data.FirstName.random() }))

                Expect.equal (updateResult |> Result.isOk) true "Get should be Ok"

                let deleteResult = 
                    createResult
                    |> Result.andThen(fun m -> m.Delete())
                
                Expect.equal (deleteResult |> Result.isOk) true "Delete should be Ok"
            }
            
            test "Authenticate" {
                let authResult = Member.Authenticate("wade.ochoa@mailwire.com", "=CDMFY2JhT")
                ()
                }

            test "Config - Set,Get & Delete" {
                let result = Member.Get(495660)

                result 
                |> Result.iter(fun m ->
                    let setResult = m.SetConfig("Key", "value")
                    let getResult = m.GetConfig()
                    let delResult = m.DeleteConfig("Key")

                    Expect.equal (setResult |> Result.isOk) true "Set should be Ok"
                    Expect.equal (getResult |> Result.isOk) true "Get should be Ok"
                    Expect.equal (delResult |> Result.isOk) true "Delete should be Ok"
                    ) }]
