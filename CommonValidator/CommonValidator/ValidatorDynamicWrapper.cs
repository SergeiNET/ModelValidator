using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonValidator
{
    internal class ValidatorDynamicWrapper
    {
        protected CommonValidator<dynamic> validator;

        public ValidatorDynamicWrapper(string name)
        {
            validator = new CommonValidator<dynamic>(name);
        }

        public void AddRule<T>(IValidationRule<T> rule)
        {
            Func<dynamic, bool> dynamicPredicate = x => rule.RulePredicate(x);
            var dynamicRule = new ValidationRule<dynamic>(rule.Name, dynamicPredicate, rule.Message);
            validator.AddRule(dynamicRule);
        }

        public void AddRules<T>(IEnumerable<IValidationRule<T>> rules)
        {
            foreach (var rule in rules)
            {
                Func<dynamic, bool> dynamicPredicate = x => rule.RulePredicate(x);
                var dynamicRule = new ValidationRule<dynamic>(rule.Name, dynamicPredicate, rule.Message);
                validator.AddRule(dynamicRule);
            }        
        }

        public IEnumerable<ValidationError> Validate<T>(T model)
        {
            return validator.Validate(model);
        }

        public bool IsValid<T>(T model)
        {
            return validator.IsValid(model);
        }

        public IEnumerable<ValidationError> ValidateInParallel<T>(T model)
        {
            return validator.ValidateInParallel(model);
        }

        public bool IsValidInParallel<T>(T model)
        {
            return validator.IsValidInParallel(model);
        }
    }
}
