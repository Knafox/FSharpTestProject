
/// F# Examples
/// From


namespace Examples
open System

module TailRecursion = 

    /// Exampe of Factorial without Tail recursion
    let rec Fact x = 
        if x = 1 then
            1
        else
            Fact (x-1) * x
    

    let factTail x =
        let rec factacc acc x =
            if x = 1 then
                acc
            else
                factacc (acc*x) (x-1)
                
        factacc 1 x

    factTail 3

/// Not tail recusrive
    let fib x =
        if x = 0 then
            0
        else if x = 1 then
            1
        else fib (x-1) + fib (x-2) 

    fib 4

// Not sure how to tai resurive this functio 
    let fibtail x =
       1
    





#if COMPILED
module BoilerPlateForForm = 
    [<System.STAThread>]
    do ()
    do System.Windows.Forms.Application.Run()
#endif
