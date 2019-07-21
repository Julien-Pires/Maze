namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type Game = {
    Responses: Channel<string, OutputCommand>
}

type GameStatus = {
    Context : IGameContext
}

module Game =
    let start () =
        let contextOutput = Channel.create<GameCommand<_>>()
        let gameOutput = Channel.create<OutputCommand>()
        let playerInput = Channel.create<string>()
        let status = { Context = Title.init contextOutput }
        
        let rec gameLoop () = async {
            let! command = contextOutput.Receive()
            match command with
            | Switch _ -> ()
            | GameCommand.Output out -> gameOutput.Post out
            
            return! gameLoop()
        }
        
        let rec playerLoop () = async {
            let! input = playerInput.Receive()
            status.Context.Update input
            
            return! playerLoop()
        }
        
        gameLoop() |> Async.Start
        playerLoop() |> Async.Start

        Channel.createFrom playerInput gameOutput