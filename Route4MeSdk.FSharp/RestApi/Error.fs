namespace Route4MeSdk.FSharp

type Error = 
    | ValidationError of string
    | ApiError of StatusCode : int * Errors : string[]
