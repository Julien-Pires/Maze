// Learn more about F# at http://fsharp.org

open System
open Maze.Engine

[<EntryPoint>]
let main argv =
    let dungeon = {
        Character = { Name = "Indiana Jones"}, { X = 0; Y = 0 }
        Map = {
            Rooms = Map.ofList <| [
                ({ X = 0; Y = 0 }, Empty) 
                ({ X = 0; Y = 1 }, Empty) 
                ({ X = 0; Y = 2 }, Empty)
                ({ X = 0; Y = 3 }, Empty) ] 
        } 
    }
    let game = Game.Start dungeon
    let rec loop () =  async {
        let input = Console.ReadLine()
        let results = game.Input input
        match results with
        | Response x -> printfn "%s" x
        return! loop() }

    loop() |> Async.RunSynchronously
    0 // return an integer exit code
