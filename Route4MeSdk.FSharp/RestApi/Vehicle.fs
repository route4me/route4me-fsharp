namespace Route4MeSdk.FSharp

open System
open System.Collections.Generic
open FSharpExt
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq

[<CLIMutable>]
type Vehicle = {
    [<JsonProperty("vehicle_id")>]
    Id: string

    [<JsonProperty("created_time")>]
    CreatedTime: string

    [<JsonProperty("member_id")>]
    MemberId: int

    [<JsonProperty("vehicle_alias")>]
    Alias: string

    [<JsonProperty("vehicle_reg_state")>]
    RegState: string

    [<JsonProperty("vehicle_reg_state_id")>]
    RegStateId: string

    [<JsonProperty("vehicle_reg_country")>]
    RegCountry: string

    [<JsonProperty("vehicle_reg_country_id")>]
    RegCountryId: string

    [<JsonProperty("vehicle_license_plate")>]
    LicensePlate: string

    [<JsonProperty("vehicle_make")>]
    Maker: string

    [<JsonProperty("vehicle_model_year")>]
    ModelYear: string

    [<JsonProperty("vehicle_model")>]
    Model: string

    [<JsonProperty("vehicle_year_acquired")>]
    YearAcquired: string

    [<JsonProperty("vehicle_cost_new")>]
    CostNew: double option

    [<JsonProperty("license_start_date")>]
    LicenseStartDate: string

    [<JsonProperty("license_end_date")>]
    LicenseEndDate: string

    [<JsonProperty("vehicle_axle_count")>]
    AxleCount: string

    [<JsonProperty("mpg_city")>]
    MpgCity: string

    [<JsonProperty("mpg_highway")>]
    MpgHighway: string
        
    [<JsonProperty("fuel_type")>]
    FuelType: string
        
    [<JsonProperty("height_inches")>]
    HeightInches: string
        
    [<JsonProperty("weight_lb")>]
    WeightLb: string }

    with
        static member Get(?apiKey) =
            let url = Url.build Url.V1.viewVehicles
            let query = [
                ("api_key", defaultArg apiKey Api.demoApiKey)]

            Http.Request(url, query = query, httpMethod = "GET", silentHttpErrors = true)
            |> Api.convertResponse
            |> Result.map(fun json -> JsonConvert.DeserializeObject<Vehicle[]>(json, Api.getConverters()))