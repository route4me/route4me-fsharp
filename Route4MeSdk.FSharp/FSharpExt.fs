namespace FSharpExt

open System

[<AutoOpen>]
module Prelude =
    /// A constant function, a function that always returns the same value. Used for unit input.
    let inline always value = fun () -> value

    /// A constant function, a function that always returns the same value regardless of what input you give.
    let inline always' value = fun _ -> value

    /// Disposes of any #IDisposable without first having to cast to IDisposable.
    let inline dispose (disposable : #System.IDisposable) =
        disposable.Dispose()

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
    
    let inline andThen f option = option |> Option.bind (f)
    let inline withDefault def (option : Option<'Value>) = option |> substitute (always def) id

    let inline ofPair(success : bool, value) =
        if success then Some value
        else None
        
[<AutoOpen>]
module StringPervasive =
    let inline (|Blank|Present|) value =
        if String.IsNullOrWhiteSpace value then Blank
        else Present value

type Result<'Ok, 'Error> = 
    | Ok of OkValue : 'Ok
    | Error of ErrorValue : 'Error

module Result = 
    let inline substitute (forError : 'Error -> 'Outcome) (forOk : 'Value -> 'Outcome) result = 
        match result with
        | Ok v -> forOk v
        | Error e -> forError e
    
    let inline isOk result = result |> substitute (always' false) (always' true)
    let inline isError result = result |> substitute (always' true) (always' false)
    let inline asOk result = result |> substitute (always' None) Some
    let inline asError result = result |> substitute Some (always' None)
    
    let inline bind result f = result |> substitute Error f
    let inline map f result = result |> substitute Error (f >> Ok)
    let inline formatError f result = result |> substitute (f >> Error) Ok
    let inline andThen f result = result |> substitute Error f
    
    let inline ofOption error option = 
        match option with
        | None -> Error error
        | Some v -> Ok v
    
    let inline toOption result = result |> asOk

    let inline iter f result = result |> substitute (always' ()) f
    let inline sink f result = result |> substitute f (always' ())
    let inline count result = result |> substitute (always' 0) (always' 1)
    
    let inline filter predicate result = 
        result |> substitute (always' None) (fun v -> 
                        if predicate v then Some v
                        else None)