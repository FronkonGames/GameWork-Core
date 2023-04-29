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
using UnityEngine;
using System.Threading.Tasks;

namespace FronkonGames.GameWork.Core
{
  /// <summary> Custom async Awaiters. </summary>
  public static class Awaiters
  {
    /// <summary> Wait for the next Update. </summary>
    /// <returns>Task</returns>
    public static async Task NextUpdate() => await (Game.Instance.NextUpdate ??= new CallbackTask());

    /// <summary> Wait for the next FixedUpdate. </summary>
    /// <returns>Task.</returns>
    public static async Task NextFixedUpdate() => await (Game.Instance.NextFixedUpdate ??= new CallbackTask());

    /// <summary> Wait a few seconds. </summary>
    /// <param name="seconds">Seconds to wait.</param>
    /// <returns>Task.</returns>
    public static async Task Seconds(float seconds)
    {
      while (seconds >= 0.0f)
      {
        await NextUpdate();

        seconds -= Time.deltaTime;
      }
    }
  }
}
