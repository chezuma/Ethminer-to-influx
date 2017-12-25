module Ethermine.Worker
open Configuration
open System
open System.Net.Http
open InfluxDB.Net
open Newtonsoft.Json
open System.Collections.Generic
open InfluxDB.Net.Models

type StatusData = { Time:int64; LastSeen:int64; ReportedHashRate:double; CurrentHashrate:double; ValidShares:int64; InvalidShares:int64; StaleShares:int64; AverageHashrate:double; ActiveWorkers:int64; Unpaid:int64; CoinsPerMin:double; UsdPerMin:double; BtcPerMin:double; Worker:string}
type StatusRecord = { Data:seq<StatusData> }

let private getCurrentStatForWorkers (minerhash:string) : Async<StatusRecord> = 
    async {
        let! value = (new HttpClient()).GetStringAsync(Uri("http://api.ethermine.org/miner/"+minerhash+"/workers")) |> Async.AwaitTask
        return JsonConvert.DeserializeObject<StatusRecord>(value)
    }

let private createTimeSeriesForWorkers (asyncRecord:Async<StatusRecord>) : Async<seq<Point>> = 
    async {
        let! temp = asyncRecord
        printfn "%s" (JsonConvert.SerializeObject temp)
        return temp.Data |> Seq.map(fun record ->
                Point(Fields=Dictionary(dict<string,obj> 
                                 [("time", record.LastSeen :> obj);
                                  ("ActiveWorkers",record.ActiveWorkers :> obj);
                                  ("AverageHashrate",record.AverageHashrate :> obj);
                                  ("BtcPerMin",record.BtcPerMin :> obj) ;
                                  ("EtherPerMin",record.CoinsPerMin :> obj);
                                  ("UsdPerMin",record.UsdPerMin :> obj);
                                  ("StaleShares",record.StaleShares :> obj);
                                  ("ValidShares", record.ValidShares :> obj);
                                  ("InvalidShares", record.InvalidShares :> obj);
                                  ("CurrentHashrate", record.CurrentHashrate:> obj);
                                  ("Name", record.Worker :> obj)])
                     ,Precision = Enums.TimeUnit.Seconds
                     , Measurement = measurmentName + "worker/"+  record.Worker))
        }

let createAndSaveTimeSeriesForWorkers (hash:string)= 
    async {
        let _client = InfluxDb <| getInfluxDbConfigurationTuple()
        let! series = createTimeSeriesForWorkers (getCurrentStatForWorkers hash) 
        return series 
        |> Seq.map( fun currentSerie -> _client.WriteAsync (InfluxDbConfiguration.Database,currentSerie,""))
    }
