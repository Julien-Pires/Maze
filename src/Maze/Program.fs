open System
open FSharp.Control
open Maze.Engine

[<EntryPoint>]
let main _ =
    async {
        let game = Game.start()
        let rec loop () = async {
            let! result = game.Receive()
            match result with
            | UserAction action ->
                match action with
                | UserAction.Input -> game.Post <| Console.ReadLine()
            | Response message -> printfn "%s" message
            
            return! loop()
        }
        
        return! loop()
    } |> Async.RunSynchronously   
    
    0 // return an integer exit code