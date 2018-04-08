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
                | true  -> logger.LogDebug (sprintf "successfully sent acceptance mail to %s" item.EMail)
                           return true
            with
            | error -> logger.LogError (error, "failed with exception")
                       return false
        }

    let acceptRegistrationsWorker (ids : Guid seq) (logger : ILogger) (configuration : ServiceConfiguration.EMailProperties) =
        async {
            let processor = sendSingleMail configuration logger 
            for id in ids do
                let! result = CompositionRoot.Storage.acceptRegistration id logger processor
                match result with
                | true  -> logger.LogInformation (sprintf "registration with id %A accepted" id)
                | false -> logger.LogWarning (sprintf "accept registration with id %A failed" id)
        }
    
    let acceptRegistrations (ids : Guid seq) (logger : ILogger) =
        let result = handle {
            acceptRegistrationsWorker ids logger ServiceConfiguration.emailsInfo |> Async.RunSynchronously
            return! succeed StatusCodes.Status202Accepted
        }
        
        match result () with
        | Success code      -> code
        | InternalFailure e -> logger.LogError (e, "Exception accepting registrations")
                               StatusCodes.Status500InternalServerError
        | ExternalFailure s -> logger.LogWarning (s)
                               StatusCodes.Status422UnprocessableEntity
