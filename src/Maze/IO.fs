namespace Maze

open System.IO

module IO =
    let readTextAsync path = async {
        if File.Exists path then
            let! text = File.ReadAllTextAsync(path) |> Async.AwaitTask
            return Ok text
        else
            return Error "Failed to read the file, the file does not exist"
    }