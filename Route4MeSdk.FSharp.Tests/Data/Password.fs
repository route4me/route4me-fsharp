namespace Route4MeSdk.FSharp.Tests.Data

open System
open FsCheck

module Password =
    let random length =
        ['0'..'z']
        |> Gen.shuffle
        |> Gen.map(fun chars -> new String(chars))
        |> Gen.sample 1 1
        |> List.pick(fun item -> item.Substring(0, length) |> Some)
