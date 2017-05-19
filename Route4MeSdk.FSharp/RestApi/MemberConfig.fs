namespace Route4MeSdk.FSharp

open Newtonsoft.Json

[<CLIMutable>]
type MemberConfig = {
    [<JsonProperty("member_id")>]
    Id : int
        
    [<JsonProperty("config_key")>]
    Key : string

    [<JsonProperty("config_value")>]
    Value : string }