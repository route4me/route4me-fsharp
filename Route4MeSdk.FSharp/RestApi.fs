namespace Route4MeSdk.FSharp

open System
open System.Collections.Generic
open FSharp.Data
open FSharpExt
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Runtime.Serialization
open Newtonsoft.Json.Linq
open FSharp.Data.HtmlDocument

module RestApi =
    ()

    
    //[<CLIMutable>]
    //type Vehicle = {
    //    [<JsonProperty("vehicle_id")>]
    //    Id: string

    //    [<JsonProperty("created_time")>]
    //    CreatedTime: string

    //    [<JsonProperty("member_id")>]
    //    MemberId: int

    //    [<JsonProperty("vehicle_alias")>]
    //    Alias: string

    //    [<JsonProperty("vehicle_reg_state")>]
    //    RegState: string

    //    [<JsonProperty("vehicle_reg_state_id")>]
    //    RegStateId: string

    //    [<JsonProperty("vehicle_reg_country")>]
    //    RegCountry: string

    //    [<JsonProperty("vehicle_reg_country_id")>]
    //    RegCountryId: string

    //    [<JsonProperty("vehicle_license_plate")>]
    //    LicensePlate: string

    //    [<JsonProperty("vehicle_make")>]
    //    Maker: string

    //    [<JsonProperty("vehicle_model_year")>]
    //    ModelYear: string

    //    [<JsonProperty("vehicle_model")>]
    //    Model: string

    //    [<JsonProperty("vehicle_year_acquired")>]
    //    YearAcquired: string

    //    [<JsonProperty("vehicle_cost_new")>]
    //    CostNew: double option

    //    [<JsonProperty("license_start_date")>]
    //    LicenseStartDate: string

    //    [<JsonProperty("license_end_date")>]
    //    LicenseEndDate: string

    //    [<JsonProperty("vehicle_axle_count")>]
    //    AxleCount: string

    //    [<JsonProperty("mpg_city")>]
    //    MpgCity: string

    //    [<JsonProperty("mpg_highway")>]
    //    MpgHighway: string
        
    //    [<JsonProperty("fuel_type")>]
    //    FuelType: string
        
    //    [<JsonProperty("height_inches")>]
    //    HeightInches: string
        
    //    [<JsonProperty("weight_lb")>]
    //    WeightLb: string }

    //module Vehicle =
    //    //[<CLIMutable>]
    //    //type internal Get = { 
    //    //    [<JsonProperty("vehicle_id")>]
    //    //    VehicleId : string

    //    //    [<JsonProperty("offset")>]
    //    //    Skip : uint32 option

    //    //    [<JsonProperty("limit")>]
    //    //    Take : uint32 option }

    //    let get apiKey vehicleId skip take =
    //        let url = String.Join("/", Url.V1.viewVehicles)
    //        let query = [("api_key", apiKey)]
            
    //        Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
    //        |> convertResponse
    //        |> Result.map(fun json -> JsonConvert.DeserializeObject<Vehicle[]>(json, getDefaultConverters()))