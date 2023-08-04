using System;
using System.ComponentModel;
using System.Globalization;

namespace TileExplorer
{
    public class Converters
    {
        public class EnumDescriptionConverter : EnumConverter
        {
            private readonly Type type;

            public EnumDescriptionConverter(Type type) : base(type)
            {
                this.type = type;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return destType == typeof(string);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                var field = type.GetField(Enum.GetName(type, value));

                var description = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                return description != null ? description.Description : (object)value.ToString();
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return srcType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                foreach (var field in type.GetFields())
                {
                    var description = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                    if ((description != null) && ((string)value == description.Description))
                        return Enum.Parse(type, field.Name);
                }

                return Enum.Parse(type, (string)value);
            }
        }
    }
}