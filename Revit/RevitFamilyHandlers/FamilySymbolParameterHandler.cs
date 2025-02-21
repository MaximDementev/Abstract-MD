using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Abstract.RevitFamilyHandlers
{
    /// <summary>
    /// Класс для установки значений параметров в <see cref="FamilySymbol"/> в зависимости от их типа.
    /// </summary>
    public static class FamilySymbolParameterHandler
    {
        /// <summary>
        /// Устанавливает значение параметра семейства, учитывая его тип данных.
        /// </summary>
        /// <param name="familySymbol">Тип семейства, в котором находится параметр.</param>
        /// <param name="ParameterName">Название параметра.</param>
        /// <param name="Value">Значение, которое нужно установить.</param>
        public static void SetValueToParam(FamilySymbol familySymbol, string ParameterName, string Value)
        {
            Parameter parameter = GetParamByName(familySymbol, ParameterName);
            string dataType = parameter.Definition.ParameterType.ToString();
            switch (dataType)
            {
                case "Text":
                    {
                        parameter.Set(Value);
                        break;
                    }
                case "Integer":
                    {
                        parameter.Set(Int32.Parse(Value));
                        break;
                    }
                case "Number":
                    {
                        parameter.Set(Int32.Parse(Value));
                        break;
                    }
                case "Length":
                    {
                        parameter.Set(Int32.Parse(Value) / 304.8);
                        break;
                    }
                case "BarDiameter":
                    {
                        char[] chars = new char[2];
                        chars[0] = ' ';
                        chars[1] = 'm';

                        Value = Value.Trim(chars);
                        parameter.Set(Int32.Parse(Value) / 304.8);
                        break;
                    }
                case "YesNo":
                    {
                        if (Value == "False")
                            parameter.Set(0);
                        else parameter.Set(1);

                        break;
                    }

                case "FamilyType":
                    {
                        Document doc = familySymbol.Document;
                        List<ElementId> vars = familySymbol.Family.GetFamilyTypeParameterValues(parameter.Id).ToList();
                        ElementId targetVar;
                        targetVar = vars?.FirstOrDefault(var =>
                        doc.GetElement(var).Name.Contains(Value)
                        );
                        if (targetVar != null)
                            parameter.Set(targetVar);
                        else throw new Exception($"Не удалось заполнить параметр \"{ParameterName}\"");
                        break;
                    }
                default:
                    TaskDialog.Show("Ошибка", $"Тип данных: {dataType}");
                    break;
            }
        }

        /// <summary>
        /// Получает параметр семейства по его имени.
        /// </summary>
        /// <param name="familySymbol">Тип семейства.</param>
        /// <param name="ParameterName">Название параметра.</param>
        /// <returns>Объект <see cref="Parameter"/>, если найден; иначе <c>null</c>.</returns>
        public static Parameter GetParamByName(FamilySymbol familySymbol, string ParameterName)
        {
            Parameter parameterType = familySymbol.LookupParameter(ParameterName);
            return parameterType;
        }

        /// <summary>
        /// Получает параметр по имени для элемента или его типа в документе Revit.
        /// </summary>
        /// <param name="doc">Документ Revit, в котором находятся элементы.</param>
        /// <param name="instance">Экземпляр элемента, для которого необходимо получить параметр.</param>
        /// <param name="ParameterName">Имя параметра.</param>
        /// <returns>Параметр экземпляра элемента или параметр типа элемента.</returns>
        /// <exception cref="Exception">Выбрасывается, если не удаётся найти параметр ни у экземпляра, ни у типа.</exception>
        public static Parameter GetParamByName(Document doc, Element instance, string ParameterName)
        {
            Parameter instanceParameter = instance.LookupParameter(ParameterName);
            if (instanceParameter != null)
            { return instanceParameter; }

            ElementId typeId = instance.GetTypeId();
            if (typeId == ElementId.InvalidElementId)
                throw new Exception("Не удалось получить тип по экземпляру семейства");

            Element typeElement = doc.GetElement(typeId);
            if (typeElement == null)
                throw new Exception("Не удалось получить тип по экземпляру семейства");

            Parameter symbolParameter = typeElement.LookupParameter(ParameterName);
            if (symbolParameter == null)
                throw new Exception("Не удалось найти параметр ни в экземпляре, ни в типе");
            return symbolParameter;
        }

        /// <summary>
        /// Получает строковое значение параметра элемента в документе Revit.
        /// </summary>
        /// <param name="doc">Документ Revit, в котором находится элемент.</param>
        /// <param name="element">Элемент, для которого нужно получить значение параметра.</param>
        /// <param name="ParameterName">Имя параметра.</param>
        /// <returns>Строковое значение параметра.</returns>
        /// <exception cref="Exception">Выбрасывается, если параметр не имеет значения (null).</exception>
        public static string GetParameterValue(Document doc, Element element, string ParameterName)
        {
            Parameter parameter = GetParamByName(doc, element, ParameterName);
            string value = parameter.AsValueString();
            if (parameter == null) throw new Exception("Параметр не имеет значения (null)");
            return value;
        }
    }
}
