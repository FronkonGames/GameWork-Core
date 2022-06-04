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
    private readonly FastList<IInitializable> initializables = new FastList<IInitializable>();
    private readonly FastList<IActivable> activables = new FastList<IActivable>();
    private readonly FastList<IUpdatable> updatables = new FastList<IUpdatable>();
    private readonly FastList<IGUI> GUIables = new FastList<IGUI>();
    private readonly FastList<IRenderObject> renderableObjects = new FastList<IRenderObject>();
    private readonly FastList<IDestructible> destructibles = new FastList<IDestructible>();
    private readonly FastList<ISceneLoad> sceneLoads = new FastList<ISceneLoad>();
#if UNITY_ANDROID || UNITY_IOS
    private readonly FastList<ILowMemory> lowMemories = new FastList<ILowMemory>();
#endif
    private readonly FastList<IModule> allModules = new FastList<IModule>();

    /// <summary>
    /// Register modules. Only allowed in OnInitialize.
    /// </summary>
    /// <param name="object">List of modules.</param>
    public void RegisterModule(params object[] modules)
    {
      if (Initialized == true || sceneLoaded == true)
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
      if (Initialized == true || sceneLoaded == true)
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
    public void UnregisterModule(params object[] modules)
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

    private void RegisterModule(object obj)
    {
      Type type = obj.GetType();

      if (typeof(IInitializable).IsAssignableFrom(type) == true)
      {
        IInitializable initializable = obj as IInitializable;
        initializable.OnInitialize();

        initializables.Add(initializable);
      }

      if (typeof(IActivable).IsAssignableFrom(type) == true)
        activables.Add(obj as IActivable);

      if (typeof(IUpdatable).IsAssignableFrom(type) == true)
        updatables.Add(obj as IUpdatable);

      if (typeof(IGUI).IsAssignableFrom(type) == true)
        GUIables.Add(obj as IGUI);

      if (typeof(IRenderObject).IsAssignableFrom(type) == true)
        renderableObjects.Add(obj as IRenderObject);

      if (typeof(IDestructible).IsAssignableFrom(type) == true)
        destructibles.Add(obj as IDestructible);

      if (typeof(ISceneLoad).IsAssignableFrom(type) == true)
        sceneLoads.Add(obj as ISceneLoad);

#if UNITY_ANDROID || UNITY_IOS
      if (typeof(ILowMemory).IsAssignableFrom(type) == true)
        lowMemories.Add(module as ILowMemory);
#endif

      if (typeof(IModule).IsAssignableFrom(type) == true)
        allModules.Add(obj as IModule);
      else
        Log.Error($"Object '{type.Name}' is not a valid module");
    }

    private void UnregisterModule(object module)
    {
      Type type = module.GetType();
      if (allModules.Contains(module as IModule) == true)
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

        if (typeof(ISceneLoad).IsAssignableFrom(type) == true)
          sceneLoads.Remove(module as ISceneLoad);

#if UNITY_ANDROID || UNITY_IOS
      if (typeof(ILowMemory).IsAssignableFrom(type) == true)
        lowMemories.Remove(module as ILowMemory);
#endif

        allModules.Remove(module as IModule);
      }
      else
        Log.Error($"Object '{type.Name}' is not a valid module");

      if (sceneDependencyContainer.Contains(type) == true)
        sceneDependencyContainer.Remove(type);
    }
  }
}
