namespace Analysis

open Vibrant.InfluxDB.Client.Rows
open System.Collections

module ExampleFunctions =
    open System
    let Mean (array: double[]) = 
        let mutable s = 0.0
        for x in array do
            s <- s + x
        s/(double)array.Length


        
