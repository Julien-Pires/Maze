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

type GameCommand<'a> =
    | Response of GameResponse
    | Switch of (Channel<'a, 'a> -> IGameContext)

type CommandResult<'a> = {
    Value: 'a
    Message: Message }