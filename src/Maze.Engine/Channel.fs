namespace Maze.Engine

open System.Threading
open FSharp.Control

type Post<'a> = 'a -> unit

type Receive<'a> = unit -> Async<'a>

type Socket<'a, 'b> =
    { Post: Post<'a>
      Receive: Receive<'b> }

type Channel<'a, 'b> =
    { EndpointA: Socket<'a, 'b>
      EndpointB: Socket<'b, 'a>
      Close: unit -> unit }

module Channel =
    let create<'a, 'b> () =
        let cts = new CancellationTokenSource()
        let socketA = MailboxProcessor<'a>.Start((fun _ -> async.Return()), cts.Token)
        let socketB = MailboxProcessor<'b>.Start((fun _ -> async.Return()), cts.Token)
        {
            EndpointA =
                { Post = fun c -> socketA.Post c
                  Receive = fun _ -> socketB.Receive() }
            EndpointB =
                { Post = fun c -> socketB.Post c
                  Receive = fun _ -> socketA.Receive() }
            Close = fun _ -> cts.Cancel()
        }