using System;

namespace CommonValidator
{
    public interface IValidationRule<T>
    {
        string Message { get; }
        string Name { get; }
        Func<T, bool> RulePredicate { get; }

        bool MatchRule(T model);
    }
}