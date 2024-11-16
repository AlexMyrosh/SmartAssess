using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace SmartAssess.Extensions
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<CustomIdentityErrorDescriber> _localizer;
        public CustomIdentityErrorDescriber(IStringLocalizer<CustomIdentityErrorDescriber> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"{_localizer["PasswordTooShortPart1"]} {length} {_localizer["PasswordTooShortPart2"]}"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = _localizer["PasswordRequiresNonAlphanumeric"].ToString()
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = _localizer["PasswordRequiresUpper"].ToString()
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = _localizer["PasswordRequiresLower"].ToString()
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"{_localizer["DuplicateUserNamePart1"]} '{userName}' {_localizer["DuplicateUserNamePart2"]}"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"{_localizer["DuplicateEmailPart1"]} '{email}' {_localizer["DuplicateEmailPart2"]}"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"{_localizer["InvalidUserNamePart1"]} '{userName}' {_localizer["InvalidUserNamePart2"]}"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = $"{_localizer["InvalidEmailPart1"]} '{email}' {_localizer["InvalidEmailPart2"]}"
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = _localizer["InvalidToken"].ToString()
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = _localizer["PasswordRequiresDigit"].ToString()
            };
        }
    }

}
