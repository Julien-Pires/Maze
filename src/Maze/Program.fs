open System
open FSharp.Control
open Maze.Engine
open Maze.FSharp

[<EntryPoint>]
let main argv =
    let dungeon = {
        Character = { Name = "Indiana Jones"}, { X = 0; Y = 0 }
        Map = {
            Rooms = Map.ofList <| [
                ({ X = 0; Y = 0 }, { Type = Normal }) 
                ({ X = 0; Y = 1 }, { Type = Normal }) 
                ({ X = 0; Y = 2 }, { Type = Normal })
                ({ X = 0; Y = 3 }, { Type = Normal }) ] 
        } 
    }
    let game = Game.init dungeon
    game.Result
    |> AsyncSeq.iter (function
        | UserAction action ->
            match action with
            | Input f -> Console.ReadLine() |> f
        | Result result ->
            match result with
            | Ok x -> printfn "%s" x
            | Error x -> printfn "%s" x)
    |> Async.RunSynchronously      
    
    0 // return an integer exit code