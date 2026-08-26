namespace FSDK
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionUtils
    {
        /// <summary>
        /// Get all public properties from T
        /// </summary>
        /// <typeparam name="T">Type to get properties from.</typeparam>
        public static PropertyInfo[] GetProperties<T>()
        {
            Type t = typeof(T);
            return  t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Extension: Get property's value by its name
        /// </summary>
        /// <param name="obj">Instance of T</param>
        /// <param name="propertyName">Name of property</param>
        /// <typeparam name="T">Type to get property's value from</typeparam>
        public static object GetPropertyValue<T>(this object obj, string propertyName)
        {
            Type         t    = typeof(T);
            PropertyInfo info = t.GetProperty(propertyName);
            return info == null ? default : info.GetValue(obj);
        }

        /// <summary>Get all types dives from T or Implement interface T that are not abstract.
        /// Note: Only same assembly</summary>
        public static IEnumerable<Type> GetAllDriveType<T>()
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(T).IsAssignableFrom(type));
        }

        /// <summary>
        /// Get all derived types from a type T.
        /// </summary>
        /// <typeparam name="T">Type to get derived classes.</typeparam>
        public static IEnumerable<Type> GetAllDerivedTypes<T>()
        {
            var type = typeof(T);
            return AppDomain.CurrentDomain.GetAssemblies().Where(s => s.IsDynamic == false)
                .SelectMany(GetTypesSafely)
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
        }


        public static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
#if UNITY_EDITOR
            IEnumerable<Type> types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(t => t != null);
            }

            return types;
#else
            return assembly.GetTypes();
#endif
        }
    }
}