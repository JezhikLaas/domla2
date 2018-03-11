namespace D2.ServiceBroker

module ServiceConnection =
    open RabbitMQ.Client
    open System
    open System.Text

    type ServiceConnector (queue : string) as this =
        
        let factory = new ConnectionFactory(HostName = "localhost")

        do
            this.Connection <- factory.CreateConnection ()
            this.Channel <- this.Connection.CreateModel ()
            let queueOk = this.Channel.QueueDeclare(
                              queue = queue,
                              durable = false,
                              exclusive = false,
                              autoDelete = false
                          )
            
            ()
        
        member val Connection : IConnection = null with get, set
        member val Channel : IModel = null with get, set

        member this.Dispose (disposing : bool) =
            if disposing then
                if not (isNull this.Connection) then this.Connection.Dispose ()
                if not (isNull this.Channel) then this.Channel.Dispose ()
            ()

        interface IDisposable with
            member this.Dispose () =
                this.Dispose true
        
        member this.Post (message : string) =
            let data = Encoding.UTF8.GetBytes message
            this.Channel.BasicPublish (
                exchange = "",
                routingKey = "hello",
                basicProperties = null,
                body = data
            )
