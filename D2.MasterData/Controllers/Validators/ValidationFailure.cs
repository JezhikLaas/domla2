namespace D2.MasterData.Controllers.Validators
{
    public interface ValidationFailure
    {
        string PropertyName { get; }
        string ErrorMessage { get; }
    }
}
