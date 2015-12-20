
module main
open System
open System.IO


let wl  s= Console.WriteLine( s.ToString())
let nl = System.Environment.NewLine;
let tab = "   "
let combined (slist:string list) =
             let Sb = new System.Text.StringBuilder()
             for i in (Array.ofList slist) do
                Sb.Append i |> ignore
                Sb.Append "  " |> ignore //multiple the spaces into tabs
             Sb.ToString()
                
                            
wl "go"

wl "Enter the path of the JS file"
let path = Console.ReadLine()
let chars = (File.ReadAllText path).ToCharArray() |> List.ofArray 



//the list is reveresed, keep that in mind, as head fucntions do FIFO
let rec Deminimize remainingList  (accumulator : string list) (currentIndents: string list) =
    let tabs = combined currentIndents
    match remainingList with 
    //alias the head and tail of the list
    | head :: tail -> 

            match head with  
                        //add a newline character and then the current indents
            | ';' ->    let appendMe = head.ToString() + nl  +  tabs
                        Deminimize (tail)   (appendMe ::accumulator) (currentIndents)           
                       
                       //add a tab to the set of current indents and add a  new line character, increase the current number of indents
            | '{' ->   let appendMe = nl + tabs + head.ToString() + nl + (combined (tab::currentIndents))  
                       Deminimize (tail)   (appendMe :: accumulator) (tab::currentIndents)

                        //remove a tab from the current indents, add a newline and the current indents
            | '}' ->    let appendMe = nl + (combined currentIndents.Tail)  + head.ToString() + nl  + tabs
                        Deminimize (tail)  (appendMe :: accumulator) (currentIndents.Tail)

                        //add the character to the list and recursively go to the next one
            | _ ->  let appendMe = (head.ToString()) 
                    Deminimize (tail)  (appendMe :: accumulator) (currentIndents)    


    // if its an empty list, return the accumulator
    | [] -> accumulator

let deminimized = (Deminimize chars [] []) |> List.rev


wl "Enter name of new file"
let name = Console.ReadLine ()

//remome the old name starting at the /
let slash = path.LastIndexOf "\\"
let newPath = (path.Remove slash) + "\\" + name + ".js" 
let stream = File.Create newPath
let textwriter = new StreamWriter (stream)

for i in deminimized do
    textwriter.Write i
    //wl ("writing " + i)

wl " Done"

stream.Close()
stream.Dispose()
