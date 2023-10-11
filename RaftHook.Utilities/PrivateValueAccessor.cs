using System;
using System.Linq;
using System.Reflection;

namespace RaftHook.Utilities
{
    public class PrivateValueAccessor
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty |
                                           BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic;

        public static PropertyInfo GetPrivatePropertyInfo(Type type, string propertyName)
        {
            var props = type.GetProperties(Flags);
            return props.FirstOrDefault(propInfo => propInfo.Name == propertyName);
        }

        public static FieldInfo GetPrivateFieldInfo(Type type, string fieldName)
        {
            var fields = type.GetFields(Flags);
            return fields.FirstOrDefault(fieldInfo => fieldInfo.Name == fieldName);
        }

        public static object GetPrivateFieldValue(Type type, string fieldName, object o)
        {
            return GetPrivateFieldInfo(type, fieldName).GetValue(o);
        }
    }
}