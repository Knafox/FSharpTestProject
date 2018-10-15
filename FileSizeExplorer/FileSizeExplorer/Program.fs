namespace Program

open System 
open System.IO

module FileParser = 

    type File = 
       {
            Name: string
            Directory: string
            Extension: string
            Size: int64
       }

    type Directory = 
        {
            Path: string
            Name: string
            Files: seq<File>
            TotalSize: int64
            SubDirectory: seq<Directory>
        }

    let CreateFile (x:FileInfo) = 
        {   
            File.Name = x.FullName
            File.Directory = x.Directory.Name
            File.Extension = x.Extension
            File.Size = x.Length
        }

    let GetFileTotal x =
        try
        (
            x
            |> Seq.map (fun x -> x.Size) 
            |> Seq.reduce (fun acc f -> acc + f)
        )
        with
        | _ -> int64(0)

    let CreateDirectory (x:System.IO.DirectoryInfo) (y:seq<File>) (z:seq<Directory>) =
        {
             Directory.Path = x.FullName
             Directory.Name = x.Name
             Directory.Files = y
             Directory.TotalSize = GetFileTotal y
             Directory.SubDirectory = z
        }

    let TryOpenFile x =
        try
            FileInfo x
            |> ignore
            File.Exists x
        with
        | :? System.IO.IOException -> false
        | _ as exn -> raise exn

    let GetFilesInDirectory x = 
        Directory.EnumerateFiles x
        |> Seq.filter TryOpenFile
        |> Seq.map FileInfo
        |> Seq.map CreateFile

    
    let rec GetDirectories x =
        Directory.EnumerateDirectories x
        |> Seq.map (fun x -> CreateDirectory (DirectoryInfo x) (GetFilesInDirectory x) (GetDirectories x))
    
    let printFiles (files:seq<File>) =
        Seq.iter (fun (x:File) -> 
            printfn "File Name: %s, Directory: %s, Extention: %s, Size: %d" x.Name x.Directory x.Extension x.Size
        ) files

    let rec printDirectory (x:seq<Directory>) =
        Seq.iter (fun y ->
            printfn "Driectory Name: %s, Path: %s, TotalSize: %d" y.Name y.Path y.TotalSize
            printFiles y.Files
            printDirectory y.SubDirectory
        ) x
    
    let rec FileCount (x:seq<Directory>) =
        Seq.fold(fun dirAcc dir -> 
            dirAcc + (Seq.fold(fun acc file -> acc + 1) 0 dir.Files) + FileCount dir.SubDirectory            
        ) 0 x
    
    let rec DirectoryCount (x:seq<Directory>) = 
        Seq.fold(fun dirAcc dir -> 
            dirAcc + DirectoryCount dir.SubDirectory + 1           
        ) 0 x

    let rec Totalsize (x:seq<Directory>) =
        Seq.fold (fun dirAcc dir -> 
            dirAcc + dir.TotalSize + Totalsize dir.SubDirectory
        ) (int64(0)) x

    let rec Totalsize2 (x:seq<Directory>) =
        Seq.fold (fun dirAcc dir -> 
            dirAcc + Totalsize2 dir.SubDirectory +
            (Seq.fold(fun acc file -> acc + file.Size) 0L dir.Files) 
        ) (int64(0)) x

    let GetAllTypes (x:seq<Directory>) (types:list<String>) =
        Seq.iter (fun dir ->
            
        ) x

    [<EntryPoint>]
    let main argv =
        // get the files in a directory
        let y = GetFilesInDirectory @"C:\"
        printFiles y 
        //Console.ReadKey() |> ignore
        printfn ""

        //get all the files in a directory and sub directories
        let x = GetDirectories @"D:\GotHub"
        printDirectory x
        //Console.ReadKey() |> ignore
        printfn ""

        //get stats on files processed
        printfn "Total count of files is: %d! " (FileCount x)
        printfn "Total count of directories is: %d! " (DirectoryCount x)
        printfn "Total size of files processed is: %d! " (Totalsize x)
        Console.ReadKey() |> ignore
        0 // return an integer exit code



//    let listOfFilesInDirectory (givenDirectory:string)=
  //      let  listOfFiles=Directory.GetFiles  givenDirectory |> List.ofSeq
    //    listOfFiles |> List.map (fun fn -> 
      //      Path.GetFileName(fn), FileInfo(fn).Length)


      
//let rec GetDirectories x =
  //  Directory.EnumerateDirectories x
    //|> Seq.map (fun x -> CreateDirectory (DirectoryInfo x) (GetFilesInDirectory x) (GetDirectories x))


//let printFile (x:File) = 
  //  printfn "File Name: %s, Directory: %s, Extention: %s, Size: %d" x.Name x.Directory x.Extension x.Size

//let rec printDirectory (x:Directory) =
  //  printfn "Driectory Name: %s, Path: %s, TotalSize: %d" x.Name x.Path x.TotalSize
    //x.Files
    //|> Seq.iter printFile
//    x.SubDirectory
  //  |> Seq.iter printDirectory

//let y = GetDirectories @"C:\"
//y
//|> Seq.iter printDirectory