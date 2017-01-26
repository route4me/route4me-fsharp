namespace Route4MeSdk.FSharp

open System
open FSharp.Data

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

    type User = {
        MemberId : int option
        AccountTypeId : int option
        MemberType : string
        MemberFirstName : string
        MemberLastName : string
        MemberEmail : string
        PhoneNumber : string
        ReadonlyUser : bool option
        ShowSuperuserAddresses : bool option }

    module User =
        ()