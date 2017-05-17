namespace Route4MeSdk.FSharp

open System
open System.Collections.Generic
open FSharp.Data
open FSharpExt
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Runtime.Serialization
open Newtonsoft.Json.Linq

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

    type ApiError = { 
        StatusCode : int
        Errors : string[] }

    [<AutoOpen>]
    module private Helper = 
        let convertResponse(response:HttpResponse) = 
            match response.Body with 
                | Text value -> 
                    if response.StatusCode = 200 then   
                        Ok value
                    else 
                        {   StatusCode = response.StatusCode
                            Errors = 
                                let dict = JsonConvert.DeserializeObject<Dictionary<string, obj>>(value)
                                let array = dict.["errors"] :?> JArray
                                array.ToObject<string[]>()}
                        |> Error
                | Binary _ -> raise <| new NotSupportedException()   
                
        let getDefaultConverters() = [|
            new JsonConverter.Option() :> JsonConverter
            new JsonConverter.Boolean() :> JsonConverter 
            |]

    [<JsonConverter(typeof<StringEnumConverter>)>]
    type DistanceUnit =
        | [<EnumMember(Value = "MI")>]
          Mile = 0

        | [<EnumMember(Value = "KM")>]
          KiloMeter = 1
          
    [<JsonConverter(typeof<StringEnumConverter>)>]
    type MemberType = 
        | [<EnumMember(Value = "PRIMARY_ACCOUNT")>]
          PrimaryAccount = 0

        | [<EnumMember(Value = "SUB_ACCOUNT_ADMIN")>]
          SubAccountAdmin = 1

        | [<EnumMember(Value = "SUB_ACCOUNT_REGIONAL_MANAGER")>]
          SubAccountRegionalManager = 2

        | [<EnumMember(Value = "SUB_ACCOUNT_DISPATCHER")>]
          SubAccountDispatcher = 3

        | [<EnumMember(Value = "SUB_ACCOUNT_PLANNER")>]
          SubAccountPlanner = 4

        | [<EnumMember(Value = "SUB_ACCOUNT_DRIVER")>]
          SubAccountDriver = 5

        | [<EnumMember(Value = "SUB_ACCOUNT_ANALYST")>]
          SubAccountAnalyst = 6

        | [<EnumMember(Value = "SUB_ACCOUNT_VENDOR")>]
          SubAccountVendor = 7

        | [<EnumMember(Value = "SUB_ACCOUNT_CUSTOMER_SERVICE")>]
          SubAccountCustomerService = 8
          
    [<CLIMutable>]
    type Member = {
        [<JsonProperty("member_id")>]
        Id : int option
        
        [<JsonProperty("OWNER_MEMBER_ID")>]
        OwnerId : int option
        
        [<JsonProperty("member_type")>]
        Type : MemberType
        
        [<JsonProperty("member_first_name")>]
        FirstName : string
        
        [<JsonProperty("member_last_name")>]
        LastName : string
        
        [<JsonProperty("member_email")>]
        Email : string 
        
        [<JsonProperty("member_phone")>]
        Phone : string

        [<JsonProperty("READONLY_USER")>]
        ReadOnly : bool option

        [<JsonProperty("member_password")>]
        Password : string
        
        [<JsonProperty("date_of_birth")>]
        DateOfBirth : string //DateTime option
        
        [<JsonProperty("timezone")>]
        TimeZone : string
        
        [<JsonProperty("member_zipcode")>]
        ZipCode : string    
        
        [<JsonProperty("preferred_language")>]
        PreferedLanguage : string

        [<JsonProperty("preferred_units")>]
        PreferedUnit : DistanceUnit option

        [<JsonProperty("SHOW_ALL_DRIVERS")>]
        ShowAllDrivers : bool option
        
        [<JsonProperty("SHOW_ALL_VEHICLES")>]
        ShowAllVehicles : bool option
        
        [<JsonProperty("HIDE_ROUTED_ADDRESSES")>]
        HideRoutedAddresses : bool option
        
        [<JsonProperty("HIDE_VISITED_ADDRESSES")>]
        HideVisitedAddresses : bool option
        
        [<JsonProperty("HIDE_NONFUTURE_ROUTES")>]
        HideNonFutureAddresses : bool option }

    module Member =
        let get apiKey memberId =
            let url = String.Join("/", Url.V4.user)
            let query = [
                ("api_key", apiKey)
                ("member_id", memberId.ToString())]
            
            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> convertResponse
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, getDefaultConverters()))

        let getAll apiKey =
            let url = String.Join("/", Url.V4.user)
            let query = [
                ("api_key", apiKey)]
            
            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> convertResponse
            |> Result.map(fun json -> 
                let dict = JsonConvert.DeserializeObject<Dictionary<string,obj>>(json, getDefaultConverters())
                let results = dict.["results"] :?> JArray
                let itemsJson = results.ToString()
                JsonConvert.DeserializeObject<Member[]>(itemsJson, getDefaultConverters()))

        let create apiKey (member' : Member) =
            let url = String.Join("/", Url.V4.user)
            let query = [
                ("api_key", apiKey)]

            let json = JsonConvert.SerializeObject(member', getDefaultConverters())

            Http.Request(url, query = query, httpMethod = "POST", silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
            |> convertResponse
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, getDefaultConverters()))

        let update apiKey (member' : Member) =
            let url = String.Join("/", Url.V4.user)
            let query = [
                ("api_key", apiKey)
                ("member_id", member'.Id.ToString())]

            let json = JsonConvert.SerializeObject(member', getDefaultConverters())

            Http.Request(url, query = query, httpMethod = "PUT", silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
            |> convertResponse            
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, getDefaultConverters()))

        let delete apiKey (memberId:int) =
            let url = String.Join("/", Url.V4.user)
            let query = [
                ("api_key", apiKey)
                ("member_id", memberId.ToString())]
            Http.Request(url, query = query, httpMethod = "DELETE", silentHttpErrors = true)
            |> convertResponse
            |> Result.map ignore

        // Note: Currently returns Html response despite the format = "json"
        let authenticate apiKey email password =
            let url = String.Join("/", Url.actionAuthenticate)
            let query = [
                ("api_key", apiKey)
                ("format", "xml")
                ("strEmail", email)
                ("strPassword", password)]
            
            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> convertResponse