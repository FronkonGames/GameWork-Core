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
using UnityEngine;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Auto adapts the quality settings.
  /// </summary>
  public sealed class AdaptQualitySettings : ScriptableModule,
                                             IInitializable,
                                             IUpdatable
  {
    [SerializeField]
    private float updateTime = 5.0f;

    [SerializeField]
    private float flickerWaitTime = 20.0f;

    [SerializeField]
    private float lowerFPSThreshold = 25.0f;

    [SerializeField]
    private float upperFPSThreshold = 40.0f;

    [Inject]
    private CalculateFPS calculateFPS;

    private float timeToUpdate;
    private int stability;
    private int flickering;
    private bool lastChangeWasDown;

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
      timeToUpdate = 0.0f;
      stability = 0;
      flickering = 0;
      lastChangeWasDown = false;
    }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public void OnInitialized()
    {
      if (calculateFPS == null)
      {
        ShouldUpdate = false;

        Debug.LogError("This module requires CalculateFPS.");
      }
    }

    /// <summary>
    /// When deinitialize.
    /// </summary>
    public void OnDeinitialize() { }

    /// <summary>
    /// Update event.
    /// </summary>
    public void OnUpdate() { }

    /// <summary>
    /// FixedUpdate event.
    /// </summary>
    public void OnFixedUpdate() { }

    /// <summary>
    /// LateUpdate event.
    /// </summary>
    public void OnLateUpdate()
    {
      timeToUpdate -= Time.unscaledDeltaTime;
      if (timeToUpdate < 0.0f)
      {
        timeToUpdate = updateTime;

        CheckQuality();
      }
    }

    private void CheckQuality()
    {
      if (calculateFPS.CurrentFPS < lowerFPSThreshold)
      {
        QualitySettings.DecreaseLevel();
        stability--;

        if (lastChangeWasDown == false)
          flickering++;

        Log.Warning($"Reducing the quality level to '{QualitySettings.names[QualitySettings.GetQualityLevel()]}'");
      }
    }
  }
}
