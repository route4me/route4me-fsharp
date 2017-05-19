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
            let url = Url.build Url.V4.user
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)
                ("member_id", memberId.ToString())]
            
            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> Api.convertResponse
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, Api.getConverters()))

        static member GetAll(?apiKey) =
            let url = Url.build Url.V4.user
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)]
            
            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> Api.convertResponse
            |> Result.map(fun json -> 
                let dict = JsonConvert.DeserializeObject<Dictionary<string,obj>>(json, Api.getConverters())
                let results = dict.["results"] :?> JArray
                let itemsJson = results.ToString()
                JsonConvert.DeserializeObject<Member[]>(itemsJson, Api.getConverters()))

        static member Create(member' : Member, ?apiKey) =
            let url = Url.build Url.V4.user
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)]

            let json = JsonConvert.SerializeObject(member', Api.getConverters())

            Http.Request(url, query = query, httpMethod = "POST", silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
            |> Api.convertResponse
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, Api.getConverters()))

        static member Update(member' : Member, ?apiKey) =
            let url = Url.build Url.V4.user
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)
                ("member_id", member'.Id.ToString())]

            let json = JsonConvert.SerializeObject(member', Api.getConverters())

            Http.Request(url, query = query, httpMethod = "PUT", silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
            |> Api.convertResponse            
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Member>(json, Api.getConverters()))

        static member Delete(memberId:int, ?apiKey) =
            let url = Url.build Url.V4.user
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)
                ("member_id", memberId.ToString())]
            Http.Request(url, query = query, httpMethod = "DELETE", silentHttpErrors = true)
            |> Api.convertResponse
            |> Result.map ignore

        member self.Delete() =
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id -> Member.Delete(id))

        // Note: Currently returns Html response despite the format = "json"
        static member Authenticate(email, password, ?apiKey) =
            let url = Url.build Url.actionAuthenticate
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)
                ("format", "json")
                ("strEmail", email)
                ("strPassword", password)]
            
            Http.Request(url, query = query, httpMethod = "POST", silentHttpErrors = true)
            |> Api.convertResponse

        member self.SetConfig (key, value, ?apiKey) = 
            let url = Url.build Url.V4.configSettings
            let query = [("api_key", defaultArg apiKey Api.demoApiKey)]
            
            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let config = 
                    { Id = id
                      Key = key
                      Value = value }

                let json = JsonConvert.SerializeObject(config, Api.getConverters())
                Http.Request(url, query = query, httpMethod = "PUT", silentHttpErrors = true, body = HttpRequestBody.TextRequest json)
                |> Api.convertResponse)

        member self.GetConfig(?apiKey) =
            let url = Url.build Url.V4.configSettings

            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let query = 
                    [("api_key", defaultArg apiKey Api.demoApiKey)
                     ("member_id", id.ToString())]
            
                Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
                |> Api.convertResponse
                |> Result.map(fun json -> 
                    let converters = Api.getConverters()
                    let dict = JsonConvert.DeserializeObject<Dictionary<string,obj>>(json, converters)
                    let data = dict.["data"] :?> JArray
                    JsonConvert.DeserializeObject<MemberConfig[]>(data.ToString(), converters)))

        member self.DeleteConfig(key, ?apiKey) =
            let url = Url.build Url.V4.configSettings

            self.Id
            |> Result.ofOption(ValidationError("Member Id must be supplied."))
            |> Result.andThen(fun id ->
                let query = 
                    [("api_key", defaultArg apiKey Api.demoApiKey)
                     ("member_id", id.ToString())]
                
                let json = 
                    [("config_key", key)] 
                    |> dict
                    |> JsonConvert.SerializeObject
                    |> HttpRequestBody.TextRequest

                Http.Request(url, query = query, httpMethod = "DELETE", silentHttpErrors = true, body = json)
                |> Api.convertResponse)