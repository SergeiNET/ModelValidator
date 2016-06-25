using System.Collections.Generic;

namespace CommonValidator
{
    public interface IValidatorsContainer
    {
        IEnumerable<string> RegisteredModels { get; }

        bool IsValid<T>(T model);
        bool IsValidInParalell<T>(T model);
        void RegisterRule<T>(IValidationRule<T> rule);
        void RegisterRules<T>(IEnumerable<IValidationRule<T>> rules);
        IEnumerable<ValidationError> Validate<T>(T model);
        IEnumerable<ValidationError> ValidateInParallel<T>(T model);
    }
}