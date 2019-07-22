namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type WorldCommand =
    | Output of GameResponse
    | Input of GameAction

type WorldState = {
    Commands: WorldCommand list
    Dungeon: Dungeon }

module World =
    let private init dungeon = 
        let obs = ObservableSource<GameResponse>()
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop state = async {
                    let! msg = inbox.Receive()
                    let command = CommandsParser.parse msg
                    let commands =
                        match command with
                        | Some x -> [ Input x ]
                        | None ->
                            [ Output <| Message "Invalid command, please retry"
                              Output <| Action(PlayerAction.Input) ]
                    return! stackCommands state commands }

                and stackCommands state commands = async {
                    return! processCommands {
                        state with 
                            Commands = commands |> List.append state.Commands } }

                and processCommands state = async {
                    match state.Commands with
                    | head::tail ->
                        match head with
                        | Input x -> return! explore { state with Commands = tail } x
                        | Output x -> return! sendResponse { state with Commands = tail } x
                    | [] -> return! loop state }

                and sendResponse state response = async {
                    obs.OnNext response
                    return! processCommands state }

                and explore state command = async {
                    match command with
                    | Move direction ->
                        let result = state.Dungeon |> Dungeon.move direction
                        let newState = { state with Dungeon = result.Value }
                        let commands = 
                            [ Output <| Message result.Message
                              Output <| Action(PlayerAction.Input) ]
                        return! stackCommands newState commands
                    | Exit ->
                        let commands =
                            if state.Dungeon |> Dungeon.canLeave then 
                                [ Output <| Message "You leave the dungeon successfully" ]
                            else
                                [ Output <| Message "You cannot leave the dungeon"
                                  Output <| Action(PlayerAction.Input) ]
                        return! stackCommands state commands
                    | _ -> return! processCommands state }

                loop {
                    Commands = []
                    Dungeon = dungeon }
        
        obs.AsObservable
        |> AsyncSeq.ofObservableBuffered
        |> AsyncSeq.merge (asyncSeq { yield Action(PlayerAction.Input) })

    let start dungeon = init dungeon