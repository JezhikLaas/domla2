namespace D2.MasterData.Infrastructure
{
    public interface IParameterValidator
    {
        ValidationResult Validate(object requestParameters, RequestType requestType);
    }
}
