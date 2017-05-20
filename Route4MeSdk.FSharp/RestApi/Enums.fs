namespace Route4MeSdk.FSharp

open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Runtime.Serialization

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

type DeviceType =
    | [<EnumMember(Value = "Web")>]
        Web = 0

    | [<EnumMember(Value = "IPhone")>]
        IPhone = 1

    | [<EnumMember(Value = "IPad")>]
        IPad = 2

    | [<EnumMember(Value = "AndroidPhone")>]
        AndroidPhone = 3

    | [<EnumMember(Value = "AndroidTablet")>]
        AndroidTablet = 4

    | [<EnumMember(Value = "UnknownDevice")>]
        UnknownDevice = 99