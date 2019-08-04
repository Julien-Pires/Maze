namespace Maze.Engine

open System.Threading
open Maze.FSharp

type World =
    { Exit: unit -> unit }

module World =
    let create world channel =
        let cts = new CancellationTokenSource()
        Async.Start(world channel, cts.Token)
        { Exit = fun _ -> cts.Cancel() }