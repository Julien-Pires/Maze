namespace Maze

open System
open Maze.Engine

module CommandProcessor =
    let update =
        let agent = Agent.Start <| fun inbox ->
            let rec loop () = async {
                let! msg = inbox.Receive()
                match msg with
                | Response x -> printfn "%s" x
                | WaitInput -> Console.ReadLine() |> ignore
                return! loop() }
            loop()
        agent.Post