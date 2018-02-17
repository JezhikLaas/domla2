namespace D2.MasterData.Infrastructure
{
    public interface IParameterValidator
    {
        ValidationResult Validate(RequestParameters requestParameters, RequestType requestType);
    }
}
