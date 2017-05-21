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
            Email = (sprintf "%s.%s@gmail.com" firstName lastName).ToLower()
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
                    |> Result.bind(fun m -> Member.Get(m.Id.Value))

                Expect.equal (getResult |> Result.isOk) true "Get should be Ok"

                let updateResult = 
                    createResult
                    |> Result.bind(fun m -> 
                        Member.Update({ m with FirstName = Data.FirstName.random() }))

                Expect.equal (updateResult |> Result.isOk) true "Update should be Ok"

                let deleteResult = 
                    createResult
                    |> Result.bind(fun m -> m.Delete())
                
                Expect.equal (deleteResult |> Result.isOk) true "Delete should be Ok"
            }

            test "Register" {
                let firstName = Data.FirstName.random()
                let lastName = Data.LastName.random()
                let email = Data.Email.create(sprintf "%s.%s" firstName lastName)
                let password = Data.Password.random 10
                let industry = ""
                let plan = ""
                let regResult = 
                    Member.Register(firstName, lastName, email, password, DeviceType.Web, industry, plan)
                Expect.equal (regResult |> Result.isOk) true "Register should be Ok"
                }
            
            test "Authenticate" {
                let createResult = Member.Create(member')
                Expect.equal (createResult |> Result.isOk) true "Create should be Ok"

                createResult 
                |> Result.iter(fun _ ->
                    let authResult = Member.Authenticate(member'.Email, member'.Password)
                    ())
                }

            test "ValidateSession" {
                let result = Member.Get(495660)

                result 
                |> Result.iter(fun m ->
                    let valResult = m.ValidateSession("")
                    Expect.equal (valResult |> Result.isOk) true "Should be Ok")
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
