namespace Maze.Engine

open System
open FParsec
open Maze.FSharp

type Parser<'a> = Parser<'a, unit>

module CommandsParser =
    let (|IsDirection|_|) = function
        | [ direction ] ->
            match direction |> fromString<Direction> true with
            | Some x -> Some x
            | None ->
                match direction.ToLower() with
                | "f" -> Some Forward
                | "b" -> Some Backward
                | "l" -> Some Left
                | "r" -> Some Right
                | _ -> None
        | _ -> None

    let convert inputs =
        match inputs with
        | IsDirection direction -> Some <| Move direction
        | [ "exit" ] -> Some Exit
        | _ -> None

    let parseWord : Parser<_> =
         many1Chars <| satisfy Char.IsLetter

    let parseAction : Parser<_> =
        many(spaces >>. parseWord .>> spaces)

    let (|IsAction|_|) input =
        let result = run parseAction input
        match result with
        | Success (result,_,_) -> convert result
        | _ -> None

    let parse = function
        | IsAction action -> Some action
        | _ -> None