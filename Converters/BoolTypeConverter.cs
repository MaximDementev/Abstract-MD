using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.TypeConverter;

namespace Abstract.Converters
{
    /// <summary>
    /// Преобразователь типов, который позволяет конвертировать значение типа bool 
    /// в локализованное строковое представление ("Да"/"Нет") и обратно.
    /// </summary>
    public class BoolTypeConverter : TypeConverter
    {
        /// <summary>
        /// Определяет, можно ли преобразовать значение в указанный тип.
        /// Поддерживается преобразование в строку.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="destinationType">Тип, в который необходимо преобразовать значение.</param>
        /// <returns>True, если преобразование в указанный тип возможно, иначе False.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Выполняет преобразование из значения типа bool в локализованное строковое представление ("Да"/"Нет").
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="culture">Культура, используемая для форматирования.</param>
        /// <param name="value">Значение для преобразования.</param>
        /// <param name="destinationType">Тип, в который необходимо преобразовать значение.</param>
        /// <returns>Локализованное строковое представление "Да" или "Нет".</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is bool)
            {
                return (bool)value ? "Да" : "Нет"; // Локализованное текстовое представление
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Определяет, можно ли выполнить преобразование из указанного типа.
        /// Поддерживается преобразование из строки.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="sourceType">Тип, из которого выполняется преобразование.</param>
        /// <returns>True, если преобразование из указанного типа возможно, иначе False.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Выполняет преобразование из локализованной строки ("Да"/"Нет") в значение типа bool.
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <param name="culture">Культура, используемая для форматирования.</param>
        /// <param name="value">Значение для преобразования.</param>
        /// <returns>Логическое значение true или false.</returns>
        /// <exception cref="FormatException">Выбрасывается, если строка не является "Да" или "Нет".</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strValue)
            {
                // Преобразование локализованной строки в bool
                if (strValue.Equals("Да", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (strValue.Equals("Нет", StringComparison.OrdinalIgnoreCase))
                    return false;

                throw new FormatException($"Невозможно преобразовать '{strValue}' в логическое значение.");
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Определяет, поддерживаются ли стандартные значения для типа (для выпадающего списка).
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <returns>Всегда возвращает True.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            // Указываем, что есть стандартные значения для выпадающего списка
            return true;
        }

        /// <summary>
        /// Определяет, ограничены ли возможные значения только стандартными (только "Да" и "Нет").
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <returns>Всегда возвращает True.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            // Указываем, что только стандартные значения допустимы (выпадающий список)
            return true;
        }

        /// <summary>
        /// Возвращает стандартные значения для выбора в выпадающем списке ("Да" и "Нет").
        /// </summary>
        /// <param name="context">Контекст форматирования.</param>
        /// <returns>Коллекция стандартных значений "Да" и "Нет".</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // Возвращаем стандартные значения для выпадающего списка
            return new StandardValuesCollection(new[] { "Да", "Нет" });
        }
    }
}
