namespace Program

open System 
open System.IO

module FileParser = 

    /// Possibly could use the given FileInfo System Type
    type File = {
        Name: string;
        Directory: string;
        Extension: string;
        Size: double;
    } 


//Directory and Files and extention  Pattern Match?

    let GetFilesInDirectory x = 
        Directory.EnumerateFiles x
        |> Seq.map ( fun x -> FileInfo(x) )
        |> Seq.filter ( fun x -> File.Exists(x.FullName ))
        |> Seq.filter ( fun x -> x.Length > int64(10) )
        |> Seq.iter ( fun x -> printfn "File: %s, of Type: %s, is %d bytes " x.Name x.Extension x.Length )

    let rec GetDirectories x =
        Directory.EnumerateDirectories x
        |> Seq.iter( fun x -> 
            GetFilesInDirectory x
            printfn "%s" x
            GetDirectories x)



    [<EntryPoint>]
    let main argv =
        GetDirectories @"D:\Gothub\"

        0 // return an integer exit code



//    let listOfFilesInDirectory (givenDirectory:string)=
  //      let  listOfFiles=Directory.GetFiles  givenDirectory |> List.ofSeq
    //    listOfFiles |> List.map (fun fn -> 
      //      Path.GetFileName(fn), FileInfo(fn).Length)