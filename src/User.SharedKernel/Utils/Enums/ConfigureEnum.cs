using System.ComponentModel;
using System.Reflection;

namespace User.SharedKernel.Utils.Enums
{
    public static class ConfigureEnum
    {
        public static string GetEnumDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}
