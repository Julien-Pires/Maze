namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type CommandExecutionResult =
    | Success of string
    | Failure of string

type UserAction =
    | Input of (string -> unit)

type WorldResponse =
    | UserAction of UserAction
    | CommandResult of CommandExecutionResult

type WorldCommand = string

type WorldState = {
    Commands: Command list
    Dungeon: Dungeon }

module World =
    let private init dungeon = 
        let obs = ObservableSource<WorldResponse>()
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop state = async {
                    let! msg = inbox.Receive()
                    let command = CommandsParser.parse msg
                    match command with
                    | Some cmd -> return! queueCommand state cmd
                    | None ->
                        obs.OnNext <| CommandResult(Failure "Invalid command, please retry")
                        obs.OnNext <| UserAction(Input(inbox.Post))
                        return! loop state }

                and queueCommand state command = async {
                    return! dequeueCommand { state with Commands = command::state.Commands} }

                and dequeueCommand state = async {
                    match state.Commands with
                    | head::tail -> return! explore { state with Commands = tail } head
                    | [] -> return! loop state }

                and sendResult state result = async {
                    match (result) with
                    | Result.Success x -> obs.OnNext <| CommandResult(Success x.Message)
                    | Result.Failure x -> obs.OnNext <| CommandResult(Failure x)
                    obs.OnNext <| UserAction(Input(inbox.Post))
                    return! loop state }

                and explore state command = async {
                    match command with
                    | Move direction ->
                        let result = state.Dungeon |> Dungeon.move direction
                        let newState =
                            match result with
                            | Result.Success x -> { state with Dungeon = x.Value }
                            | Result.Failure _ -> state
                        return! sendResult newState result }

                loop {
                    Commands = []
                    Dungeon = dungeon }
        
        obs.AsObservable
        |> AsyncSeq.ofObservableBuffered
        |> AsyncSeq.merge (asyncSeq { yield UserAction(Input(agent.Post)) })

    let start dungeon = init dungeon