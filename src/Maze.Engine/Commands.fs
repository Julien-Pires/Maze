namespace Maze.Engine

open Maze.Engine

type UserAction =
    | Input

type InputCommand =
    | Move of Direction
    | Exit
    | GetPosition of AsyncReplyChannel<Position>

type OutputCommand =
    | UserAction of UserAction
    | Response of Message

type GameCommand<'a> =
    | Output of OutputCommand
    | Switch of (Channel<'a, 'a> -> IGameContext)

type Command =
    | Input of InputCommand
    | Output of OutputCommand

type CommandResult<'a> = {
    Value: 'a
    Message: Message }