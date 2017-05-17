namespace Route4MeSdk.FSharp.Tests.Data

open System
open FsCheck

module Phone =
    let countryCodeGenerator = 
        Gen.choose(10, 99)
        |> Gen.map(sprintf "+%i")

    let areaCodeGenerator = 
        Gen.choose(10, 999)
        |> Gen.map string

    let localGenerator = 
        Gen.choose(1, 9999)
        |> Gen.map(sprintf "%4i")

    let generator = 
        Gen.zip3 countryCodeGenerator areaCodeGenerator localGenerator
        |> Gen.map(fun (a,b,c) -> sprintf "%s%s%s" a b c)

    let random() =
        generator
        |> Gen.sample 1 1
        |> List.head