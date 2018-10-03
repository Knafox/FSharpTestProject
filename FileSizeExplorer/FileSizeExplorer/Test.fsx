open System 
open System.IO
open System.Linq

let TryOpenFile x =
    try
        FileInfo x
        |> ignore
        true
    with
    | :? System.IO.IOException -> false
    | _ as exn -> raise exn

type File = 
   {
        Name: string
        Directory: string
        Extension: string
        Size: int64
   }

let CreateFile (x:FileInfo) = 
    {   
        File.Name = x.FullName
        File.Directory = x.Directory.Name
        File.Extension = x.Extension
        File.Size = x.Length
    }

let GetFilesInDirectory x = 
    Directory.EnumerateFiles x
    |> Seq.filter File.Exists
    |> Seq.map FileInfo
    |> Seq.map CreateFile

let rec GetDirectories x =
    Directory.EnumerateDirectories x
    |> Seq.iter (fun x -> 
        let dir = DirectoryInfo x 
        GetFilesInDirectory x
        |> CreateDirectory dir 
        printfn "%s" x
        GetDirectories x
    )

GetDirectories @"C:\"


//Fix Exception Issue
//Get Total size of directory and %s


