namespace Maze.Engine

open Maze.Engine

type PlayerAction =
    | Input

type PlayerResponse =
    | Entry of string

type GameAction =
    | Move of Direction
    | Exit
    | GetPosition of AsyncReplyChannel<Position>

type GameResponse =
    | Action of PlayerAction
    | Message of Message

type GameCommand =
    | Response of GameResponse
    | Switch of (Channel<PlayerResponse, GameCommand> -> Async<unit>)

type CommandResult<'a> = {
    Value: 'a
    Message: Message }