namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type WorldCommand = string

type WorldState = {
    Commands: Command list
    Dungeon: Dungeon }

module World =
    let private init dungeon = 
        let obs = ObservableSource<OutputCommand>()
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop state = async {
                    let! msg = inbox.Receive()
                    let command = CommandsParser.parse msg
                    let commands =
                        match command with
                        | Some x -> [ Input x ]
                        | None ->
                            [ Output <| Response "Invalid command, please retry"
                              Output <| UserAction(UserAction.Input) ]
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
                            [ Output <| Response result.Message
                              Output <| UserAction(UserAction.Input) ]
                        return! stackCommands newState commands
                    | Exit ->
                        let commands =
                            if state.Dungeon |> Dungeon.canLeave then 
                                [ Output <| Response "You leave the dungeon successfully" ]
                            else
                                [ Output <| Response "You cannot leave the dungeon"
                                  Output <| UserAction(UserAction.Input) ]
                        return! stackCommands state commands
                    | _ -> return! processCommands state }

                loop {
                    Commands = []
                    Dungeon = dungeon }
        
        obs.AsObservable
        |> AsyncSeq.ofObservableBuffered
        |> AsyncSeq.merge (asyncSeq { yield UserAction(UserAction.Input) })

    let start dungeon = init dungeon