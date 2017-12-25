open System.Net.Http
open System
open System.Collections
open System.Collections.Generic

#r "System.Net.Http.dll"
#r "Newtonsoft.Json.dll"

let client = new HttpClient()
let address = "cc5ab6119817c4cce6d43956c31bd8c036d98429"
let url = "http://api.ethermine.org/miner/"+address+"/currentStats"

//client.BaseAddress = Uri(url)
let result = client.GetStringAsync(Uri(url)).Result
printfn "%s" result

type EthermineCurrentStatusData = { Time:int64; LastSeen:int64; ReportedHashRate:double; CurrentHashrate:double; ValidShares:int64; InvalidShares:int64; StaleShares:int64; AverageHashrate:double; ActiveWorkers:int64; Unpaid:int64; CoinsPerMin:double; UsdPerMin:double; BtcPerMin:double}
type EthermineCurrentStatusRecord = { Status:string; Data:EthermineCurrentStatusData} 

open Newtonsoft.Json
open System.Dynamic
let jsonString = "{'status':'OK','data':{'time':1513078200,'lastSeen':1513078195,'reportedHashrate':0,'currentHashrate':332666666.6666666,'validShares':289,'invalidShares':0,'staleShares':16,'averageHashrate':328560956.79012346,'activeWorkers':5,'unpaid':93480437648772050,'unconfirmed':null,'coinsPerMin':0.00003351545330358077,'usdPerMin':0.01790797700916928,'btcPerMin':0.0000010681374967851191}}"
let o = JsonConvert.DeserializeObject<EthermineCurrentStatusRecord>(jsonString)
let s = JsonConvert.SerializeObject (o, (Newtonsoft.Json.Formatting.Indented))

printfn "%s" s