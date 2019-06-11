namespace Maze.Engine

type Get =
    | Character of AsyncReplyChannel<Character>

type BoardCommand<'a> =
    | Action of Action
    | Get of Get

type BoardStatus = {
    Character: Character
    Map: Map }

module Board =
    let move (map : Map) character direction =
        let movement =
            match direction with
            | Direction.Up -> { X = 0; Y = -1 }
            | Direction.Down -> { X = 0; Y = 1 }
            | Direction.Left -> { X = -1; Y = 0 }
            | Direction.Right -> { X = 1; Y = 0 }
            | _ -> { X = 0; Y = 0 }
        let target =
            { X = character.Position.X + movement.X 
              Y = character.Position.Y + movement.Y }
        if map.CanMove character.Position target then 
            { Value = { character with Position = target }
              Message = "" }
        else
            { Value = character
              Message = "" }

type Board (character: Character, map: Map) =
    let agent = Agent.Start <| fun inbox ->
        let rec loop (board : BoardStatus) = async {
            let! msg = inbox.Receive();
            match msg with
            | Action action ->
                match action with
                | Move direction ->
                    let result = Board.move board.Map board.Character direction
                    return! loop { board with Character = result.Value }
            | Get x ->
                match x with
                | Character reply ->
                    reply.Reply board.Character
            return! loop board }
        loop { Character = character; Map = map }

    member __.Do action = agent.Post <| Action action

    member __.GetCharacter () = agent.PostAndReply(fun reply -> Character reply |> Get)