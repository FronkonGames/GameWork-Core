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
    /// Last FPS.
    /// </summary>
    /// <value>FPS.</value>
    public float CurrentFPS { get; private set; }

    /// <summary>
    /// Average FPS.
    /// </summary>
    /// <value>FPS.</value>
    public float AverageFPS { get; private set; }

    private int frames;
    private float deltaTime;

    private Queue<float> history = new Queue<float>(HistoryFrames);
    private IEnumerator<float> historyEnumerator;

    private const int UpdatePerSecond = 2;
    private const int HistoryFrames = 100;

    /// <summary>
    /// Reset the counters.
    /// </summary>
    public void Reset()
    {
      CurrentFPS = 0.0f;
      AverageFPS = 0.0f;
      frames = 0;
      deltaTime = 0.0f;

      history.Clear();
      historyEnumerator = history.GetEnumerator();
    }

    /// <summary>
    /// When initialize.
    /// </summary>
    public void OnInitialize() => Reset();

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
      
      float lapse = 1.0f / UpdatePerSecond;
      if (deltaTime > lapse)
      {
        CurrentFPS = frames / deltaTime;
        frames = 0;
        deltaTime -= lapse;

        int count = history.Count;
        if (count >= HistoryFrames)
          history.Dequeue();

        history.Enqueue(CurrentFPS);

        float total = 0.0f;
        while (historyEnumerator.MoveNext() == true)
          total += historyEnumerator.Current;

        AverageFPS = total / count;
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
