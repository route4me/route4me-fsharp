namespace Route4MeSdk.FSharp

open System

[<AutoOpen>]
module Utilities =
    let buildUrl (portions : string list) = String.Join("/", portions)
