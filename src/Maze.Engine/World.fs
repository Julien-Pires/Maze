namespace Maze.Engine

open FSharp.Control
open Maze.FSharp

type WorldResult =
    | WaitInput
    | Response of string

type WorldCommand = string

type WorldResponse = WorldResult -> unit

type WorldState = {
    Commands: Command list
    Dungeon: Dungeon }

module World =
    let private init dungeon = 
        let obs = ObservableSource()
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop state = async {
                    let! msg = inbox.Receive()
                    let command = CommandsParser.parse msg
                    match command with
                    | Some cmd -> return! queueCommand state cmd
                    | None ->
                        obs.OnNext <| Response "Invalid command, please retry"
                        obs.OnNext WaitInput
                        return! loop state }

                and queueCommand state command = async {
                    return! dequeueCommand { state with Commands = command::state.Commands} }

                and dequeueCommand state = async {
                    match state.Commands with
                    | head::tail -> return! explore { state with Commands = tail } head
                    | [] -> return! loop state }

                and sendResult state result = async {
                    match (result) with
                    | Success x -> obs.OnNext <| Response x.Message
                    | Failure x -> obs.OnNext <| Response x
                    obs.OnNext WaitInput
                    return! loop state }

                and explore state command = async {
                    match command with
                    | Move direction ->
                        let result = state.Dungeon |> Dungeon.move direction
                        let newState =
                            match result with
                            | Success x -> { state with Dungeon = x.Value }
                            | Failure _ -> state
                        return! sendResult newState result }

                loop {
                    Commands = []
                    Dungeon = dungeon }
        
        let resultSeq =
            obs.AsObservable
            |> AsyncSeq.ofObservableBuffered
            |> AsyncSeq.merge (asyncSeq { yield WaitInput })
        (agent.Post, resultSeq)

    let start dungeon = init dungeon