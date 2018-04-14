namespace D2.Administration

module Registration =
    open D2.Common
    open D2.UserManagement.Persistence
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.Logging
    open RestSharp
    open RestSharp.Authenticators
    open System
    open System.IO
    
    let generateWelcomeMailText (configuration : ServiceConfiguration.EMailProperties) (item : UserRegistration) =
        File.ReadAllText(Path.Combine(configuration.Directory, configuration.AcceptRegistration.File) + ".txt")
            .Replace("{link}", configuration.AcceptRegistration.Link)
            .Replace("{id}", item.Id.ToString("N")) 

    let generateWelcomeMailHtml (configuration : ServiceConfiguration.EMailProperties) (item : UserRegistration) =
        File.ReadAllText(Path.Combine(configuration.Directory, configuration.AcceptRegistration.File) + ".html")
            .Replace("{link}", configuration.AcceptRegistration.Link)
            .Replace("{id}", item.Id.ToString("N")) 

    let sendSingleMail (configuration : ServiceConfiguration.EMailProperties) (logger : ILogger) (item : UserRegistration) =
        async {
            try
                let client = RestClient ()
                client.BaseUrl <- Uri (configuration.MailGun.Url)
                client.Authenticator <- HttpBasicAuthenticator ("api", configuration.MailGun.Api)
    
                let request = RestRequest ()
                request
                    .AddParameter("domain", "domla.de", ParameterType.UrlSegment)
                    .AddParameter("from", "Registrierung <noreply@domla.de>")
                    .AddParameter("to", item.EMail)
                    .AddParameter("subject", "Willkomen zu Domla/2")
                    .AddParameter("text", generateWelcomeMailText configuration item)
                    .AddParameter("html", generateWelcomeMailHtml configuration item)
                    .Method <- Method.POST
            
                let! result = client.ExecuteTaskAsync request
                              |> Async.AwaitTask
                    
                match result.IsSuccessful with
                | false -> if result.ErrorException |> isNull |> not then logger.LogError (result.ErrorException, "failed with exception")
                           logger.LogError result.ErrorMessage
                           return false
                | true  -> logger.LogInformation (sprintf "successfully sent acceptance mail to %s" item.EMail)
                           return true
            with
            | error -> logger.LogError (error, "failed with exception")
                       return false
        }

    let acceptRegistrationsWorker (ids : Guid seq) (logger : ILogger) (configuration : ServiceConfiguration.EMailProperties) =
        let processor = sendSingleMail configuration logger
        
        let rec countingProcessor (ids : Guid list) (succeeded : int) =
            async {
                match ids with
                | []           -> return succeeded
                | head :: tail -> let! result = CompositionRoot.Storage.acceptRegistration head logger processor
                                  match result with
                                  | true  -> logger.LogInformation (sprintf "registration with id %A accepted" head)
                                             return! countingProcessor tail (succeeded + 1)
                                  | false -> logger.LogWarning (sprintf "accept registration with id %A failed" head)
                                             return! countingProcessor tail succeeded
            }
        countingProcessor (ids |> Seq.toList) 0
    
    let acceptRegistrations (ids : Guid seq) (logger : ILogger) =
        let result = handle {
            let succeeded = acceptRegistrationsWorker ids logger ServiceConfiguration.emailsInfo
                            |> Async.RunSynchronously
            
            match (ids |> Seq.length) > succeeded with 
            | false -> return! succeed StatusCodes.Status202Accepted
            | true  -> match succeeded with
                       | 0 -> return! succeed StatusCodes.Status500InternalServerError
                       | _ -> return! succeed StatusCodes.Status416RangeNotSatisfiable
        }
        
        match result () with
        | Success code      -> code
        | InternalFailure e -> logger.LogError (e, "Exception accepting registrations")
                               StatusCodes.Status500InternalServerError
        | ExternalFailure s -> logger.LogWarning (s)
                               StatusCodes.Status422UnprocessableEntity
