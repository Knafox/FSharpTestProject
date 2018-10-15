open System 
open System.IO
open System.Linq


let SeqCount (x:seq<int>) = 
    Seq.fold( fun acc x -> acc + 1 ) 0 x

let SeqSeqCount (x:seq<seq<int>>) =
    Seq.fold(fun dirAcc dir -> dirAcc + Seq.fold(fun acc file -> acc + 1) 0 dir ) 0 x

let seqseq = { 0 .. 100}

let x = SeqCount firstseq