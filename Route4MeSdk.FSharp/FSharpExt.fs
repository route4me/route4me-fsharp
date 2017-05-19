namespace FSharpExt

open System

[<AutoOpen>]
module Prelude =
    /// A constant function, a function that always returns the same value. Used for unit input.
    let inline always value = fun () -> value

    /// A constant function, a function that always returns the same value regardless of what input you give.
    let inline always' value = fun _ -> value

    /// Disposes of any #IDisposable without first having to cast to IDisposable.
    let inline dispose (resource : #System.IDisposable) =
        if isNull <| box resource then ()
        else resource.Dispose()

    /// Applies value to the isValid guard function, calling the supplied
    /// pass function if the outcome is true otherwise the fail function.
    let inline guard isValid pass fail (value : 'T) : 'Outcome =
        if isValid value then pass value
        else fail value
        
    /// Tries to convert the value to T, returning Some T if possible otherwise None.
    let inline tryCast<'T> (value : obj) = 
        match value with
        | :? 'T as res -> Some res
        | _ -> None

module Option = 
    let inline substitute (forNone : unit -> 'Outcome) (forSome : 'Value -> 'Outcome) (option : Option<'Value>) = 
        match option with
        | None -> forNone()
        | Some value -> forSome value
    
    let inline andThen continuation option = option |> Option.bind (continuation)

    let inline ofPair(success : bool, value) =
        if success then Some value
        else None

[<AutoOpen>]
module OptionPervasive = 
    type OptionBuilder() =
        member this.Return(value) = Some value

        member this.ReturnFrom(m: 'T option) = m

        member this.Bind(m, f) = Option.bind f m

        member this.Zero() = None

        member this.Combine(m, f) = Option.bind f m

        member this.Delay(f: unit -> _) = f

        member this.Run(f) = f()

        member this.TryWith(m, handle) =
            try this.ReturnFrom(m)
            with e -> handle e

        member this.TryFinally(m, compensate) =
            try this.ReturnFrom(m)
            finally compensate()

        member this.Using(resource:#IDisposable, body) =
            this.TryFinally(body resource, fun () -> dispose resource)

        member this.While(guard, f) =
            if not (guard()) then Some() else
            do f() |> ignore

            this.While(guard, f)

        member this.For(sequence:seq<_>, body) =
            this.Using(sequence.GetEnumerator(), 
                fun enum -> this.While(enum.MoveNext, this.Delay(fun () -> body enum.Current)))

    let option = OptionBuilder()
        
module String =
    let inline join seperator (values:seq<_>) = 
        String.Join(seperator, values)

[<AutoOpen>]
module StringPervasive =
    let inline (|Blank|Present|) value =
        if String.IsNullOrWhiteSpace value then Blank
        else Present value

module Result = 
    let inline substitute (forError : 'TError -> 'Outcome) (forOk : 'T -> 'Outcome) result = 
        match result with
        | Ok v -> forOk v
        | Error e -> forError e
    
    let inline isOk result = result |> substitute (always' false) (always' true)
    let inline isError result = result |> substitute (always' true) (always' false)
    let inline asOk result = result |> substitute (always' None) Some
    let inline asError result = result |> substitute Some (always' None)

    let inline orElse (otherResult : Result<'T,'TErrorB>) (result : Result<'T,'TErrorA>) = 
        result |> substitute (always' otherResult) Ok

    let inline orElseWith (resultThunk : _ -> Result<'T,'TErrorB>) (result : Result<'T,'TErrorA>) = 
        result |> substitute resultThunk Ok
    
    let inline andThen continuation result = result |> substitute Error continuation
    
    let inline ofOptionWith errorThunk option =
        option
        |> Option.substitute (errorThunk >> Error) Ok

    let inline ofOption error option = 
        option |> ofOptionWith (always error)
    
    let inline toOption result = result |> asOk

    let inline iter action result = result |> substitute (always' ()) action
    let inline sink action result = result |> substitute action (always' ())
    let inline count result = result |> substitute (always' 0) (always' 1)