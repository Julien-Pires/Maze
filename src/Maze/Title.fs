namespace Maze

open Maze.FSharp

module Title =
    let init channel =
        let rec loop () =
            async {
                channel |> postMessage "Please enter the path to the file that contains the dungeon to explore:"
                channel |> postAction PlayerAction.Input
                let! path = channel.Receive()
                match path with
                | Entry path ->
                    let! loadResult = Data.loadMap <| IO.readTextAsync path
                    match loadResult with
                    | Ok map ->
                        let character = ({ Name = "Player" }, { X = 0; Y = 0 })
                        channel.Post (ChangeWorld <| Dungeon.init map character)
                    | Error err ->
                        channel |> postMessage err
                        return! loop()
            }
        loop()