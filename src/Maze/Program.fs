open System
open FSharp.Control
open Maze.Engine

[<EntryPoint>]
let main argv =
    let game = Game.start()
    game.Output
    |> AsyncSeq.iter (function
        | UserAction action ->
            match action with
            | UserAction.Input -> Console.ReadLine() |> game.Input
        | Response message -> printfn "%s" message)
    |> Async.RunSynchronously      
    
    0 // return an integer exit code