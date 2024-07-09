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
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FronkonGames.GameWork.Foundation;
using FronkonGames.GameWork.Core;

/// <summary> Injection test. </summary>
public partial class InjectionTests
{
  public class ClassA { }
  public class ClassB { }

  public class TestInjection
  {
    public ClassA ClassA => testA;

    [Inject]
    public ClassB ClassB { get; set; }

    [Inject]
    private ClassA testA;
  }

  public class BehaviourA : MonoBehaviour { }
 
  public class BehaviourB : CachedMonoBehaviour
  {
    public BehaviourA BehaviourA => behaviourA;

    [Inject(SearchIn.Parent)]
    private BehaviourA behaviourA;
  }

  public class MonoBehaviourInjectionTest : CachedMonoBehaviour
  {
    public BehaviourA BehaviourA => behaviourA;

    public BehaviourB BehaviourB => behaviourB;

    [Inject(SearchIn.Components)]
    private BehaviourA behaviourA;

    [Inject(SearchIn.Childrens)]
    private BehaviourB behaviourB;
  }

  /// <summary>
  /// Injection test.
  /// </summary>
  [UnityTest]
  public IEnumerator Injection()
  {
    DependencyContainer dependencyContainer = new DependencyContainer();
    Injector injector = new Injector();
    injector.AddContainer(dependencyContainer);

    TestInjection testInjection = new TestInjection();

    ClassA classA = new ClassA();
    ClassB classB = new ClassB();
    dependencyContainer.Register(classA, classB);

    injector.Resolve(testInjection);

    Assert.NotNull(testInjection.ClassA);
    Assert.NotNull(testInjection.ClassB);

    GameObject gameObject = new GameObject();
    gameObject.AddComponent<BehaviourA>();

    GameObject child = new GameObject();
    child.transform.parent = gameObject.transform;
    BehaviourB behaviourB = child.AddComponent<BehaviourB>();

    MonoBehaviourInjectionTest monoBehaviourInjectionTest = gameObject.AddComponent<MonoBehaviourInjectionTest>();

    injector.Resolve(monoBehaviourInjectionTest);
    injector.Resolve(behaviourB);

    Assert.NotNull(monoBehaviourInjectionTest.BehaviourA);
    Assert.NotNull(monoBehaviourInjectionTest.BehaviourB);
    Assert.NotNull(behaviourB.BehaviourA);

    yield return null;
  }
}