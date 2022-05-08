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
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class Game : MonoBehaviourSingleton<BaseMonoBehaviour>
  {
    /// <summary>
    /// During the awake, this system will start the initialization.
    /// </summary>
    private void Awake()
    {
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
    }

    /// <summary>
    /// OnRenderObject is called after camera has rendered the scene.
    /// </summary>
    private void OnRenderObject()
    {
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
    }

#if UNITY_EDITOR
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
    }
#endif

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// </summary>
    private void OnGUI()
    {
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    private void OnApplicationQuit()
    {
    }

#if UNITY_ANDROID || UNITY_IOS
    /// <summary>
    /// This event occurs when your app receives a low-memory notification from the device it is running on.
    /// </summary>
    private void OnLowMemory()
    {
    }
#endif

    /// <summary>
    /// 
    /// <summary>
    private bool OnWantsToQuit()
    {
      return true;
    }
  }
}
