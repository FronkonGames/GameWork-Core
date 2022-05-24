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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Dependencies injector.
  /// </summary>
  public sealed class Injector : IInjector
  {

    private List<IDependencyContainer> containers = new List<IDependencyContainer>();

    /// <summary>
    /// Add a dependencies container.
    /// </summary>
    /// <param name="container">Dependency container</param>
    public void AddContainer(IDependencyContainer container)
    {
      if (containers.Contains(container) == false)
        containers.Add(container);
      else
        Log.Error("The container is already added");
    }

    /// <summary>
    /// Remove a dependencies container.
    /// </summary>
    /// <param name="container">Dependency container</param>
    public void RemoveContainer(IDependencyContainer container)
    {
      if (containers.Contains(container) == true)
        containers.Remove(container);
      else
        Log.Error("Container not found");
    }

    /// <summary>
    /// Remove all dependencies containers.
    /// </summary>
    public void RemoveAllContainers() => containers.Clear();

    /// <summary>
    /// Look for 'Inject' attributes and injects available objects in the container.
    /// </summary>
    /// <param name="target">Target.</param>
    public void Resolve(object target)
    {
      // Variables.
      FieldInfo[] fieldInfos = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      for (int i = 0; i < fieldInfos.Length; ++i)
        InjectField(target, fieldInfos[i]);

      // Properties.
      PropertyInfo[] propertyInfos = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      for (int i = 0; i < propertyInfos.Length; ++i)
        InjectProperty(target, propertyInfos[i]);
    }

    private void InjectField(object target, FieldInfo fieldInfo)
    {
      InjectAttribute injectAttribute = fieldInfo.GetCustomAttribute<InjectAttribute>();
      if (injectAttribute != null)
      {
        if (injectAttribute.mode == SearchIn.Container)
        {
          bool dependencyFound = false;
          for (int i = 0; i < containers.Count; i++)
          {
            if (containers[i].Contains(fieldInfo.FieldType) == true)
            {
              fieldInfo.SetValue(target, containers[i].Get(fieldInfo.FieldType));
              dependencyFound = true;
              break;
            }
          }
          
          if (dependencyFound == false)
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
            else
              Log.Error($"Type '{fieldInfo.FieldType}' not found in '{monoBehaviour.name}'");
          }
          else
            Log.Error($"'{target}' is not a BaseMonoBehaviour");
        }
      }
    }

    private void InjectProperty(object target, PropertyInfo propertyInfo)
    {
      InjectAttribute injectAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>();
      if (injectAttribute != null)
      {
        if (injectAttribute.mode == SearchIn.Container)
        {
          bool dependencyFound = false;
          for (int i = 0; i < containers.Count; i++)
          {
            if (containers[i].Contains(propertyInfo.PropertyType) == true)
            {
              propertyInfo.SetValue(target, containers[i].Get(propertyInfo.PropertyType));
              dependencyFound = true;
              break;
            }
          }

          if (dependencyFound == false)
            Log.Error($"Type '{propertyInfo.PropertyType}' not registered");
        }
        else
        {
          BaseMonoBehaviour monoBehaviour = target as BaseMonoBehaviour;
          if (monoBehaviour != null)
          {
            Component component = null;
            if (injectAttribute.mode == SearchIn.Components)
              component = monoBehaviour.GetComponent(propertyInfo.PropertyType);
            else if (injectAttribute.mode == SearchIn.Parent)
              component = monoBehaviour.GetComponentInParent(propertyInfo.PropertyType);
            else if (injectAttribute.mode == SearchIn.Childrens)
              component = monoBehaviour.GetComponentInChildren(propertyInfo.PropertyType);

            if (component != null)
              propertyInfo.SetValue(target, component);
            else
              Log.Error($"Type '{propertyInfo.PropertyType}' not found in '{monoBehaviour.name}'");
          }
          else
            Log.Error($"'{target}' is not a BaseMonoBehaviour");
        }
      }
    }
  }
}
