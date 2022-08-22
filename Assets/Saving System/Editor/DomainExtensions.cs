using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DomainExtensions
{
    public static List<Type> GetAllDerivedTypes(this AppDomain domain, Type baseType)
    {
        List<Type> result = new List<Type>();
        Assembly runtimeAssembly = domain.GetAssemblies().First(assembly => assembly.FullName == "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

        Type[] types = runtimeAssembly.GetTypes();

        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].IsSubclassOf(baseType))
                result.Add(types[i]);
        }

        return result;
    }

    public static List<Type> GetAllDerivedTypes<T>(this AppDomain domain) where T : Object
    {
        return GetAllDerivedTypes(domain, typeof(T));
    }

    public static List<Type> GetTypesWithInterface(this AppDomain domain, Type interfaceType)
    {
        List<Type> result = new List<Type>();
        Assembly runtimeAssembly = domain.GetAssemblies().First(assembly => assembly.FullName == "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

        Type[] types = runtimeAssembly.GetTypes();

        for (int i = 0; i < types.Length; i++)
        {
            if (interfaceType.IsAssignableFrom(types[i]))
                result.Add(types[i]);
        }
        return result;
    }

    public static List<Type> GetTypesWithInterface<T>(this AppDomain domain)
    {
        return GetTypesWithInterface(domain, typeof(T));
    }
}
