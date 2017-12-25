module Ethermine.Pool
// open Configuration
// open System
// open System.Net.Http
// open InfluxDB.Net
// open Newtonsoft.Json
// open System.Collections.Generic
// open InfluxDB.Net.Models

// type MinedBlock = { Number:int64; Miner:string; Time:int64 }
// type TopMiner = { Miner:string; HashRate:int64 }
// type PoolStatus = {HashRate:int64; Miners:int64; Workers:int64 }
// type CurrencyPrice = { Usd:double; Btc:double}
// type PoolStatusRecord = {MinedBlocks:seq<MinedBlock>; TopMiners:seq<TopMiner>; PoolStats:PoolStatus; Price:CurrencyPrice }

// let private getCurrentStatusForPool (minerhash:string) : Async<PoolStatusRecord> = 
//     async {
//         let! value = (new HttpClient()).GetStringAsync(Uri("http://api.ethermine.org/miner/"+minerhash+"/currentStats")) |> Async.AwaitTask
//         return JsonConvert.DeserializeObject<PoolStatusRecord>(value)
//     }

// let private createTimeSeriesForPool (asyncRecord:Async<PoolStatusRecord>) : Async<seq<Point>> = 
//     async {
//         let! temp = asyncRecord
        
//     }


// let createAndSaveTimeSeriesForMiner (hash:string)= 
//     //host 10.0.1.142:30001,root root,EthMiner
//     async { 
//         let _client = InfluxDb <| getInfluxDbConfigurationTuple()
//         let! serie = createTimeSeriesForPool (getCurrentStatusForPool hash) 
//         //EthMiner
//         return _client.WriteAsync (InfluxDbConfiguration.Database,serie,"")
//     }
