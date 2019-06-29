open System
open FSharp.Control
open Maze.Engine

[<EntryPoint>]
let main argv =
    let dungeon = {
        Character = { Name = "Indiana Jones"}, { X = 0; Y = 0 }
        Map = {
            Rooms = Map.ofList <| [
                ({ X = 0; Y = 0 }, { Type = Normal }) 
                ({ X = 0; Y = 1 }, { Type = Normal }) 
                ({ X = 0; Y = 2 }, { Type = Normal })
                ({ X = 0; Y = 3 }, { Type = Normal })
                ({ X = 0; Y = 4 }, { Type = Entrance }) ] 
        } 
    }
    let game = Game.init dungeon
    game.Responses
    |> AsyncSeq.iter (function
        | UserAction action ->
            match action with
            | UserAction.Input f -> Console.ReadLine() |> f
        | Response message -> printfn "%s" message)
    |> Async.RunSynchronously      
    
    0 // return an integer exit code