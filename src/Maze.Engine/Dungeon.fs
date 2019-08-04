namespace Maze.Engine

type Dungeon =
    { Character: Character * Position
      Map: Map }

module Dungeon =
    let move direction position map =
        let movement =
            match direction with
            | Forward -> { X = 0; Y = 1 }
            | Backward -> { X = 0; Y = -1 }
            | Left -> { X = -1; Y = 0 }
            | Right -> { X = 1; Y = 0 }
        let target =
            { X = position.X + movement.X 
              Y = position.Y + movement.Y }
        if map |> Map.canMove position target then
            let message =
                if map |> Map.hasReachedExit target then 
                    "You moved to a new room, you can exit the dungeon"
                else 
                    "You moved to a new room"
            { Value = target
              Message = message }
        else
            { Value = position
              Message = "You cannot move further" }

    let canLeave position map =
        map |> Map.hasReachedExit position
        
    let init map character channel =
        channel.Post <| SendResponse(Message "Welcome to the dungeon")
        
        let rec waitUser state =
            async {
                channel.Post <| SendResponse(Action(PlayerAction.Input))
                let! msg = channel.Receive()
                match msg with
                | Entry text ->
                    let command = CommandsParser.parse text
                    match command with
                    | Some x -> return! explore state x
                    | None ->
                        channel.Post <| SendResponse(Message "Invalid command, please retry")
                        return! waitUser state
            }

        and explore state command =
            async {
                let newState = 
                    match command with
                    | Move direction ->
                        let (character, position) = state.Character
                        let newPosition = move direction position state.Map
                        channel.Post <| SendResponse(Message newPosition.Message)
                        { state with Character = (character, newPosition.Value) }
                    | Exit ->
                        if state.Map |> canLeave (snd state.Character) then 
                            channel.Post <| SendResponse(Message "You leave the dungeon successfully")
                        else
                            channel.Post <| SendResponse(Message "You cannot leave the dungeon")
                        state
                    | _ -> state
                return! waitUser newState
            }

        waitUser {
            Character = character
            Map = map }