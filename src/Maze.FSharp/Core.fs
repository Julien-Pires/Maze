namespace Maze.FSharp

type Agent<'a> = MailboxProcessor<'a>

type Result<'a, 'b> =
    | Success of 'a
    | Failure of 'b