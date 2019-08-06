namespace Maze.Engine

[<AutoOpen>]
module Socket =
    let postMessage text socket = socket.Post <| SendResponse(Message text)
    
    let postAction action socket = socket.Post <| SendResponse(Action action)