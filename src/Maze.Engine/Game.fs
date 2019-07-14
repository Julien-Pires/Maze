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
        
        contextOutput.Output
        |> AsyncSeq.iter (fun cmd ->
            match cmd with
            | Switch _ -> ()
            | GameCommand.Output out -> gameOutput.Input out)
        |> Async.Start
        
        playerInput.Output
        |> AsyncSeq.iter (fun c ->
            status.Context.Update c)
        |> Async.Start

        Channel.createFrom playerInput gameOutput