open System
open FSharp.Control
open Maze
open Maze.Engine

[<EntryPoint>]
let main argv =
    let map = Input.readUntil
                   "Please enter the path to the file that contains the dungeon to explore:"
                   (IO.readTextAsync >> (Data.loadMap >> Async.RunSynchronously))
    let game = Game.init map
    game.Responses
    |> AsyncSeq.iter (function
        | UserAction action ->
            match action with
            | UserAction.Input f -> Console.ReadLine() |> f
        | Response message -> printfn "%s" message)
    |> Async.RunSynchronously      
    
    0 // return an integer exit code