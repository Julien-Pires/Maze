namespace Maze.Engine

open Maze.FSharp

module Title =
    let init channel =
        let rec loop () = async {
            channel.Post (Response <| Message "Please enter the path to the file that contains the dungeon to explore:")
            channel.Post (Response <| Action(PlayerAction.Input))
            let! path = channel.Receive()
            match path with
            | Entry path ->
                let! loadResult = Data.loadMap <| IO.readTextAsync path
                match loadResult with
                | Ok x -> ()
                | Error err -> channel.Post (Response <| Message err)
            return! loop()
        }
        loop()