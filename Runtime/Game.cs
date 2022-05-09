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
  /// 
  /// </summary>
  public abstract class Game : MonoBehaviourSingleton<BaseMonoBehaviour>,
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

    private readonly FastList<IInitializable> initializables = new FastList<IInitializable>();
    private readonly FastList<IActivable> activables = new FastList<IActivable>();
    private readonly FastList<IUpdatable> updatables = new FastList<IUpdatable>();
    private readonly FastList<IGUI> GUIables = new FastList<IGUI>();
    private readonly FastList<IRenderObject> renderableObjects = new FastList<IRenderObject>();
    private readonly FastList<IDestructible> destructibles = new FastList<IDestructible>();
#if UNITY_ANDROID || UNITY_IOS
    private readonly FastList<ILowMemory> lowMemories = new FastList<ILowMemory>();
#endif
    private readonly FastList<IModule> allModules = new FastList<IModule>();

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

    /// <summary>
    /// Returns a module.
    /// </summary>
    /// <typeparam name="T">Type of module.</typeparam>
    /// <returns>Module o null</returns>
    public T GetModule<T>() where T : IModule, new()
    {
      Type typeOf = typeof(T);

      for (int i = 0; i < allModules.Count; ++i)
      {
        if (allModules[i].GetType() == typeOf)
          return (T)allModules[i];
      }

      Log.Exception($"Unable to get '{typeOf}', it was not registerd");

      return default(T);
    }

    /// <summary>
    /// Returns a module.
    /// </summary>
    /// <typeparam name="T">Type of module.</typeparam>
    /// <returns>Module o null</returns>
    public IModule GetModule(Type typeOf)
    {
      for (int i = 0; i < allModules.Count; ++i)
      {
        if (allModules[i].GetType() == typeOf)
          return allModules[i];
      }

      Log.Exception($"Unable to get '{typeOf}', it was not registerd");

      return null;
    }

    /// <summary>
    /// The module is registered?
    /// </summary>
    /// <typeparam name="T">Type of module.</typeparam>
    /// <returns>True if you are registered.</returns>
    public bool HasModule<T>() where T : IModule, new()
    {
      Type typeOfS = typeof(T);
      for (int i = 0; i < allModules.Count; ++i)
      {
        if (this.allModules[i].GetType() == typeOfS)
          return true;
      }

      return false;
    }

    private void RegisterModule(IModule module, Type type)
    {
      if (typeof(IInitializable).IsAssignableFrom(type) == true)
        initializables.Add(module as IInitializable);

      if (typeof(IActivable).IsAssignableFrom(type) == true)
        activables.Add(module as IActivable);

      if (typeof(IUpdatable).IsAssignableFrom(type) == true)
        updatables.Add(module as IUpdatable);

      if (typeof(IGUI).IsAssignableFrom(type) == true)
        GUIables.Add(module as IGUI);

      if (typeof(IRenderObject).IsAssignableFrom(type) == true)
        renderableObjects.Add(module as IRenderObject);

      if (typeof(IDestructible).IsAssignableFrom(type) == true)
        destructibles.Add(module as IDestructible);

#if UNITY_ANDROID || UNITY_IOS
      if (typeof(ILowMemory).IsAssignableFrom(type) == true)
        lowMemories.Add(service as ILowMemory);
#endif

      allModules.Add(module);

      Log.Info($"'{type.ToString()}' registred");
    }

    /// <summary>
    /// During the awake, this system will start the initialization.
    /// </summary>
    private void Awake()
    {
      DontDestroyOnLoad(this.gameObject);

      Application.wantsToQuit += OnWantsToQuit;
#if UNITY_ANDROID || UNITY_IOS
      Application.lowMemory += OnLowMemory;
#endif

      this.OnInitialize();

      for (int i = 0; i < initializables.Count; ++i)
        initializables[i].OnInitialize();
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
      if (Initialized == false)
      {
        this.Initialized = true;
        this.OnInitialized();

        for (int i = 0; i < initializables.Count; ++i)
        {
          initializables[i].Initialized = true;
          initializables[i].OnInitialized();
        }
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
#if UNITY_ANDROID || UNITY_IOS
      lowMemories.Clear();
#endif

      allModules.Clear();

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
    protected override void OnApplicationQuit()
    {
      base.OnApplicationQuit();

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
