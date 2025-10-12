using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static partial class ComBotsSaveSystem
{
    private static class ReflectionUtils
    {
        public static bool IsList(MemberInfo memberInfo)
        {
            Type type = null;
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                type = ((FieldInfo)memberInfo).FieldType;
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                type = ((PropertyInfo)memberInfo).PropertyType;
            }
            if (type != null)
            {
                // Check if the type is a generic type and its generic type definition is List<>
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
            }
            return false;
        }

        public static bool IsArray(MemberInfo memberInfo)
        {
            Type type = null;
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                type = ((FieldInfo)memberInfo).FieldType;
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                type = ((PropertyInfo)memberInfo).PropertyType;
            }
            if (type != null)
            {
                return type.IsArray;
            }

            throw new Exception(
                "MemberInfo is neither FieldInfo nor PropertyInfo");
        }

        public static Type GetType(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)memberInfo).FieldType;
            }
            else if (memberInfo.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)memberInfo).PropertyType;
            }

            throw new Exception(
                "MemberInfo is neither FieldInfo nor PropertyInfo");
        }

        public static IList CreateListFromType(Type type)
        {
            // Get the generic List<T> type definition
            Type listGenericType = typeof(List<>);

            // Construct the specific List<T> type using the provided 'type' variable
            Type constructedListType = listGenericType.MakeGenericType(type);

            // Create an instance of the constructed List<T>
            return (IList)Activator.CreateInstance(constructedListType);
        }

        public static Array ConvertToArrayRuntime(IList list, Type elementType)
        {
            // Get the generic Cast<T> method from System.Linq.Enumerable
            MethodInfo castMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.Cast))
                .MakeGenericMethod(elementType);

            // Invoke Cast<T> on the list
            IEnumerable castedEnumerable = (IEnumerable)castMethod
                .Invoke(null, new object[] { list });

            // Get the generic ToArray<T> method from System.Linq.Enumerable
            MethodInfo toArrayMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToArray))
                .MakeGenericMethod(elementType);

            // Invoke ToArray<T> on the casted enumerable
            return (Array)toArrayMethod.Invoke(
                null, new object[] { castedEnumerable });
        }
    }

}