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
using UnityEngine.SceneManagement;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// .
  /// </summary>
  public abstract partial class Game : MonoBehaviourSingleton<Game>,
                                       IInitializable,
                                       IUpdatable,
                                       IDestructible
  {
    /// <summary>
    /// During the awake, this system will start the initialization.
    /// </summary>
    private void Awake()
    {
    }

    // This function is called when the object becomes enabled and active.
    private void OnEnable()
    {
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
      for (int i = 0; i < initializables.Count; ++i)
      {
        if (initializables[i].Initialized == true)
        {
          initializables[i].OnDeinitialize();
          initializables[i].Initialized = false;
        }
      }

      if (this.Initialized == true)
      {
        this.OnDeinitialize();
        this.Initialized = false;
      }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
      if (this.Initialized == false || sceneLoaded == true)
      {
        ResolveLoadedSceneDependencies();

        if (this.Initialized == false)
        {
          this.OnInitialized();
          this.Initialized = true;
        }

        for (int i = 0; i < initializables.Count; ++i)
        {
          if (initializables[i].Initialized == false)
          {
            initializables[i].Initialized = true;
            initializables[i].OnInitialized();
          }
        }

        sceneLoaded = false;
      }

      if (ShouldUpdate == true && this.WillDestroy == false)
      {
        this.OnUpdate();

        for (int i = 0; i < updatables.Count; ++i)
        {
          if (updatables[i].ShouldUpdate == true)
            updatables[i].OnUpdate();
        }
      }

      if (NextUpdate != null)
      {
        CallbackAwaiter awaiter = NextUpdate.Awaiter;
        NextUpdate = null;
        awaiter.Complete();
      }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
      if (ShouldUpdate == true && this.WillDestroy == false)
      {
        this.OnFixedUpdate();

        for (int i = 0; i < updatables.Count; ++i)
        {
          if (updatables[i].ShouldUpdate == true)
            updatables[i].OnFixedUpdate();
        }
      }

      if (NextFixedUpdate != null)
      {
        CallbackAwaiter awaiter = NextFixedUpdate.Awaiter;
        NextFixedUpdate = null;
        awaiter.Complete();
      }
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
      if (ShouldUpdate == true && this.WillDestroy == false)
      {
        this.OnLateUpdate();

        for (int i = 0; i < updatables.Count; ++i)
        {
          if (updatables[i].ShouldUpdate == true)
            updatables[i].OnLateUpdate();
        }
      }
    }

    /// <summary>
    /// OnRenderObject is called after camera has rendered the scene.
    /// </summary>
    private void OnRenderObject()
    {
      for (int i = 0; i < renderableObjects.Count; ++i)
        renderableObjects[i].OnRenderObject();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
      SceneManager.sceneLoaded -= OnSceneLoaded;

      this.WillDestroy = true;

      for (int i = 0; i < destructibles.Count; ++i)
      {
        destructibles[i].WillDestroy = true;
        destructibles[i].OnWillDestroy();
      }

      this.OnWillDestroy();

      initializables.Clear();
      activables.Clear();
      updatables.Clear();
      GUIables.Clear();
      renderableObjects.Clear();
      destructibles.Clear();
      beforeSceneLoad.Clear();
#if UNITY_ANDROID || UNITY_IOS
      lowMemories.Clear();
#endif
      allModules.Clear();

      dependencyContainer.Clear();

#if UNITY_ANDROID || UNITY_IOS
      Application.lowMemory -= OnLowMemory;
#endif
      Application.wantsToQuit -= OnWantsToQuit;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
      for (int i = 0; i < GUIables.Count; ++i)
        GUIables[i].OnDrawGizmos();
    }
#endif

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// </summary>
    private void OnGUI()
    {
      for (int i = 0; i < GUIables.Count; ++i)
        GUIables[i].OnGUI();
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    private void OnApplicationQuit()
    {
      this.WillDestroy = true;

      for (int i = 0; i < destructibles.Count; ++i)
        destructibles[i].WillDestroy = true;
    }

#if UNITY_ANDROID || UNITY_IOS
    /// <summary>
    /// This event occurs when your app receives a low-memory notification from the device it is running on.
    /// </summary>
    private void OnLowMemory()
    {
      for (int i = 0; i < lowMemories.Count; ++i)
        lowMemories[i].OnLowMemory();
    }
#endif
  }
}
