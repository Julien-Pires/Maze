namespace Maze

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
    | SendResponse of GameResponse
    | ChangeWorld of (Socket<GameCommand, PlayerResponse> -> Async<unit>)

type CommandResult<'a> =
    { Value: 'a
      Message: Message }