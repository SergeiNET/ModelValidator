using System;

namespace CommonValidator
{
    public class ValidationRule<T> : IValidationRule<T>
    {
        public Func<T, bool> RulePredicate { get; protected set; }
        public string Name { get; protected set; }
        public string Message { get; protected set; }

        public ValidationRule(string name, Func<T, bool> rulePredicate)
        {
            this.Name = name;
            this.RulePredicate = rulePredicate;
            this.Message = string.Empty;
        }

        public ValidationRule(string name, Func<T, bool> rulePredicate, string messagge)
        {
            this.Name = name;
            this.RulePredicate = rulePredicate;
            this.Message = messagge;
        }

        public bool MatchRule(T model)
        {
            return this.RulePredicate(model);
        }
    }
}
