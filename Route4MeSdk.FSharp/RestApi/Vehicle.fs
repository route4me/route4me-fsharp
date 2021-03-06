﻿namespace Route4MeSdk.FSharp

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
        static member Get(?take:int, ?skip:int, ?apiKey) =
            let query = 
                [ take |> Option.map(fun v -> "limit", v.ToString())
                  skip |> Option.map(fun v -> "offset", v.ToString()) ]
                |> List.choose id

            Api.Post(Url.V1.viewVehicles, [], query, apiKey)
            |> Result.map Api.Deserialize<Vehicle[]>