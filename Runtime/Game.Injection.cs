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
//#define ENABLE_PROFILING
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary> Module manager and persistent singleton. </summary>
  public abstract partial class Game : PersistentMonoBehaviourSingleton<Game>,
                                       IInitializable,
                                       IUpdatable,
                                       IDestructible
  {
    protected IInjector injector;
    protected IDependencyContainer sceneDependencyContainer;
    protected IDependencyContainer childDependencyContainer;

    private void ResolveChildsDependencies()
    {
#if ENABLE_PROFILING
      using (Profiling.Time("Resolve childs dependencies"))
#endif
      {
        childDependencyContainer.Clear();

        MonoBehaviour[] monoBehaviours = this.gameObject.GetComponentsInChildren<MonoBehaviour>();

        for (int i = 0; i < monoBehaviours.Length; ++i)
        {
          if (typeof(IModule).IsAssignableFrom(monoBehaviours[i].GetType()) == true)
          {
            RegisterModule(monoBehaviours[i]);

            childDependencyContainer.Register(monoBehaviours[i]);
          }
        }

        for (int i = 0; i < monoBehaviours.Length; ++i)
          injector.Resolve(monoBehaviours[i]);
      }
    }

    private void ResolveLoadedSceneDependencies()
    {
#if ENABLE_PROFILING
      using (Profiling.Time("Resolve scene dependencies"))
#endif
      {
        sceneDependencyContainer.Clear();

        List<Component> monoBehaviours = new();
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
          Scene scene = SceneManager.GetSceneAt(i);
          GameObject[] rootGameObjects = scene.GetRootGameObjects();

          for (int j = 0; j < rootGameObjects.Length; ++j)
          {
            List<GameObject> gameObjects = rootGameObjects[j].GetAllChildrenAndSelf();
            for (int k = 0; k < gameObjects.Count; ++k)
              monoBehaviours.AddRange(gameObjects[k].GetComponents<MonoBehaviour>());
          }
        }

        for (int i = 0; i < monoBehaviours.Count; ++i)
          sceneDependencyContainer.Register(monoBehaviours[i]);

        for (int i = 0; i < monoBehaviours.Count; ++i)
          injector.Resolve(monoBehaviours[i]);
      }
    }
  }
}
