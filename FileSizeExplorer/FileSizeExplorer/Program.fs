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
        }

    let CreateFile (x:FileInfo) = 
        {   
            File.Name = x.FullName
            File.Directory = x.Directory.Name
            File.Extension = x.Extension
            File.Size = x.Length
        }

    let GetFileTotal x =
        x
        |> Seq.map (fun x -> x.Size) 
        |> Seq.reduce (fun acc f -> acc + f)

    let CreateDirectory (x:System.IO.DirectoryInfo) (y:seq<File>) =
        {
             Directory.Path = x.FullName
             Directory.Name = x.Name
             Directory.Files = y
             Directory.TotalSize = GetFileTotal y
        }

    let TryOpenFile x =
        try
            FileInfo x
            |> ignore
            true
        with
        | :? System.IO.IOException -> false
        | _ as exn -> raise exn

    let GetFilesInDirectory x = 
        Directory.EnumerateFiles x
        |> Seq.filter File.Exists
        |> Seq.map FileInfo
        |> Seq.map CreateFile

    let rec GetDirectories x =
        Directory.EnumerateDirectories x
        |> Seq.iter (fun x -> 
            let dir = DirectoryInfo x 
            let DirList = GetFilesInDirectory x >> CreateDirectory dir 
            printfn "%s" x
            GetDirectories x
        )

    let GetFilesInDirectory x = 
        Directory.EnumerateFiles x
        |> Seq.filter TryOpenFile 
        |> Seq.map FileInfo >> CreateFile
        //|> Seq.iter (fun (x:File) -> printfn "File: %s, of Type: %s, is %d bytes " x.Name x.Extension x.Size)

    let rec GetDirectories x =
        Directory.EnumerateDirectories x
        |> Seq.iter (fun x -> 
            GetFilesInDirectory x
            printfn "%s" x
            GetDirectories x
        )

    [<EntryPoint>]
    let main argv =
        GetDirectories @"D:\Gothub\"

        0 // return an integer exit code



//    let listOfFilesInDirectory (givenDirectory:string)=
  //      let  listOfFiles=Directory.GetFiles  givenDirectory |> List.ofSeq
    //    listOfFiles |> List.map (fun fn -> 
      //      Path.GetFileName(fn), FileInfo(fn).Length)