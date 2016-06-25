using System.Collections.Generic;

namespace CommonValidator
{
    public interface ICommonValidator<T>
    {
        string Name { get; }

        void AddRule(IValidationRule<T> rule);
        void AddRules(IEnumerable<IValidationRule<T>> rules);
        bool IsValid(T model);
        bool IsValidInParallel(T model);
        IEnumerable<ValidationError> Validate(T model);
        IEnumerable<ValidationError> ValidateInParallel(T model);
    }
}