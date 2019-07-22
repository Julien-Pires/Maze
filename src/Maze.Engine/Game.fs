namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type Game = {
    Responses: Channel<string, GameResponse>
}

type GameStatus = {
    Context : IGameContext
}

module Game =
    let start () =
        let contextOutput = Channel.create<GameCommand<_>>()
        let gameOutput = Channel.create<GameResponse>()
        let playerInput = Channel.create<PlayerResponse>()
        let agent = Agent.Start <| fun inbox ->
            let rec loop game = async {
                let! (msg : obj) = inbox.Receive()
                match msg with
                | :? PlayerResponse as cmd ->
                    match cmd with
                    | Entry x -> game.Context.Update x
                | :? GameCommand<'a> as cmd ->
                    match cmd with
                    | Switch _ -> ()
                    | Response out -> gameOutput.Post out
                | _ -> ()
                return! loop game
            }
            loop { Context = Title.init contextOutput }
            
        let rec gameLoop () = async {
            let! command = contextOutput.Receive()
            agent.Post command           
            return! gameLoop()
        }
        
        let rec playerLoop () = async {
            let! command = playerInput.Receive()
            agent.Post command          
            return! playerLoop()
        }
        
        gameLoop() |> Async.Start
        playerLoop() |> Async.Start

        Channel.createFrom playerInput gameOutput