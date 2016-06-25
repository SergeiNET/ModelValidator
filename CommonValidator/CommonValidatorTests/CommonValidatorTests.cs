using System.Collections.Generic;
using System.Linq;
using CommonValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonValidatorTests
{
    public class BookModel
    {
        public string Name;
        public UserModel Author;
        public int PagesCount;
    }

    public class UserModel
    {
        public string Name;
        public int Age;
        public List<string> Emails;
    }

    [TestClass]
    public class CommonValidatorTests
    {
        private ICommonValidator<UserModel> validator;

        [TestInitialize]
        public void Init()
        {
            validator = new CommonValidator<UserModel>("simple validator");
            validator.AddRule(new ValidationRule<UserModel>("Name Requried", x => !string.IsNullOrWhiteSpace(x.Name)));
            validator.AddRule(new ValidationRule<UserModel>("Is Adult User", x => x.Age >= 18));
            validator.AddRule(new ValidationRule<UserModel>("Invalid Age", x => x.Age > 0 && x.Age < 150));
            validator.AddRule(new ValidationRule<UserModel>("Should Have More Than 1 Email", x => x.Emails != null && x.Emails.Count > 1));
        }

        [TestMethod]
        public void ValidateValidModel_Returns_True()
        {
            var model = new UserModel
            {
                Name = "Andy",
                Age = 18,
                Emails = new List<string> { "ts@mail.com", "kts@ms.com" }
            };

            Assert.IsTrue(validator.IsValid(model));
            Assert.IsTrue(validator.IsValidInParallel(model));

            Assert.IsFalse(validator.Validate(model).Any());
            Assert.IsFalse(validator.ValidateInParallel(model).Any());
        }

        [TestMethod]
        public void ValidateInvalidModel_Returns_False()
        {
            var model = new UserModel
            {
                Name = "",
                Age = 17,
                Emails = new List<string> { "ts@mail.com" }
            };

            Assert.IsFalse(validator.IsValid(model));
            Assert.IsFalse(validator.IsValidInParallel(model));

            Assert.IsTrue(validator.Validate(model).Any());
            Assert.IsTrue(validator.ValidateInParallel(model).Any());
        }

        [TestMethod]
        public void ValidatorsContainer_ValidateDifferentModels()
        {
            var validatorsContainer = new ValidatorsContainer();
            validatorsContainer.RegisterRule(new ValidationRule<BookModel>("Invalid Author", x => validatorsContainer.IsValid(x.Author)));
            validatorsContainer.RegisterRule(new ValidationRule<UserModel>("Is not null", x => x != null));
            validatorsContainer.RegisterRule(new ValidationRule<BookModel>("Is not null", x => x != null));

            validatorsContainer.RegisterRule(new ValidationRule<UserModel>("Name Requried", x => !string.IsNullOrWhiteSpace(x.Name)));
            validatorsContainer.RegisterRule(new ValidationRule<UserModel>("Is Adult User", x => x.Age >= 18));
            validatorsContainer.RegisterRule(new ValidationRule<BookModel>("Name Requried", x => !string.IsNullOrWhiteSpace(x.Name)));
            validatorsContainer.RegisterRule(new ValidationRule<BookModel>("Invalid pages count", x => x.PagesCount > 0));
            

            var user = new UserModel
            {
                Name = "Andy",
                Age = 16,
                Emails = new List<string> { "ts@mail.com", "kts@ms.com" }
            };

            var book = new BookModel
            {
                Name = "Savin.QA.Bible",
                PagesCount = -3,
                Author = user
            };

            var userErrors = validatorsContainer.Validate(user).ToList();
            var bookErrors = validatorsContainer.Validate(book).ToList();
            Assert.IsTrue(userErrors.Any());
            Assert.IsTrue(bookErrors.Any());
        }
    }
}
