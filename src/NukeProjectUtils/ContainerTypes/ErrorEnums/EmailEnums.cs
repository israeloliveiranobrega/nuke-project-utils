using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace NukeProjectUtils.ContainerTypes.ErrorEnums;

public enum EmailCreationError
{
    [Description("Email address cannot be empty or white spaces.")]
    NullOrEmpty = 0,

    [Description("The email contains invalid characters.")]
    InvalidCharacters = 1,

    [Description("The email contains numbers, which are not allowed.")]
    NumbersNotAllowed = 2,

    [Description("The email address is too long.")]
    MaxLengthExceeded = 3,

    [Description("The email format is invalid.")]
    InvalidFormat = 4,

    [Description("The token is invalid")]
    InvalidToken = 5,
}
public enum EmailValidationError
{
    ValidationTokenAlreadyExists = 0,
    ValidationTokenNotFound = 1,
}
