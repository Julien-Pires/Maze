namespace Maze.Engine

type WorldResult =
    | Response of string

type WorldCommand = {
    Command: string
    Reply: AsyncReplyChannel<WorldResult> }

type WorldResponse = WorldResult -> unit

type WorldState = {
    Commands: Command list
    Dungeon: Dungeon
    Response: WorldResponse option }

module World =
    let private init dungeon = 
        let agent =
            Agent.Start <| fun inbox ->
                let rec loop state = async {
                    let! msg = inbox.Receive()
                    let command = CommandsParser.parse msg.Command
                    match command with
                    | Some cmd -> return! queueCommand {
                        state with Response = Some (fun result -> msg.Reply.Reply result) } cmd
                    | None ->
                        msg.Reply.Reply <| Response "Invalid command, please retry"
                        return! loop state }

                and queueCommand state command = async {
                    return! dequeueCommand { state with Commands = command::state.Commands} }

                and dequeueCommand state = async {
                    match state.Commands with
                    | head::tail -> return! explore { state with Commands = tail } head
                    | [] -> return! loop state }

                and sendResult state result = async {
                    match (state.Response, result) with
                    | (Some reply), (Success x) -> reply <| Response x.Message
                    | (Some reply), (Failure x) -> reply <| Response x
                    | _ -> ()
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
                    Dungeon = dungeon
                    Response = None }

        fun command -> agent.PostAndReply (fun reply -> { Command = command; Reply = reply })       

    let start dungeon = init dungeon