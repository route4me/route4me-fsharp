namespace Route4MeSdk.FSharp

open System
open FSharp.Data
open FSharpExt

module RestApi =
    let demoApiKey = "11111111111111111111111111111111"

    module private Url =
        let host = [ "https://www.route4me.com" ]

        let route4me = host @ [ "route4me.php" ]
        let trackSet = host @ [ "track"; "set.php" ]
        let actionAuthenticate = host @ [ "actions"; "authenticate.php" ]
        let actionRegister = host @ [ "actions"; "regiser_action.php" ]
        let validateSession = host @ [ "datafeed"; "session"; "validate_session.php" ]
        let addRouteNotes = host @ [ "actions"; "addRouteNotes.php" ]
        let duplicateRoute = host @ [ "actions"; "duplicate_route.php" ]

        module V3 = 
            let segments = host @ [ "api.v3" ]

            let routeReoptimize = segments @ [ "route"; "reoptimize_2.php" ]

        module V4 =
            let segments = host @ [ "api.v4" ]

            let route = segments @ [ "route.php" ]
            let user = segments @ [ "user.php" ]
            let configSettings = segments @ [ "configuration-settings.php" ]
            let activityFeed = segments @ [ "activity_feed.php" ]
            let address = segments @ [ "address.php" ]
            let addressBook = segments @ [ "address_book.php" ]
            let avoidance = segments @ [ "avoidance.php" ]
            let territory = segments @ [ "territory.php" ]
            let order = segments @ [ "order.php" ]
            let status = segments @ [ "status.php" ]

    type DistanceUnit =
        | Mile
        | KiloMeter

    type UserPreference = {
        Unit : DistanceUnit
        Language : string }

    type UserType = 
        | SubAccountDriver
        | SubAccountDispatcher

    type User = {
        Id : int
        OwnerId : int option
        Type : UserType
        FirstName : string
        LastName : string
        Email : string 
        Phone : string
        DateOfBirth : DateTime option
        Preference : UserPreference 
        TimeZone : string
        ZipCode : string }

    module User =
        type GetResponse = JsonProvider<"JsonData/GetUserResponse.json">
        type CreateRequest = JsonProvider<"JsonData/CreateUserRequest.json">

        let get apiKey memberId =
            let url = String.Join("/", Url.V4.user)
            let apiKeyQuery = ("api_key", apiKey)
            let memberIdQuery = ("member_id", sprintf "%i" memberId)
            let response = Http.Request(url, query=[apiKeyQuery; memberIdQuery], httpMethod="GET")
            
            if response.StatusCode = 200 then 
                match response.Body with 
                | Text value -> 
                    let json = GetResponse.Parse(value)
                    let instance = {
                        Id = json.MemberId
                        OwnerId = json.OwnerMemberId |> Option.ofObj |> Option.andThen(Int32.TryParse >> Option.ofBoolAndValue)
                        Type = UserType.SubAccountDriver //TODO: Match on response field
                        FirstName = json.MemberFirstName
                        LastName = json.MemberLastName
                        Email = json.MemberEmail
                        Phone = json.MemberPhone
                        DateOfBirth = None
                        Preference = { 
                            Unit = if json.PreferredUnits = "MI" then DistanceUnit.Mile else DistanceUnit.KiloMeter
                            Language = json.PreferredLanguage }
                        TimeZone = json.Timezone
                        ZipCode = json.MemberZipcode }
                    Ok instance
                | Binary _ -> raise <| new NotSupportedException()
            else Error <| response.StatusCode

        let create apiKey (firstName, lastName, dateOfBirth, password, email, phone, zipCode) =
            let url = String.Join("/", Url.V4.user)
            let apiKeyQuery = ("api_key", apiKey)
            let request = 
                new CreateRequest.Root(
                    false, phone, zipCode, 0, email, false, false, "",dateOfBirth, 
                    firstName, password, false, lastName, true, true)
            //let requestJson = CreateRequest. request.
            //let response = Http.Request(url, query=[apiKeyQuery], httpMethod="POST")
            ()
