namespace D2.UserManagement

open D2.Common
open D2.UserManagement.Persistence
open Newtonsoft.Json
open System
open System.Text.RegularExpressions

module Registration =

    let private unicodeCharacters = "À-ÿ\p{L}\p{M}ÀàÂâÆæÇçÈèÉéÊêËëÎîÏïÔôŒœÙùÛûÜü«»€₣äÄöÖüÜß"

    let private symbolCharacters = @"!#%&'""=`{}~\.\-\+\*\?\^\|\/\$"

    let private emailPattern = String.Format(
                                   @"^([\w{0}{2}])+@{1}[\w{0}]+([-.][\w{0}]+)*\.[\w{0}]+([-.][\w{0}]+)*$",
                                   unicodeCharacters,
                                   "{1}",
                                   symbolCharacters
                               )

    let private loginPattern = new Regex("^[a-z_]+[0-9a-z_\\-]*$")

    let isValidEmailAddress (input : String) =
        let email = input.Trim();
        if email.Length > 0 then
            let valid = Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase) &&
                        not (email.StartsWith("-")) &&
                        not (email.StartsWith(".")) &&
                        not (email.EndsWith(".")) && 
                        not (email.Contains("..")) &&
                        not (email.Contains(".@")) &&
                        not (email.Contains("@."))
            valid
        else
            false
    
    let isValidLogin input = loginPattern.IsMatch input || isValidEmailAddress input

    let validateUser (user : UserRegistration) =
        if String.IsNullOrWhiteSpace(user.Login) then
            "Empty login"
        elif isValidLogin user.Login = false then
            "Invalid login name"
        elif String.IsNullOrWhiteSpace(user.EMail) then
            "Empty mail address"
        elif isValidEmailAddress user.EMail = false then
            "Invalid mail address"
        elif String.IsNullOrWhiteSpace(user.LastName) then
            "Empty last name"
        else
            ""
    
    let register (user : string) =
        let result = handle {
            let entry = JsonConvert.DeserializeObject<Mapper.UserRegistrationI>(user)
             
            match validateUser entry with
            | "" -> return CompositionRoot.Storage.register entry |> Async.RunSynchronously
            | m -> return! failExternal m
        }
        result ()

