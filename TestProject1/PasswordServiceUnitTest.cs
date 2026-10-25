using Servers;
using Xunit;

namespace TestProject1
{
    public class PasswordServiceUnitTest
    {
        private readonly PasswordService _passwordService = new PasswordService();

        [Fact]
        public void VerifyPassword_ReturnsTrue_ForMatchingPassword()
        {
            string hash = _passwordService.HashPassword("Str0ng!Passw0rd");

            Assert.True(_passwordService.VerifyPassword("Str0ng!Passw0rd", hash));
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_ForWrongPassword()
        {
            string hash = _passwordService.HashPassword("Str0ng!Passw0rd");

            Assert.False(_passwordService.VerifyPassword("wrongPassword", hash));
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_ForPlainTextStoredPassword()
        {
            // users saved before hashing was added have a plain-text password in the DB
            Assert.False(_passwordService.VerifyPassword("123456", "123456"));
        }

        [Fact]
        public void HashPassword_DoesNotReturnPlainText()
        {
            string hash = _passwordService.HashPassword("Str0ng!Passw0rd");

            Assert.NotEqual("Str0ng!Passw0rd", hash);
            Assert.StartsWith("$2", hash);
        }
    }
}
