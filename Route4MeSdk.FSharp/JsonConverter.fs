namespace Route4MeSdk.FSharp

open System
open Microsoft.FSharp.Reflection
open Newtonsoft.Json
open Newtonsoft.Json.Converters

module JsonConverter =
    type Option() =
        inherit JsonConverter()
    
        override x.CanConvert(t) = 
            t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<option<_>>

        override x.WriteJson(writer, value, serializer) =
            let value = 
                if value = null then null
                else 
                    let _,fields = FSharpValue.GetUnionFields(value, value.GetType())
                    fields.[0]  
            serializer.Serialize(writer, value)

        override x.ReadJson(reader, t, existingValue, serializer) =        
            let innerType = t.GetGenericArguments().[0]
            let innerType = 
                if innerType.IsValueType then (typedefof<Nullable<_>>).MakeGenericType([|innerType|])
                else innerType        
            let value = serializer.Deserialize(reader, innerType)
            let cases = FSharpType.GetUnionCases(t)
            if value = null then FSharpValue.MakeUnion(cases.[0], [||])
            else FSharpValue.MakeUnion(cases.[1], [|value|])

    type Boolean() =
        inherit JsonConverter()
    
        override __.CanConvert(objectType) = 
            objectType = typeof<bool> || objectType = typeof<Nullable<bool>>

        override __.CanWrite 
            with get() = false

        override __.WriteJson(writer, value, serializer) =
            raise <| new NotImplementedException()

        override __.ReadJson(reader, objectType, existingValue, serializer) =
            let value = reader.Value.ToString().ToLower().Trim();
            match value with
                | "true"
                | "yes"
                | "y"
                | "1" -> box true
                | _ -> box false

    type ZerosIsoDateTime() =
        inherit JsonConverter()

        let zeroData = "0000-00-00"

        override __.CanConvert(objectType) = 
            objectType = typeof<DateTime> || objectType = typeof<Nullable<DateTime>>

        override __.CanWrite 
            with get() = false

        override __.WriteJson(writer, value, serializer) =
            raise <| new NotImplementedException()

        override self.ReadJson(reader, objectType, existingValue, serializer) =
            if reader.Value = null || reader.Value.ToString() = zeroData then 
                if objectType = typeof<Nullable<DateTime>> then 
                    new Nullable<DateTime>() :> _
                else box DateTime.MinValue
            else serializer.Deserialize(reader, objectType)

