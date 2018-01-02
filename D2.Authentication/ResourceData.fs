namespace D2.Authentication

module ResourceData =

    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System
    open System.Data.Common

    let findApiResource (options : ConnectionOptions) (name : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- """SELECT
                                          data
                                      FROM
                                          api_resources
                                      WHERE
                                          name = :name"""
                
            command.Parameters << ("name", name) |> ignore
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

            match reader.Read () with
            | true  -> return Some (JsonConvert.DeserializeObject<ApiResource>(reader.GetString(0)))
            | false -> return None
        }

    let findApiResourcesByScope (options : ConnectionOptions) (names : string seq) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- sprintf
                                    """SELECT
                                           data
                                       FROM
                                           api_resources
                                       WHERE
                                           data -> Scopes ->> Name IN (%s)"""
                                    (String.Join (",", names))
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

            return seq {
                while reader.Read () do
                    yield JsonConvert.DeserializeObject<ApiResource>(reader.GetString(0))
            }
            |> Seq.toList
            |> List.toSeq
        }

    let findIdentityResourcesByScope (options : ConnectionOptions) (names : string seq) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- sprintf
                                    """SELECT
                                           data
                                       FROM
                                           identity_resources
                                       WHERE
                                           name IN (%s)"""
                                    (String.Join (",", names))
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

            return seq {
                while reader.Read () do
                    yield JsonConvert.DeserializeObject<IdentityResource>(reader.GetString(0))
            }
            |> Seq.toList
            |> List.toSeq
        }

    let getAllResources (options : ConnectionOptions) () =
        async {
            use connection = authentication options
                
            let identityResources () = 
                use commandIdentities = connection.CreateCommand ()
                commandIdentities.CommandText <- """SELECT
                                                        data
                                                    FROM
                                                        identity_resources"""
                
                use readerIdentities = commandIdentities.ExecuteReader ()
                
                seq {
                    while readerIdentities.Read () do
                        yield JsonConvert.DeserializeObject<IdentityResource>(readerIdentities.GetString(0))
                }
                |> Seq.toList
                
            let apiResources () = 
                use commandApis = connection.CreateCommand ()
                commandApis.CommandText <- """SELECT
                                                  data
                                              FROM
                                                  api_resources"""
                    
                use readerApis = commandApis.ExecuteReader ()

                seq {
                    while readerApis.Read () do
                        yield JsonConvert.DeserializeObject<ApiResource>(readerApis.GetString(0))
                }
                |> Seq.toList
                
            return new Resources (
                identityResources (),
                apiResources ()
            )
        }

    let access (options : ConnectionOptions) =
        {
            findApiResource = findApiResource options;
            findApiResourcesByScope = findApiResourcesByScope options;
            findIdentityResourcesByScope = findIdentityResourcesByScope options;
            getAllResources = getAllResources options;
        }



