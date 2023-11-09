using System;
using System.Linq;
using System.Reflection;

namespace Itibsoft.PanelManager
{
    public static class PanelExtensions
    {
        public static ushort GetStableHash(this Type controller)
        {
            var key = controller.FullName;

            if (key == default)
            {
                return 0;
            }

            unchecked
            {
                return (ushort)key.Aggregate(23, (current, c) => current * 31 + c);
            }
        }

        public static void SetMeta(this IPanel panel, PanelAttribute meta)
        {
            var propertyType = panel.GetType().GetProperty("Meta",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (propertyType != null)
            {
                var setter = propertyType.GetSetMethod(true);

                if (setter != null) setter.Invoke(panel, new object[] { meta });
                else throw new Exception("No set method found for property 'Meta'");
            }
        }
    }
}