namespace D2.MasterData.Infrastructure
{
    public class ValidationError
    {
        public ValidationError(string name, string error)
        {
            Property = name;
            Error = error;
        }

        public string Property { get; }

        public string Error { get; }
    }
}

