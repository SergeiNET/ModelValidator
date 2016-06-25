using System.Collections.Generic;
using System.Linq;

namespace CommonValidator
{
    public class CommonValidator<T> : ICommonValidator<T>
    {
        protected List<IValidationRule<T>> rules;
        public string Name { get; protected set; }
        public CommonValidator(string name)
        {
            this.Name = name;
            this.rules = new List<IValidationRule<T>>();
        }

        public bool IsValid(T model)
        {
            return rules.All(rule => rule.MatchRule(model));
        }

        public bool IsValidInParallel(T model)
        {
            return rules.AsParallel().All(rule => rule.MatchRule(model));
        }

        public IEnumerable<ValidationError> Validate(T model)
        {
            return rules.Where(rule => !rule.MatchRule(model))
                .Select(r => new ValidationError { RuleName = r.Name, Message = r.Message });
        }

        public IEnumerable<ValidationError> ValidateInParallel(T model)
        {
            return rules.AsParallel().Where(rule => !rule.MatchRule(model))
                .Select(r => new ValidationError { RuleName = r.Name, Message = r.Message });
        }

        public void AddRule(IValidationRule<T> rule)
        {
            this.rules.Add(rule);
        }

        public void AddRules(IEnumerable<IValidationRule<T>> rules)
        {
            this.rules.AddRange(rules);
        }
    }
}
