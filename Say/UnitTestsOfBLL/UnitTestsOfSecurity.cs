#region using
using BLL.Infrastructure;
using BLL.Security;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Security.Validators;
using DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic; 
#endregion

namespace UnitTestsOfBLL
{
    [TestClass]
    public class UnitTestsOfSecurity
    {
        #region PasswordIsEmpty
        [TestMethod]
        public void TestMethod_PasswordIsEmpty()
        {
            // arrange
            IValidation validation = new BasicValidation();
            string password = null;
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Input string is empty"));
        }
        #endregion

        #region PasswordIsEmpty2
        [TestMethod]
        public void TestMethod_PasswordIsEmpty2()
        {
            // arrange
            IValidation validation = new BasicValidation();
            string password = "";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Input string is empty"));
        }
        #endregion

        #region PasswordIsEmpty3
        [TestMethod]
        public void TestMethod_PasswordIsEmpty3()
        {
            // arrange
            IValidation validation = new BasicValidation();
            string password = "           ";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Input string is empty"));
        }
        #endregion

        #region PasswordLengthIsTooLarge
        [TestMethod]
        public void TestMethod_PasswordLengthIsTooLarge()
        {
            // arrange
            IValidation validation = new LengthValidation() { Selector = "Password" };
            string password = "0123456789101234567891012345678910123456789101234567891A";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Password must be less then 50 characters"));
        } 
        #endregion

        #region PasswordLengthIsTooSmall
        [TestMethod]
        public void TestMethod_PasswordLengthIsTooSmall()
        {
            // arrange
            IValidation validation = new PasswordLengthValidation();
            string password = "01235A";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Password must be more then 7 characters"));
        } 
        #endregion

        #region PasswordWithoutUppercaseLetter
        [TestMethod]
        public void TestMethod_PasswordWithoutUppercaseLetter()
        {
            // arrange
            IValidation validation = new PasswordSymbolsValidation();
            string password = "abcdabc7";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Password must contain at least one uppercase letter"));
        } 
        #endregion

        #region PasswordWithoutDigit
        [TestMethod]
        public void TestMethod_PasswordWithoutDigit()
        {
            // arrange
            IValidation validation = new PasswordSymbolsValidation();
            string password = "abcdacdA";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Password must contain at least one digit"));
        } 
        #endregion

        #region RightPassword
        [TestMethod]
        public void TestMethod_RightPassword()
        {
            // arrange
            IValidator validator = new PasswordValidator();
            IEnumerable<IValidation> validations = validator.GetValidations();
            string password = "abcdacdA7";
            List<string> errors = new List<string>();
            bool isValid = true;

            // act
            foreach (var v in validations)
            {
                if (!v.IsValid(password, errors))
                    isValid = false;
            }

            // assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(errors.Capacity, 0);
        } 
        #endregion

        #region AdminWrongPassword
        [TestMethod]
        public void TestMethod_AdminWrongPassword()
        {
            // arrange
            IValidation validation = new AdminPasswordValidation();
            string password = "abcdacdA7";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Administrator's password must contain one secret letter"));
        } 
        #endregion

        #region AdminRightPassword
        [TestMethod]
        public void TestMethod_AdminRightPassword()
        {
            // arrange
            IValidation validation = new AdminPasswordValidation();
            string password = "abcdacdV7";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(password, errors);

            // assert
            Assert.IsTrue(isValid);
        } 
        #endregion

        #region CannotAddNewUserWithWrongPassword
        [TestMethod]
        public void TestMethod_CannotAddNewUserWithWrongPassword()
        {
            // arrange
            User user = new User();
            user.FirstName = "Vladlen";
            user.Surname = "Babitski";
            user.Username = "Vladlen";
            user.Password = "123";
            user.Email = "babitski.vladlen@gmail.com";
            Mock<IUserService> mock = new Mock<IUserService>();
            IRegistration reg = new DefaultRegistration(mock.Object,  (IValidatorFactory)null,
                (IPasswordEngine)null, new MockContext(mock.Object));
            List<string> errors = new List<string>();

            // act
            bool result = reg.TryAddUser(user, user.Password, errors);

            // assert
            Assert.IsFalse(result);
        } 
        #endregion

        #region CannotAddNewUserWithDifferentPasswords
        [TestMethod]
        public void TestMethod_CannotAddNewUserWithDifferentPasswords()
        {
            // arrange
            User user = new User();
            user.FirstName = "Vladlen";
            user.Surname = "Babitski";
            user.Username = "Vladlen";
            user.Password = "123Vladlen";
            user.Email = "babitski.vladlen@gmail.com";
            Mock<IUserService> mock = new Mock<IUserService>();
            IRegistration reg = new DefaultRegistration(mock.Object, (IValidatorFactory)null,
                (IPasswordEngine)null, new MockContext(mock.Object));
            List<string> errors = new List<string>();

            // act
            bool result = reg.TryAddUser(user, "123Vladle", errors);

            // assert
            Assert.IsFalse(result);
            Assert.IsTrue(errors.Contains("Different passwords"));
        } 
        #endregion

        #region AddNewUserWithRightPassword
        [TestMethod]
        public void TestMethod_AddNewUserWithRightPassword()
        {
            // arrange
            User user = new User();
            user.FirstName = "Vladlen";
            user.Surname = "Babitski";
            user.Username = "Vladlen";
            user.Password = "123Vladlen";
            user.Email = "babitski.vladlen@gmail.com";
            Mock<IUserService> mock = new Mock<IUserService>();
            IRegistration reg = new DefaultRegistration(mock.Object, (IValidatorFactory)null,
                (IPasswordEngine)null, new MockContext(mock.Object));
            List<string> errors = new List<string>();

            // act
            reg.TryAddUser(user, user.Password, errors);

            // assert
            mock.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Once);
        } 
        #endregion

        #region WrongEmail
        [TestMethod]
        public void TestMethod_WrongEmail()
        {
            // arrange
            IValidation validation = new EmailSymbolsValidation();
            string email = "vladlen";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(email, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Email is wrong"));
        }
        #endregion

        #region WrongEmail2
        [TestMethod]
        public void TestMethod_WrongEmail2()
        {
            // arrange
            IValidation validation = new EmailSymbolsValidation();
            string email = "vladlen@mail";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(email, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Email is wrong"));
        }
        #endregion

        #region WrongEmail3
        [TestMethod]
        public void TestMethod_WrongEmail3()
        {
            // arrange
            IValidation validation = new EmailSymbolsValidation();
            string email = "vladlen@mail.b";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(email, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Email is wrong"));
        }
        #endregion

        #region WrongEmail4
        [TestMethod]
        public void TestMethod_WrongEmail4()
        {
            // arrange
            IValidation validation = new EmailSymbolsValidation();
            string email = "@.";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(email, errors);

            // assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Email is wrong"));
        }
        #endregion

        #region WrongEmail
        [TestMethod]
        public void TestMethod_RightEmail()
        {
            // arrange
            IValidation validation = new EmailSymbolsValidation();
            string email = "vladlen@mail.ru";
            List<string> errors = new List<string>();
            bool isValid;

            // act
            isValid = validation.IsValid(email, errors);

            // assert
            Assert.IsTrue(isValid);
        }
        #endregion
    }
}
