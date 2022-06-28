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
    /// Initialized?
    /// </summary>
    /// <value>True/false.</value>
    public bool Initialized { get; set; }

    /// <summary>
    /// Should be updated?
    /// </summary>
    /// <value>True/false.</value>
    public bool ShouldUpdate { get; set; } = true;

    /// <summary>
    /// Will it be destroyed?
    /// </summary>
    /// <value>True si va a destruirse.</value>
    public bool WillDestroy { get; set; }

    public CallbackTask NextUpdate { get; set; }
    public CallbackTask NextFixedUpdate { get; set; }

    /// <summary>
    /// On initialize.
    /// </summary>
    public virtual void OnInitialize() { }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public virtual void OnInitialized() { }

    /// <summary>
    /// On deinitialize.
    /// </summary>
    public virtual void OnDeinitialize() { }

    /// <summary>
    /// Update event.
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// FixedUpdate event.
    /// </summary>
    public virtual void OnFixedUpdate() { }

    /// <summary>
    /// LateUpdate event.
    /// </summary>
    public virtual void OnLateUpdate() { }

    /// <summary>
    /// On destroy.
    /// </summary>
    public virtual void OnWillDestroy() { }

    private bool sceneLoaded;

    private void EntryPoint()
    {
      DontDestroyOnLoad(this.gameObject);

      SceneManager.sceneLoaded += OnSceneLoaded;
      SceneManager.sceneUnloaded += OnSceneUnloaded;

      sceneDependencyContainer = new DependencyContainer();
      childDependencyContainer = new DependencyContainer();

      injector = new Injector();
      injector.AddContainer(sceneDependencyContainer);
      injector.AddContainer(childDependencyContainer);

      Application.wantsToQuit += OnWantsToQuit;
#if UNITY_ANDROID || UNITY_IOS
      Application.lowMemory += OnLowMemory;
#endif

      this.OnInitialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void OnBeforeSplashScreen()
    {
      if (Game.IsCreated == true)
        Game.Instance.EntryPoint(); 
    }

    private void OnSceneLoaded(Scene current, LoadSceneMode mode)
    {
      sceneLoaded = true;

      for (int i = 0; i < sceneLoads.Count; ++i)
        sceneLoads[i].OnSceneLoad(current.buildIndex);
    }

    private void OnSceneUnloaded(Scene current)
    {
      for (int i = 0; i < sceneLoads.Count; ++i)
        sceneLoads[i].OnSceneUnload();
    }

    private bool OnWantsToQuit()
    {
      // @HACK: At the end of the execution, Unity calls OnDisable when there are already non valid objects.
      // Calling OnDeinitialize here ensures that objects are still valid and can be used.
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

      this.WillDestroy = true;

      return true;
    }
  }
}
