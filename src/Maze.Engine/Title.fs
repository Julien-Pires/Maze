namespace Maze.Engine

open Maze.FSharp

module Title =
    let init channel =
        let rec loop () = async {
            channel.Post (SendResponse <| Message "Please enter the path to the file that contains the dungeon to explore:")
            channel.Post (SendResponse <| Action(PlayerAction.Input))
            let! path = channel.Receive()
            match path with
            | Entry path ->
                let! loadResult = Data.loadMap <| IO.readTextAsync path
                match loadResult with
                | Ok map ->
                    let character = ({ Name = "Player" }, { X = 0; Y = 0 })
                    channel.Post (ChangeWorld <| Dungeon.init map character)
                    return ()
                | Error err ->
                    channel.Post (SendResponse <| Message err)
                    return! loop()
        }
        loop()