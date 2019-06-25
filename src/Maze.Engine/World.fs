namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type WorldResponse =
    | UserAction of UserAction
    | Result of CommandExecutionResult

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
                        obs.OnNext <| Result(Error "Invalid command, please retry")
                        obs.OnNext <| UserAction(Input inbox.Post)
                        return! loop state }

                and queueCommand state command = async {
                    return! dequeueCommand { state with Commands = command::state.Commands} }

                and dequeueCommand state = async {
                    match state.Commands with
                    | head::tail -> return! explore { state with Commands = tail } head
                    | [] -> return! loop state }

                and sendResult state result = async {
                    match (result) with
                    | Ok x -> obs.OnNext <| Result(Ok x.Message)
                    | Error x -> obs.OnNext <| Result(Error x)
                    obs.OnNext <| UserAction(Input inbox.Post)
                    return! loop state }

                and explore state command = async {
                    match command with
                    | Move direction ->
                        let result = state.Dungeon |> Dungeon.move direction
                        let newState =
                            match result with
                            | Ok x -> { state with Dungeon = x.Value }
                            | Error _ -> state
                        return! sendResult newState result }

                loop {
                    Commands = []
                    Dungeon = dungeon }
        
        obs.AsObservable
        |> AsyncSeq.ofObservableBuffered
        |> AsyncSeq.merge (asyncSeq { yield UserAction <| Input(agent.Post) })

    let start dungeon = init dungeon