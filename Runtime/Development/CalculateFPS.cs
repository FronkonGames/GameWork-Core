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

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Calculate FPS.
  /// </summary>
  public sealed class CalculateFPS : IInitializable,
                                     IUpdatable
  {
    /// <summary>
    /// Last FPS value.
    /// </summary>
    /// <value>FPS.</value>
    public float CurrentFPS { get; private set; }

    private int frames;
    private float deltaTime;
    private const int updatePerSecond = 2;

    /// <summary>
    /// Is it initialized?
    /// </summary>
    /// <value>Value</value>
    public bool Initialized { get; set; }

    /// <summary>
    /// Should be updated?
    /// </summary>
    /// <value>True/false.</value>
    public bool ShouldUpdate { get; set; } = true;

    /// <summary>
    /// When initialize.
    /// </summary>
    public void OnInitialize()
    {
      frames = 0;
      deltaTime = 0.0f;
      CurrentFPS = 0.0f;
    }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public void OnInitialized() {}

    /// <summary>
    /// When deinitialize.
    /// </summary>
    public void OnDeinitialize() {}

    /// <summary>
    /// Update event.
    /// </summary>
    public void OnUpdate()
    {
      ++frames;
      deltaTime += Time.unscaledDeltaTime;
      
      float lapse = 1.0f / updatePerSecond;
      if (deltaTime > lapse)
      {
        CurrentFPS = frames / deltaTime;
        frames = 0;
        deltaTime -= lapse;
      }
    }

    /// <summary>
    /// FixedUpdate event.
    /// </summary>
    public void OnFixedUpdate() {}

    /// <summary>
    /// LateUpdate event.
    /// </summary>
    public void OnLateUpdate() {}
  }
}
