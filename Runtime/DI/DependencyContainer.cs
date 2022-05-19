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
using System;
using System.Collections.Generic;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// .
  /// </summary>
  public sealed class DependencyContainer
  {
    private Dictionary<Type, object> container = new Dictionary<Type, object>();

    public bool Contains<T>() where T : class => container.ContainsKey(typeof(T));

    public bool Contains(object obj) => container.ContainsKey(obj.GetType());

    public bool Contains(Type type) => container.ContainsKey(type);

    public T Get<T>() where T : class
    {
      T obj = null;

      Type type = typeof(T);
      if (container.ContainsKey(type) == true)
        obj = container[type] as T;
      else
        Log.Error($"Object '{type}' not found");

      return obj;
    }

    public object Get(Type type)
    {
      object obj = null;
      if (container.ContainsKey(type) == true)
        obj = container[type];
      else
        Log.Error($"Object '{type}' not found");

      return obj;
    }

    public void Register(object obj)
    {
      Type type = obj.GetType();

      if (container.ContainsKey(type) == false)
        container.Add(type, obj);
      else
        Log.Error($"Object '{type}' is already added");
    }

    public void Remove<T>() where T : class => Remove(typeof(T));

    public void Remove(object obj) => Remove(obj.GetType());

    public void Remove(Type type)
    {
      if (container.ContainsKey(type) == true)
        container.Remove(type);
      else
        Log.Error($"Object '{type}' not found");
    }

    public void Clear() => container.Clear();
  }
}
