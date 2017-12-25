module Ethermine.Miner
open Configuration
open System
open System.Net.Http
open InfluxDB.Net
open Newtonsoft.Json
open System.Collections.Generic
open InfluxDB.Net.Models

type StatusData = { Time:int64; LastSeen:int64; ReportedHashRate:double; CurrentHashrate:double; ValidShares:int64; InvalidShares:int64; StaleShares:int64; AverageHashrate:double; ActiveWorkers:int64; Unpaid:Nullable<int64>; CoinsPerMin:double; UsdPerMin:double; BtcPerMin:double}
type StatusRecord = { Status:string; Data:StatusData} 

let private getCurrentStatsForMiner (minerhash:string) : Async<StatusRecord> = 
    async {
        let! value = (new HttpClient()).GetStringAsync(Uri("http://api.ethermine.org/miner/"+minerhash+"/currentStats")) |> Async.AwaitTask
        return JsonConvert.DeserializeObject<StatusRecord>(value)
    }

let private createTimeSeriesForMiner (asyncRecord:Async<StatusRecord>) : Async<Point> = 
    async {
        let! temp = asyncRecord
        return temp
            |> fun record -> 
                Point(Fields=Dictionary(dict<string,obj> 
                                 [("time", record.Data.LastSeen :> obj);
                                  ("ActiveWorkers",record.Data.ActiveWorkers :> obj);
                                  ("AverageHashrate",record.Data.AverageHashrate :> obj);
                                  ("BtcPerMin",record.Data.BtcPerMin :> obj) ;
                                  ("EtherPerMin",record.Data.CoinsPerMin :> obj);
                                  ("UsdPerMin",record.Data.UsdPerMin :> obj);
                                  ("StaleShares",record.Data.StaleShares :> obj);
                                  ("ValidShares", record.Data.ValidShares :> obj);
                                  ("InvalidShares", record.Data.InvalidShares :> obj);
                                  ("Unpaid", (if record.Data.Unpaid.HasValue then record.Data.Unpaid.Value else int64(0)) :> obj);
                                  ("CurrentHashrate", record.Data.CurrentHashrate:> obj)])
                     ,Precision = Enums.TimeUnit.Seconds
                     , Measurement = measurmentName)
    }

let createAndSaveTimeSeriesForMiner (hash:string)= 
    async { 
        let _client = InfluxDb <| getInfluxDbConfigurationTuple()
        let! serie = createTimeSeriesForMiner (getCurrentStatsForMiner hash) 
        return _client.WriteAsync (InfluxDbConfiguration.Database,serie,"")
    }

