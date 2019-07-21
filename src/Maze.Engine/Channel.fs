namespace Maze.Engine

open System.Threading
open FSharp.Control

type ChannelPost<'a> = 'a -> unit

type ChannelReceive<'a> = unit -> Async<'a>

type Channel<'a ,'b> = {
    Post : ChannelPost<'a>
    Receive : ChannelReceive<'b>
}
    
module Channel =
    let create<'a> () : Channel<'a, 'a> =
        let cts = new CancellationTokenSource()
        let agent = MailboxProcessor<'a>.Start((fun _ -> async.Return()), cancellationToken = cts.Token)
        {
            Post = fun c -> agent.Post c
            Receive = fun _ -> agent.Receive()
        }
    
    let createFrom inputChannel outputChannel =
        { Post = inputChannel.Post
          Receive = outputChannel.Receive }