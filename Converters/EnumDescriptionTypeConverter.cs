using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.TypeConverter;

namespace Abstract.Converters
{
    /// <summary>
     /// Преобразователь типов для перечислений, который использует атрибут <see cref="DescriptionAttribute"/> 
     /// для представления значений перечисления в виде строк с описанием.
     /// </summary>
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EnumDescriptionTypeConverter"/> для указанного типа перечисления.
        /// </summary>
        /// <param name="type">Тип перечисления, для которого выполняется преобразование.</param>
        public EnumDescriptionTypeConverter(Type type) : base(type) { }

        /// <summary>
        /// Преобразует значение перечисления в строковое представление, используя атрибут <see cref="DescriptionAttribute"/>, если он присутствует.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="culture">Культура, используемая для форматирования.</param>
        /// <param name="value">Значение перечисления для преобразования.</param>
        /// <param name="destinationType">Тип, в который необходимо преобразовать значение.</param>
        /// <returns>Строковое представление значения перечисления с учетом атрибута <see cref="DescriptionAttribute"/>, если он задан.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    var field = value.GetType().GetField(value.ToString());
                    var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();

                    return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Преобразует строковое представление, соответствующее значению <see cref="DescriptionAttribute"/> или имени элемента, в значение перечисления.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="culture">Культура, используемая для форматирования.</param>
        /// <param name="value">Строковое значение для преобразования.</param>
        /// <returns>Экземпляр перечисления, соответствующий строковому представлению.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                foreach (var field in EnumType.GetFields())
                {
                    var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttribute != null && descriptionAttribute.Description == stringValue)
                    {
                        return Enum.Parse(EnumType, field.Name);
                    }
                }
                return Enum.Parse(EnumType, stringValue);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Возвращает стандартные значения для типа перечисления.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <returns>Коллекция стандартных значений перечисления.</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var values = Enum.GetValues(EnumType);
            return new StandardValuesCollection(values);
        }
    }
}
