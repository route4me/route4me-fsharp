namespace Route4MeSdk.FSharp

open System
open System.Collections.Generic
open FSharpExt
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq

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

    with
        static member Get(memberId:int, ?apiKey) =
            let query = [("member_id", memberId.ToString())]
            
            Api.Get(Url.V4.user, query, apiKey)
            |> Result.map(Api.Deserialize<Member>)

        static member GetAll(?apiKey) =
            Api.Get(Url.V4.user, [], apiKey)
            |> Result.map(fun json -> 
                let dict = Api.Deserialize<Dictionary<string,obj>>(json)
                let results = dict.["results"] :?> JArray
                let itemsJson = results.ToString()
                Api.Deserialize<Member[]>(results.ToString()))

        static member Create(member' : Member, ?apiKey) =
            Api.Post(Url.V4.user, [], apiKey, member')
            |> Result.map Api.Deserialize<Member>

        static member Update(member' : Member, ?apiKey) =
            let query = [("member_id", member'.Id.ToString())]

            Api.Put(Url.V4.user, query, apiKey, member')
            |> Result.map Api.Deserialize<Member>

        static member Delete(memberId:int, ?apiKey) =
            let query = [("member_id", memberId.ToString())]
            Api.Delete(Url.V4.user, query, apiKey)

        member self.Delete() =
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id -> Member.Delete(id))

        // Note: Currently returns Html response despite the format = "json"
        static member Authenticate(email, password, ?apiKey) =
            let query = [
                ("format", "json")
                ("strEmail", email)
                ("strPassword", password)]
            
            Api.Post(Url.actionAuthenticate, query, apiKey)

        member self.SetConfig (key, value, ?apiKey) = 
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let config = 
                    { Id = id
                      Key = key
                      Value = value }

                Api.Put(Url.V4.configSettings, [], apiKey, config))

        member self.GetConfig(?apiKey) =
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let query = [("member_id", id.ToString())]
            
                Api.Get(Url.V4.configSettings, [], apiKey)
                |> Result.map(fun json -> 
                    let dict = Api.Deserialize<Dictionary<string,obj>>(json)
                    let data = dict.["data"] :?> JArray
                    Api.Deserialize<MemberConfig[]>(data.ToString())))

        member self.DeleteConfig(key, ?apiKey) =
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let query = [("member_id", id.ToString())]
                let value = [("config_key", key)] |> dict

                Api.Delete(Url.V4.configSettings, query, apiKey, value))