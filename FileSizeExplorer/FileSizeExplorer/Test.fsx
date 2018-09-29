open System 
open System.IO
open System.IO
open System.Linq

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

let CheckPermissions x = 
    let dirAccess = Directory.GetAccessControl(x, Security.AccessControl.AccessControlSections.Access)
    let indentity = @"INFOCORP\nathan.fox"
    [for rule in dirAccess.GetAccessRules(true, true, typeof<System.Security.Principal.NTAccount>) -> rule] ///workaround
    |> List.tryFind (fun rule -> rule.IdentityReference.Value = indentity)
    |> function // |> fun s -> match s with ....
        | None -> false
        | Some _ -> true



let result = CheckPermissions @"D:\GotHub\FSharp\Examples\Tutorial\Tutorial\Tutorial.fsx"
CheckPermissions @"C:\Windows\System32\Calc.exe"        

let TryOpenFile x =
    try 
        FileInfo >> CreateFile
    with
    | :? System.IO.IOException -> ignore
    | _ as exn ->
        raise exn 

let GetFilesInDirectory x = 
    Directory.EnumerateFiles x
    |> Seq.filter File.Exists //try to open file and wrap in try catch. true if success and false if failed.
    |> Seq.map TryOpenFile 
    |> Seq.iter (fun x -> printfn "File: %s, of Type: %s, is %d bytes " x.Name x.Extension x.Size)

let rec GetDirectories x =
    Directory.EnumerateDirectories x
    |> Seq.iter (fun x -> 
        GetFilesInDirectory x
        //printfn "%s" x
        GetDirectories x
    )

GetDirectories @"C:\Users\nathan.fox\AppData\Local\Application Data"


//Fix Exception Issue
//Get Total size of directory and %s


