using System.ComponentModel;
using System.Reflection;

namespace DaccApi.Helpers
{
    /// <summary>
    /// Fornece métodos de extensão para enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtém a descrição de um valor de enum, a partir do atributo [Description].
        /// </summary>
        public static string GetDescription(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}