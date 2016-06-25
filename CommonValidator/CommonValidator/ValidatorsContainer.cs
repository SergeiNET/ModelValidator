using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommonValidator
{
    public class ValidatorsContainer: IValidatorsContainer
    {
        private Dictionary<string, ValidatorDynamicWrapper> container;

        public IEnumerable<string> RegisteredModels => container.Keys;

        public ValidatorsContainer()
        {
            this.container = new Dictionary<string, ValidatorDynamicWrapper>();
        }

        public void RegisterRule<T>(IValidationRule<T> rule)
        {
            var modelTypeName = typeof (T).FullName;
            if (!container.ContainsKey(modelTypeName))
            {
                container.Add(modelTypeName, new ValidatorDynamicWrapper(modelTypeName));
                
            }

            container[modelTypeName].AddRule(rule);
        }

        public void RegisterRules<T>(IEnumerable<IValidationRule<T>> rules)
        {
            var modelTypeName = typeof(T).FullName;
            if (!container.ContainsKey(modelTypeName))
            {
                container.Add(modelTypeName, new ValidatorDynamicWrapper(modelTypeName));
            }

            container[modelTypeName].AddRules(rules);
        }

        public bool IsValid<T>(T model)
        {
            var modelTypeName = typeof(T).FullName;

            if (container.ContainsKey(modelTypeName))
            {
                return container[modelTypeName].IsValid(model);
            }

            throw new ArgumentException(TypeInNotRegisteredExceptionMessage(modelTypeName));
        }

        public bool IsValidInParalell<T>(T model)
        {
            var modelTypeName = typeof(T).FullName;

            if (container.ContainsKey(modelTypeName))
            {
                return container[modelTypeName].IsValidInParallel(model);
            }

            throw new ArgumentException(TypeInNotRegisteredExceptionMessage(modelTypeName));
        }

        public IEnumerable<ValidationError> Validate<T>(T model)
        {
            var modelTypeName = typeof(T).FullName;

            if (container.ContainsKey(modelTypeName))
            {
                return container[modelTypeName].Validate(model);
            }

            throw new ArgumentException(TypeInNotRegisteredExceptionMessage(modelTypeName));
        }

        public IEnumerable<ValidationError> ValidateInParallel<T>(T model)
        {
            var modelTypeName = typeof(T).FullName;

            if (container.ContainsKey(modelTypeName))
            {
                return container[modelTypeName].ValidateInParallel(model);
            }

            throw new ArgumentException(TypeInNotRegisteredExceptionMessage(modelTypeName));
        }

        private string TypeInNotRegisteredExceptionMessage(string modelTypeName)
        {
            return $@"Validdation rules were not found for {modelTypeName}.
                      To fix it just register validation rules for this type...";
        }
    }
}
