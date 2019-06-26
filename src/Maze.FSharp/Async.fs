// ----------------------------------------------------------------------------
// F# async extensions (Observable.fs)
// (c) Tomas Petricek, Phil Trelford, and Ryan Riley, 2011-2012, Available under Apache 2.0 license.
// ----------------------------------------------------------------------------

namespace Maze.FSharp

open System
open System.Threading

module Async =
    let synchronize f = 
      let ctx = System.Threading.SynchronizationContext.Current
      f (fun g ->
        let nctx = System.Threading.SynchronizationContext.Current 
        if (not (isNull ctx)) && ctx <> nctx then ctx.Post((fun _ -> g()), null)
        else g() )

    let AwaitObservable(observable : IObservable<'T1>) =
        let removeObj : IDisposable option ref = ref None
        let removeLock = new obj()
        let setRemover r = 
            lock removeLock (fun () -> removeObj := Some r)
        let remove() =
            lock removeLock (fun () ->
                match !removeObj with
                | Some d -> removeObj := None
                            d.Dispose()
                | None   -> ())
        synchronize (fun f ->
        let workflow =
            Async.FromContinuations((fun (cont,econt,ccont) ->
                let rec finish cont value =
                    remove()
                    f (fun () -> cont value)
                setRemover <|
                    observable.Subscribe
                        ({ new IObserver<_> with
                            member x.OnNext(v) = finish cont v
                            member x.OnError(e) = finish econt e
                            member x.OnCompleted() =
                                let msg = "Cancelling the workflow, because the Observable awaited using AwaitObservable has completed."
                                finish ccont (System.OperationCanceledException(msg)) })
                () ))
        async {
            let! cToken = Async.CancellationToken
            let token : CancellationToken = cToken
            use registration = token.Register((fun _ -> remove()), null)
            return! workflow
        })