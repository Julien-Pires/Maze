namespace Maze.Engine

open System
open FParsec

type Parser<'a> = Parser<'a, unit>

module CommandsParser =
    let convert inputs =
        match inputs with
        | ["move"; direction] -> Move direction |> Some
        | _ -> None

    let parseWord : Parser<_> =
         many1Chars <| satisfy(fun c -> Char.IsLetter c)

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