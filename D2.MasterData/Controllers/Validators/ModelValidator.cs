using D2.MasterData.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Controllers.Validators
{
    public interface ModelValidator<T>
    {
        ValidationResult Validate(T instance, params string[] ruleSets);
    }
}
