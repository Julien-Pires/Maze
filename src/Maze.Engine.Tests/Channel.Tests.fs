namespace Maze.Engine.Tests

open Expecto
open FSharp.Control
open Swensen.Unquote
open Maze.Engine

module Channel_Tests =
    let config = FsCheckConfig.defaultConfig
    
    [<Tests>]
    let channelCreateTests =
        testList "Channel/create" [
            yield
                testPropertyWithConfig config "should create a channel that return the same value passed to the channel" <|
                    fun (value : int) ->
                        let sut = Channel.create()
                        let result = async {
                            do sut.Post value
                            return! sut.Receive() } |> Async.RunSynchronously
                        
                        test <@ result = value @>
        ]
        
    [<Tests>]
    let channelCreateFromTests =
        testList "Channel/createFrom" [
            yield
                testPropertyWithConfig config "should create a channel that post value to another channel" <|
                    fun (value : int) ->
                        let channel = Channel.create<_>()
                        let sut = Channel.createFrom channel channel
                        let result = async {
                            do sut.Post value
                            return! channel.Receive() } |> Async.RunSynchronously
                        
                        test <@ result = value @>
                        
            yield
                testPropertyWithConfig config "should create a channel that receive value from another channel" <|
                    fun (value : int) ->
                        let channel = Channel.create<_>()
                        let sut = Channel.createFrom channel channel
                        let result = async {
                            do channel.Post value
                            return! sut.Receive() } |> Async.RunSynchronously
                        
                        test <@ result = value @>
        ]