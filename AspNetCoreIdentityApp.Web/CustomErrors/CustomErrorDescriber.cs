using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomErrors
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        //stratup da/program.cs services.Identity olan hissede .AddErrorDescriber yazaq
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = "Bu ad artıq istifadə olunub"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {

            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifrə ən azı {length} simvol olmalıdır"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            IdentityError error = new()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = $"'!~#$%@ və.s' kimi işarələr istifadə olunmalıdır"
            };
            return error;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            IdentityError error = new()
            {
                Code = "DuplicateEmail",
                Description = $"{email} bu mail artıq istifadə olunub"
            };
            return error;
        }

        public override IdentityError PasswordRequiresLower()
        {
            IdentityError error = new()
            {
                Code = "PasswordRequiresLower",
                Description = $"Şifrədə kiçik hərflərdən istifadə edin"
            };
            return error;
        }

        public override IdentityError PasswordRequiresUpper()
        {
            IdentityError error = new()
            {
                Code = "PasswordRequiresUpper",
                Description = $"Şifrədə böyük hərflərdən istifadə edin"
            };
            return error;
        }
    }
}
