using System;
using System.Reflection;

namespace Itibsoft.PanelManager
{
    public static class ReflectionExtension
    {
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