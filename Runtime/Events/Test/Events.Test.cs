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
using UnityEngine.Events;
using UnityEngine.TestTools;
using FronkonGames.GameWork.Core;

/// <summary>
/// Events test.
/// </summary>
public partial class EventsTests
{
  public class TestEvents
  {
    public UnityAction voidAction;
    public UnityAction<byte> byteAction;
    public UnityAction<bool> boolAction;
    public UnityAction<int> intAction;
    public UnityAction<float> floatAction;
    public UnityAction<string> stringAction;
    public UnityAction<Vector2> vector2Action;
    public UnityAction<Vector3> vector3Action;
    public UnityAction<Quaternion> quaternionAction;
    public UnityAction<GameObject> gameObjectAction;
    public UnityAction<Component> componentAction;

    public bool boolValue = false;
    public byte byteValue = 0;
    public int intValue = 0;
    public float floatValue = 0.0f;
    public string stringValue = string.Empty;
    public Vector2 vector2Value = Vector2.zero;
    public Vector3 vector3Value = Vector3.zero;
    public Quaternion quaternionValue = Quaternion.identity;
    public GameObject gameObjectValue = null;
    public Component componentValue = null;

    private void VoidFunc() => boolValue = !boolValue;
    private void ByteFunc(byte value) => byteValue = value;
    private void BoolFunc(bool value) => boolValue = value;
    private void IntFunc(int value) => intValue = value;
    private void FloatFunc(float value) => floatValue = value;
    private void StringFunc(string value) => stringValue = value;
    private void Vector2Func(Vector2 value) => vector2Value = value;
    private void Vector3Func(Vector3 value) => vector3Value = value;
    private void QuaternionFunc(Quaternion value) => quaternionValue = value;
    private void GameObjectFunc(GameObject value) => gameObjectValue = value;
    private void ComponentFunc(Component value) => componentValue = value;

    public TestEvents()
    {
      voidAction += VoidFunc;
      byteAction += ByteFunc;
      boolAction += BoolFunc;
      intAction += IntFunc;
      floatAction += FloatFunc;
      stringAction += StringFunc;
      vector2Action += Vector2Func;
      vector3Action += Vector3Func;
      quaternionAction += QuaternionFunc;
      gameObjectAction += GameObjectFunc;
      componentAction += ComponentFunc;
    }
  }

  /// <summary>
  /// Events test.
  /// </summary>
  [UnityTest]
  public IEnumerator Events()
  {
    TestEvents testEvents = new TestEvents();

    VoidEvent voidEvent = ScriptableObject.CreateInstance<VoidEvent>();
    ByteEvent byteEvent = ScriptableObject.CreateInstance<ByteEvent>();
    BoolEvent boolEvent = ScriptableObject.CreateInstance<BoolEvent>();
    IntEvent intEvent = ScriptableObject.CreateInstance<IntEvent>();
    FloatEvent floatEvent = ScriptableObject.CreateInstance<FloatEvent>();
    StringEvent stringEvent = ScriptableObject.CreateInstance<StringEvent>();
    Vector2Event vector2Event = ScriptableObject.CreateInstance<Vector2Event>();
    Vector3Event vector3Event = ScriptableObject.CreateInstance<Vector3Event>();
    QuaternionEvent quaternionEvent = ScriptableObject.CreateInstance<QuaternionEvent>();
    GameObjectEvent gameObjectEvent = ScriptableObject.CreateInstance<GameObjectEvent>();

    voidEvent.Subscribe(testEvents.voidAction);
    voidEvent.Raise();
    Assert.IsTrue(testEvents.boolValue);
    voidEvent.Unsubscribe(testEvents.voidAction);
    voidEvent.Raise();
    Assert.IsTrue(testEvents.boolValue);

    byteEvent.Subscribe(testEvents.byteAction);
    byteEvent.Raise(1);
    Assert.AreEqual(testEvents.byteValue, 1);
    byteEvent.Unsubscribe(testEvents.byteAction);

    boolEvent.Subscribe(testEvents.boolAction);
    boolEvent.Raise(false);
    Assert.IsFalse(testEvents.boolValue);
    boolEvent.Unsubscribe(testEvents.boolAction);

    intEvent.Subscribe(testEvents.intAction);
    intEvent.Raise(1);
    Assert.AreEqual(testEvents.intValue, 1);
    intEvent.Unsubscribe(testEvents.intAction);

    floatEvent.Subscribe(testEvents.floatAction);
    floatEvent.Raise(1.0f);
    Assert.AreEqual(testEvents.intValue, 1.0f);
    floatEvent.Unsubscribe(testEvents.floatAction);

    stringEvent.Subscribe(testEvents.stringAction);
    stringEvent.Raise("raised");
    Assert.AreEqual(testEvents.stringValue, "raised");
    stringEvent.Unsubscribe(testEvents.stringAction);

    vector2Event.Subscribe(testEvents.vector2Action);
    vector2Event.Raise(Vector2.one);
    Assert.AreEqual(testEvents.vector2Value, Vector2.one);
    vector2Event.Unsubscribe(testEvents.vector2Action);

    vector3Event.Subscribe(testEvents.vector3Action);
    vector3Event.Raise(Vector2.one);
    Assert.AreEqual(testEvents.vector2Value, Vector2.one);
    vector2Event.Unsubscribe(testEvents.vector2Action);

    quaternionEvent.Subscribe(testEvents.quaternionAction);
    quaternionEvent.Raise(Quaternion.Euler(90.0f, 0.0f, 0.0f));
    Assert.AreEqual(testEvents.quaternionValue, Quaternion.Euler(90.0f, 0.0f, 0.0f));
    quaternionEvent.Unsubscribe(testEvents.quaternionAction);

    GameObject gameObject = new GameObject();
    gameObjectEvent.Subscribe(testEvents.gameObjectAction);
    gameObjectEvent.Raise(gameObject);
    Assert.AreEqual(testEvents.gameObjectValue, gameObject);
    gameObjectEvent.Unsubscribe(testEvents.gameObjectAction);

    yield return null;
  }
}