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
  /// Dependency container interface.
  /// </summary>
  public interface IDependencyContainer
  {
    /// <summary>
    /// The tipo is registered?
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>true/false</returns>
    bool Contains(Type type);

    /// <summary>
    /// Return the object, if the type is registered.
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>Object or null</returns>
    object Get(Type type);

    /// <summary>
    /// Record the types of objects.
    /// </summary>
    /// <param name="objs">Objects</param>
    void Register(params object[] objs);

    /// <summary>
    /// Record the type of the object.
    /// </summary>
    /// <param name="obj">Object</param>
    void Register(object obj);

    /// <summary>
    /// Remove a type from the register.
    /// </summary>
    /// <param name="type">Type</param>
    void Remove(Type type);

    /// <summary>
    /// Eliminate all types from the register.
    /// </summary>
    void Clear();
  }
}
