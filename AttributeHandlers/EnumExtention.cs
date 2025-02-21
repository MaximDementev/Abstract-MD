using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abstract.AttributeHandlers
{
    /// <summary>
    /// Предоставляет метод-расширение для получения описания (атрибута <see cref="DescriptionAttribute"/>) из значения перечисления.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Возвращает описание, заданное атрибутом <see cref="DescriptionAttribute"/> для указанного значения перечисления.
        /// </summary>
        /// <param name="value">Значение перечисления, для которого требуется получить описание.</param>
        /// <returns>
        /// Строковое описание из атрибута <see cref="DescriptionAttribute"/>, если он присутствует; 
        /// в противном случае — строковое представление значения перечисления.
        /// </returns>
        public static string GetEnumDescription(Enum value)
        {
            Type enumType = value.GetType();

            MemberInfo[] memberInfo = enumType.GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return value.ToString();
        }
    }
}
