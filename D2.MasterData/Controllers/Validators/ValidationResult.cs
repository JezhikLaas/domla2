using System.Collections.Generic;

namespace D2.MasterData.Controllers.Validators
{
    public interface ValidationResult
    {
        bool IsValid { get; }
        IEnumerable<ValidationFailure> Errors { get; }
    }
}
