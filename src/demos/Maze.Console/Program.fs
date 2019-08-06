open System
open FSharp.Control
open Maze

[<EntryPoint>]
let main _ =
    async {
        let game = Game.start()
        let rec loop () = async {
            let! result = game.Receive()
            match result with
            | Action action ->
                match action with
                | Input -> game.Post <| Entry(Console.ReadLine())
            | Message msg -> printfn "%s" msg
            
            return! loop()
        }
        
        return! loop()
    } |> Async.RunSynchronously   
    
    0 // return an integer exit code