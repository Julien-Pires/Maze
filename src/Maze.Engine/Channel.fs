namespace Maze.Engine

open System.Threading
open FSharp.Control
    
type Channel<'a ,'b> = {
    Input : 'a -> unit
    Output : AsyncSeq<'b>
}
    
module Channel =
    let create<'a> () : Channel<'a, 'a> =
//        let cts = new CancellationTokenSource()
//        let agent = MailboxProcessor<'a>.Start((fun _ -> async.Return()), cancellationToken = cts.Token)
//        let asAsyncSeq = asyncSeq {
//            try
//                while true do
//                    let! msg = agent.Receive()
//                    yield msg
//            finally
//                cts.Cancel()
//        }
        {
            Input = fun (c : 'a) -> ()
            Output = asyncSeq { printfn "" }
        }
    
    let createFrom inputChannel outputChannel =
        { Input = inputChannel.Input
          Output = outputChannel.Output }