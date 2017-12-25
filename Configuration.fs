module Configuration
open System;

let measurmentName = Environment.GetEnvironmentVariable("MeasurementName")
let minerhash = Environment.GetEnvironmentVariable("Minerhash")

type DatabaseConfigurationRecord = {Host:string;Database:string;Username:string;Password:string}

let InfluxDbConfiguration = {
    Host=Environment.GetEnvironmentVariable("InfluxDbHost"); 
    Database = Environment.GetEnvironmentVariable("InfluxDatabase");
    Username= Environment.GetEnvironmentVariable("InfluxDbUsername");
    Password = Environment.GetEnvironmentVariable("InfluxDbPassword")
}

let getInfluxDbConfigurationTuple() = 
    (InfluxDbConfiguration.Host, InfluxDbConfiguration.Username, InfluxDbConfiguration.Password)