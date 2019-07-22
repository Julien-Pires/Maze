namespace Maze.Engine

open Maze.FSharp

module Title =
    let init (channel : Channel<_,_>) =
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop () = async {
                    channel.Post (Response <| Message "Please enter the path to the file that contains the dungeon to explore:")
                    channel.Post (Response <| Action(PlayerAction.Input))
                    let! path = inbox.Receive()
                    let! loadResult = Data.loadMap <| IO.readTextAsync path
                    match loadResult with
                    | Ok x -> ()
                    | Error err -> channel.Post (Response <| Message err)
                    return! loop()
                }
                loop()
        { new IGameContext
            with member __.Update value = agent.Post value }