namespace Route4MeSdk.FSharp

open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Runtime.Serialization
open System.Runtime.CompilerServices

[<Extension>]
type EnumExtension () =
    [<Extension>]
    static member GetStringValue<'T
            when 'T :> System.Enum 
             and 'T : struct > (value:'T) = 
            
        let t = typeof<'T>
        t.GetMember(value.ToString())
        |> Array.tryHead
        |> Option.bind(fun m ->
            m.GetCustomAttributes(typeof<EnumMemberAttribute>, false)
            |> Array.tryHead
            |> Option.map (fun attr -> attr :?> EnumMemberAttribute))
        |> Option.map(fun attr -> attr.Value)
        |> Option.defaultWith(fun _ -> (sprintf "%A" value))     

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

[<JsonConverter(typeof<StringEnumConverter>)>]
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

[<JsonConverter(typeof<StringEnumConverter>)>]
type ActivityType = 
    | [<EnumMember(Value = "delete-destination")>]
        DeleteDestination = 0

    | [<EnumMember(Value = "insert-destination")>]
        InsertDestination = 1
    
    | [<EnumMember(Value = "mark-destination-departed")>]
        MarkDestinationDeparted = 2
    
    | [<EnumMember(Value = "mark-destination-visited")>]
        MarkDestinationVisited = 3
    
    | [<EnumMember(Value = "member-created")>]
        MemberCreated = 4
    
    | [<EnumMember(Value = "member-deleted")>]
        MemberDeleted = 5
    
    | [<EnumMember(Value = "member-modified")>]
        MemberModified = 6
    
    | [<EnumMember(Value = "move-destination")>]
        MoveDestination = 7
    
    | [<EnumMember(Value = "note-insert")>]
        NoteInsert = 8
    
    | [<EnumMember(Value = "route-delete")>]
        RouteDelete = 9
    
    | [<EnumMember(Value = "route-optimized")>]
        RouteOptimized = 10
    
    | [<EnumMember(Value = "route-owner-changed")>]
        RouteOwnerChanged = 11
    
    | [<EnumMember(Value = "update-destinations")>]
        UpdateDestinations = 12
    
    | [<EnumMember(Value = "area-added")>]
        AreaAdded = 13
    
    | [<EnumMember(Value = "area-removed")>]
         AreaRemoved = 14
    
    | [<EnumMember(Value = "area-updated")>]
        AreaUpdated = 15
    
    | [<EnumMember(Value = "destination-out-sequence")>]
        DestinationOutSequence = 16
    
    | [<EnumMember(Value = "driver-arrived-early")>]
        DriverArrivedEarly = 17
    
    | [<EnumMember(Value = "driver-arrived-on-time")>]
        DriverArrivedOnTime = 18
    
    | [<EnumMember(Value = "driver-arrived-late")>]
        DriverArrivedLate = 19
    
    | [<EnumMember(Value = "geofence-entered")>]
        GeofenceEntered = 20
    
    | [<EnumMember(Value = "geofence-left")>]
        GeofenceLeft = 21

    | [<EnumMember(Value = "user_message")>]
        UserMessage = 22