////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of
// the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Reflection;
using UnityEngine;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Dependencies injector.
  /// </summary>
  public static class Injector
  {
    /// <summary>
    /// Look for 'Inject' attributes and injects available objects in the container.
    /// </summary>
    /// <param name="target">Target.</param>
    public static void Resolve(object target, IDependencyContainer container)
    {
      // Variables.
      FieldInfo[] fieldInfos = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      for (int i = 0; i < fieldInfos.Length; ++i)
        InjectField(target, container, fieldInfos[i]);

      // Properties.
      PropertyInfo[] propertyInfos = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      for (int i = 0; i < propertyInfos.Length; ++i)
        InjectProperty(target, container, propertyInfos[i]);
    }

    private static void InjectField(object target, IDependencyContainer container, FieldInfo fieldInfo)
    {
      InjectAttribute injectAttribute = fieldInfo.GetCustomAttribute<InjectAttribute>();
      if (injectAttribute != null)
      {
        if (injectAttribute.mode == SearchIn.Container)
        {
          if (container.Contains(fieldInfo.FieldType) == true)
            fieldInfo.SetValue(target, container.Get(fieldInfo.FieldType));
          else
            Log.Error($"Type '{fieldInfo.FieldType}' not registered");
        }
        else
        {
          BaseMonoBehaviour monoBehaviour = target as BaseMonoBehaviour;
          if (monoBehaviour != null)
          {
            Component component = null;
            if (injectAttribute.mode == SearchIn.Components)
              component = monoBehaviour.GetComponent(fieldInfo.FieldType);
            else if (injectAttribute.mode == SearchIn.Parent)
              component = monoBehaviour.GetComponentInParent(fieldInfo.FieldType);
            else if (injectAttribute.mode == SearchIn.Childrens)
              component = monoBehaviour.GetComponentInChildren(fieldInfo.FieldType);

            if (component != null)
              fieldInfo.SetValue(target, component);
          }
          else
            Log.Error($"'{target}' is not a BaseMonoBehaviour");
        }
      }
    }

    private static void InjectProperty(object target, IDependencyContainer container, PropertyInfo propertyInfo)
    {
      InjectAttribute injectAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>();
      if (injectAttribute != null && injectAttribute.mode == SearchIn.Container)
      {
        if (container.Contains(propertyInfo.PropertyType) == true)
          propertyInfo.SetValue(target, container.Get(propertyInfo.PropertyType));
        else
          Log.Error($"Type '{propertyInfo.PropertyType}' not registered");
      }
    }
  }
}
