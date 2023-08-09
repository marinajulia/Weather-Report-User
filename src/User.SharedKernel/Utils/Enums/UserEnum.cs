using System.ComponentModel;

namespace User.SharedKernel.Utils.Enums
{
    public enum UserEnum
    {
        [Description("Oops.. The name must be typed")]
        FieldNameEmpty,

        [Description("Oops.. The email must be typed")]
        FieldEmailEmpty,

        [Description("Oops.. The password must be typed")]
        FieldPasswordEmpty,

        [Description("Oops.. The city must be typed")]
        FieldCityEmpty,

        [Description("Oops.. Incorrect username or password")]
        IncorrectUsernameOrPassword,

        [Description("Oops.. Could not find this user")]
        CouldNotFind,

        [Description("Oops.. There are fields empty fields")]
        EmptyFields,
    }
}
