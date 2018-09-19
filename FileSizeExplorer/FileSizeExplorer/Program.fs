namespace Program

open System 
open System.IO

module FileParser = 

//Outdated use fileINFO Type type for 
    type File = {
        Name: String;
        Directory: String;
        Extendsion: String;
        Size: int;
    } 

//Directory and Files Pattern Match?



    let GetFilesInDirectory d =
        Directory.GetFiles d

    let PrintFilesNames fileNames = 
        for x in fileNames do
            printfn "%s" x

    let GetFileInfo name =
        let x = FileInfo(name)
        printfn "File: %s, is %d bytes " x.Name x.Length

    let GetFilesAndSize d =
        Directory.GetFiles d
        |> Array.map ( fun x -> FileInfo(x) )
        |> Array.sortBy ( fun x -> x.Length )
        |> Array.iter ( fun x -> printfn "File: %s, is %d bytes " x.Name x.Length )


    [<EntryPoint>]
    let main argv = 
        //Test 1: get the files in a directory
        let listoffiles = GetFilesInDirectory @"D:\GotHub\FSharp\FileSizeExplorer\FileSizeExplorer\bin\Debug"
        PrintFilesNames listoffiles
        Console.ReadKey() |> ignore
        //Test 2: get the size of a specific file
        GetFileInfo @"D:\AlcentraBackUp"
        Console.ReadKey() |> ignore
        //Test 3: get the largest file from a list
        GetFilesAndSize @"D:\GotHub\FSharp\FileSizeExplorer\FileSizeExplorer\bin\Debug"
        Console.ReadKey() |> ignore

        0 // return an integer exit code



//    let listOfFilesInDirectory (givenDirectory:string)=
  //      let  listOfFiles=Directory.GetFiles  givenDirectory |> List.ofSeq
    //    listOfFiles |> List.map (fun fn -> 
      //      Path.GetFileName(fn), FileInfo(fn).Length)