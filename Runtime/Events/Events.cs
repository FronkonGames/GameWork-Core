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

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Event without parameters.
  /// </summary>
  [CreateAssetMenu(fileName = "VoidEvent", menuName = "Game:Work/Events/Void")]
  public class VoidEvent : Event { }

  /// <summary>
  /// Event with Byte parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "ByteEvent", menuName = "Game:Work/Events/Byte")]
  public class ByteEvent : Event<byte> { }

  /// <summary>
  /// Event with Bool parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "BoolEvent", menuName = "Game:Work/Events/Bool")]
  public class BoolEvent : Event<bool>
  {
    /// <summary>
    /// Invoke the event by changing the value.
    /// </summary>
    public void RaiseToggled() => Raise(!Value);
  }

  /// <summary>
  /// Event with int parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "IntEvent", menuName = "Game:Work/Events/Int")]
  public class IntEvent : Event<int> { }

  /// <summary>
  /// Event with float parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "FloatEvent", menuName = "Game:Work/Events/Float")]
  public class FloatEvent : Event<float> { }

  /// <summary>
  /// Event with string parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "StringEvent", menuName = "Game:Work/Events/String")]
  public class StringEvent : Event<string> { }

  /// <summary>
  /// Event with Vector2 parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "Vector2Event", menuName = "Game:Work/Events/Vector2")]
  public class Vector2Event : Event<Vector2> { }

  /// <summary>
  /// Event with Vector3 parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "Vector3Event", menuName = "Game:Work/Events/Vector3")]
  public class Vector3Event : Event<Vector3> { }

  /// <summary>
  /// Event with Quaternion parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "QuaternionEvent", menuName = "Game:Work/Events/Quaternion")]
  public class QuaternionEvent : Event<Quaternion> { }

  /// <summary>
  /// Event with GameObject parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "GameObjectEvent", menuName = "Game:Work/Events/GameObject")]
  public class GameObjectEvent : Event<GameObject> { }

  /// <summary>
  /// Event with Component parameter.
  /// </summary>
  [CreateAssetMenu(fileName = "ComponentEvent", menuName = "Game:Work/Events/Component")]
  public class ComponentEvent : Event<Component> { }
}
