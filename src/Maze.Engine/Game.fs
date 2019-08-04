namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type Game =
    { Responses: Channel<string, GameResponse> }

type GameStatus =
    { World: World }

module Game =
    let start () =
        let worldChannel = Channel.create<GameCommand, PlayerResponse>()
        let playerChannel = Channel.create<PlayerResponse, GameResponse>()
        
        let agent = Agent.Start <| fun inbox ->
            let rec loop game =
                async {
                    let! (msg : obj) = inbox.Receive()
                    match msg with
                    | :? PlayerResponse as cmd -> worldChannel.EndpointB.Post cmd
                    | :? GameCommand as cmd ->
                        match cmd with
                        | ChangeWorld newWorld ->
                            game.World.Exit()
                            return! loop { game with World = World.create newWorld worldChannel.EndpointA }
                        | SendResponse response -> playerChannel.EndpointB.Post response
                    | _ -> ()
                    return! loop game
                }
            loop { World = World.create Title.init worldChannel.EndpointA }
            
        let rec gameLoop () =
            async {
                let! command = worldChannel.EndpointB.Receive()
                agent.Post command           
                return! gameLoop()
            }
        
        let rec playerLoop () =
            async {
                let! command = playerChannel.EndpointB.Receive()
                agent.Post command          
                return! playerLoop()
            }
        
        gameLoop() |> Async.Start
        playerLoop() |> Async.Start

        playerChannel.EndpointA