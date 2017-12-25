// Learn more about F# at http://fsharp.org
module GrafanaTest
open Configuration
open System
open System.Threading
open Ethermine.Miner
open Ethermine.Worker

[<EntryPoint>]
let rec main args  =
    printfn "%s" (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString())
    
    createAndSaveTimeSeriesForMiner minerhash   
    |> Async.RunSynchronously
    |> fun x -> (printfn "Body:%s\r\mCode:%s\r\nSucces:%s\r\n" x.Result.Body (x.Result.StatusCode.ToString()) (x.Result.Success.ToString()))

    createAndSaveTimeSeriesForWorkers minerhash
    |> Async.RunSynchronously
    |> Seq.iter(fun x -> (printfn "Body:%s\r\mCode:%s\r\nSucces:%s\r\n" x.Result.Body (x.Result.StatusCode.ToString()) (x.Result.Success.ToString())))
    
    printfn "run done"
    printfn "%s" (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString())
    Thread.Sleep (60000 * 10) 
    main ([||]) |> ignore
    0
    

    