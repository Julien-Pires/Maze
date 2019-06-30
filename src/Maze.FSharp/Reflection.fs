namespace Maze.FSharp

open Microsoft.FSharp.Reflection

[<AutoOpen>]
module Reflection =
    let toString (x: 'a) = 
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name

    let fromString<'a> ignoreCase (s: string) =
        let cases =
            FSharpType.GetUnionCases typeof<'a>
            |> Array.filter (fun case -> 
                if ignoreCase then case.Name.ToLower() = s.ToLower()
                else case.Name = s)
        match cases with
        |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
        |_ -> None