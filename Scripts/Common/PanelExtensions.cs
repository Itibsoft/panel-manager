using System;
using System.Linq;
using UnityEngine;

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
            
            Debug.Log($"FullName: {key}");
            
            unchecked
            {
                return (ushort)key.Aggregate(23, (current, c) => current * 31 + c);
            }
        }
    }
}