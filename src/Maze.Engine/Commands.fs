namespace Maze.Engine

type UserAction =
    | Input of (string -> unit)

type InputCommand =
    | Move of Direction
    | Exit
    | GetPosition of AsyncReplyChannel<Position>

type OutputCommand =
    | UserAction of UserAction
    | Response of Message

type Command =
    | Input of InputCommand
    | Output of OutputCommand

type CommandResult<'a> = {
    Value: 'a
    Message: Message }