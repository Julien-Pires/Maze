namespace Maze

open System

module Input =
    let readUntil text validate =
        let rec loop () =
            printfn "%s" text
            let text = Console.ReadLine()
            match validate text with
            | Ok x -> x
            | Error x ->
                printfn "%s" x
                loop()
        loop()