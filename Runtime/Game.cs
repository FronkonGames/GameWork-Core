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
using UnityEngine;
using UnityEngine.SceneManagement;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// .
  /// </summary>
  public abstract class Game : MonoBehaviourSingleton<Game>,
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

    private readonly FastList<IInitializable> initializables = new FastList<IInitializable>();
    private readonly FastList<IActivable> activables = new FastList<IActivable>();
    private readonly FastList<IUpdatable> updatables = new FastList<IUpdatable>();
    private readonly FastList<IGUI> GUIables = new FastList<IGUI>();
    private readonly FastList<IRenderObject> renderableObjects = new FastList<IRenderObject>();
    private readonly FastList<IDestructible> destructibles = new FastList<IDestructible>();
    private readonly FastList<IBeforeSceneLoad> beforeSceneLoad = new FastList<IBeforeSceneLoad>();
#if UNITY_ANDROID || UNITY_IOS
    private readonly FastList<ILowMemory> lowMemories = new FastList<ILowMemory>();
#endif
    private readonly FastList<IModule> allModules = new FastList<IModule>();

    protected IInjector injector;
    protected IDependencyContainer dependencyContainer;

    private bool sceneInitializing;

    /// <summary>
    /// Register modules. Only allowed in OnInitialize.
    /// </summary>
    /// <param name="modules">List of modules.</param>
    public void RegisterModule(params IModule[] modules)
    {
      if (Initialized == true || sceneInitializing == true)
        Log.Error("Cannot to registered outsize of OnInitialize cycle");

      for (int i = 0; i < modules.Length; ++i)
        RegisterModule(modules[i]);
    }

    /// <summary>
    /// Register modules by type and create them.
    /// </summary>
    /// <param name="types">Types of modules.</param>
    public void RegisterModule(params Type[] types)
    {
      if (Initialized == true || sceneInitializing == true)
        Log.Error("Cannot to registered outsize of OnInitialize cycle");

      for (int i = 0; i < types.Length; ++i)
      {
        if (typeof(IModule).IsAssignableFrom(types[i]) == true)
        {
          IModule module = null;
          if (typeof(MonoBehaviourModule).IsAssignableFrom(types[i]) == false)
            module = Activator.CreateInstance(types[i]) as IModule;
          else
          {
            GameObject gameObject = new GameObject(types[i].Name);
            module = gameObject.AddComponent(types[i]) as IModule;
          }

          if (module != null)
            RegisterModule(module);
          else
            Log.Error($"'{types[i]}' null, not register to the Game");
        }
        else
          Log.Error($"'{types[i]}' unknown, not register to the Game");
      }
    }

    /// <summary>
    /// Unregister modules.
    /// </summary>
    /// <param name="modules">Listado de modulos.</param>
    public void UnregisterModule(params IModule[] modules)
    {
      for (int i = 0; i < modules.Length; ++i)
        UnregisterModule(modules[i]);
    }

    /// <summary>
    /// The module is registered?
    /// </summary>
    /// <typeparam name="T">Type of module.</typeparam>
    /// <returns>True if you are registered.</returns>
    public bool HasModule<T>() where T : IModule
    {
      for (int i = 0; i < allModules.Count; ++i)
      {
        if (allModules[i] is T)
          return true;
      }

      return false;
    }

    /// <summary>
    /// Returns the first module of a type (or null).
    /// </summary>
    /// <typeparam name="T">Module type</typeparam>
    /// <returns>Module or null</returns>
    public T GetModule<T>() where T : IModule
    {
      for (int i = 0; i < allModules.Count; ++i)
      {
        if (allModules[i] is T)
          return (T)allModules[i];
      }

      Log.Error($"Module {typeof(T).Name} not register");

      return default(T);
    }

    /// <summary>
    /// Returns all modules of a type (or empty).
    /// </summary>
    /// <typeparam name="T">Module type</typeparam>
    /// <returns>List of modules</returns>
    public List<T> GetModules<T>() where T : IModule
    {
      List<T> modules = new List<T>();
      for (int i = 0; i < allModules.Count; ++i)
      {
        if (allModules[i] is T)
          modules.Add((T)allModules[i]);
      }

      return modules;
    }

    private void RegisterModule(IModule module)
    {
      Type type = module.GetType();

      if (typeof(IInitializable).IsAssignableFrom(type) == true)
      {
        IInitializable initializable = module as IInitializable;
        initializable.OnInitialize();

        initializables.Add(initializable);
      }

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

      if (typeof(IBeforeSceneLoad).IsAssignableFrom(type) == true)
        beforeSceneLoad.Add(module as IBeforeSceneLoad);

#if UNITY_ANDROID || UNITY_IOS
      if (typeof(ILowMemory).IsAssignableFrom(type) == true)
        lowMemories.Add(module as ILowMemory);
#endif

      allModules.Add(module);

      if (dependencyContainer.Contains(type) == false)
        dependencyContainer.Register(module);
    }

    private void UnregisterModule(IModule module)
    {
      Type type = module.GetType();
      if (allModules.Contains(module) == true)
      {
        if (typeof(IInitializable).IsAssignableFrom(type) == true)
        {
          IInitializable initializable = module as IInitializable;
          initializable.OnDeinitialize();

          initializables.Remove(initializable);
        }

        if (typeof(IActivable).IsAssignableFrom(type) == true)
          activables.Remove(module as IActivable);

        if (typeof(IUpdatable).IsAssignableFrom(type) == true)
          updatables.Remove(module as IUpdatable);

        if (typeof(IGUI).IsAssignableFrom(type) == true)
          GUIables.Remove(module as IGUI);

        if (typeof(IRenderObject).IsAssignableFrom(type) == true)
          renderableObjects.Remove(module as IRenderObject);

        if (typeof(IDestructible).IsAssignableFrom(type) == true)
          destructibles.Remove(module as IDestructible);

        if (typeof(IBeforeSceneLoad).IsAssignableFrom(type) == true)
          beforeSceneLoad.Remove(module as IBeforeSceneLoad);

#if UNITY_ANDROID || UNITY_IOS
      if (typeof(ILowMemory).IsAssignableFrom(type) == true)
        lowMemories.Remove(module as ILowMemory);
#endif

        allModules.Remove(module);
      }

      if (dependencyContainer.Contains(type) == true)
        dependencyContainer.Remove(type);
    }

    private void ResolveDependencies()
    {
      System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
      timer.Start();

      UnityEngine.Object[] objects = FindObjectsOfType<UnityEngine.Object>();

      dependencyContainer.Clear();
      dependencyContainer.Register(objects);

      for (int i = 0; i < objects.Length; ++i)
        injector.Resolve(objects[i]);

      Log.Info($"Resolved {objects.Length} object dependencies in {(float)timer.ElapsedMilliseconds * 0.001f:0.000} seconds");
    }

    private void EntryPoint()
    {
      DontDestroyOnLoad(this.gameObject);

      SceneManager.sceneUnloaded += OnSceneUnloaded;

      dependencyContainer = new DependencyContainer();
      injector = new Injector();
      injector.AddContainer(dependencyContainer);

      Application.wantsToQuit += OnWantsToQuit;
#if UNITY_ANDROID || UNITY_IOS
      Application.lowMemory += OnLowMemory;
#endif

      this.OnInitialize();
    }

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
      if (this.Initialized == false || sceneInitializing == true)
      {
        if (this.Initialized == false)
        {
          this.OnInitialized();
          this.Initialized = true;
        }

        ResolveDependencies();

        for (int i = 0; i < initializables.Count; ++i)
        {
          if (initializables[i].Initialized == false)
          {
            initializables[i].Initialized = true;
            initializables[i].OnInitialized();
          }
        }

        sceneInitializing = false;
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
      SceneManager.sceneUnloaded -= OnSceneUnloaded;

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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void OnBeforeSplashScreen() => Game.Instance.EntryPoint();

    private void OnSceneUnloaded(Scene current)
    {
      sceneInitializing = true;

      for (int i = 0; i < beforeSceneLoad.Count; ++i)
        beforeSceneLoad[i].OnBeforeSceneLoad();
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
