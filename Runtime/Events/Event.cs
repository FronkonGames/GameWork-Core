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
using UnityEngine.Events;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// Event without parameters.
  /// </summary>
  public class Event : ScriptableEvent
  {
    [SerializeField]
    private UnityEvent events = new UnityEvent();

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public override void Raise() => events.Invoke();

    /// <summary>
    /// Subscribes call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Subscribe(UnityAction call)
    {
      Check.IsNotNull(call);
      events.AddListener(call);
    }

    /// <summary>
    /// Unsubscribe call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Unsubscribe(UnityAction call)
    {
      Check.IsNotNull(call);
      events.RemoveListener(call);
    }

    /// <summary>
    /// Remove all event subscriptions.
    /// </summary>
    public override void UnsubscribeAll() => events.RemoveAllListeners();
  }

  /// <summary>
  /// Event with one parameter.
  /// </summary>
  public class Event<T0> : ScriptableEvent
  {
    /// <summary>
    /// First parameter.
    /// </summary>
    public T0 Value { get; set; }

    [SerializeField]
    private UnityEvent<T0> events = new UnityEvent<T0>();

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public override void Raise() => Raise(Value);

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public void Raise(T0 value)
    {
      Value = value;

      events.Invoke(value);
    }

    /// <summary>
    /// Subscribes call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Subscribe(UnityAction<T0> call)
    {
      Check.IsNotNull(call);
      events.AddListener(call);
    }

    /// <summary>
    /// Unsubscribe call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Unsubscribe(UnityAction<T0> call)
    {
      Check.IsNotNull(call);
      events.RemoveListener(call);
    }

    /// <summary>
    /// Remove all event subscriptions.
    /// </summary>
    public override void UnsubscribeAll() => events.RemoveAllListeners();
  }

  /// <summary>
  /// Event with two parameters.
  /// </summary>
  public class Event<T0, T1> : ScriptableEvent
  {
    /// <summary>
    /// First parameter.
    /// </summary>
    public T0 Value0 { get; set; }

    /// <summary>
    /// Second parameter.
    /// </summary>
    public T1 Value1 { get; set; }

    [SerializeField]
    private UnityEvent<T0, T1> events = new UnityEvent<T0, T1>();

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public override void Raise() => Raise(Value0, Value1);

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public void Raise(T0 value0, T1 value1)
    {
      Value0 = value0;
      Value1 = value1;

      events.Invoke(value0, value1);
    }

    /// <summary>
    /// Subscribes call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Subscribe(UnityAction<T0, T1> call)
    {
      Check.IsNotNull(call);
      events.AddListener(call);
    }

    /// <summary>
    /// Unsubscribe call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Unsubscribe(UnityAction<T0, T1> call)
    {
      Check.IsNotNull(call);
      events.RemoveListener(call);
    }

    /// <summary>
    /// Remove all event subscriptions.
    /// </summary>
    public override void UnsubscribeAll() => events.RemoveAllListeners();
  }

  /// <summary>
  /// Event with three parameters.
  /// </summary>
  public class Event<T0, T1, T2> : ScriptableEvent
  {
    /// <summary>
    /// First parameter.
    /// </summary>
    public T0 Value0 { get; set; }

    /// <summary>
    /// Second parameter.
    /// </summary>
    public T1 Value1 { get; set; }

    /// <summary>
    /// Third parameter.
    /// </summary>
    public T2 Value2 { get; set; }

    [SerializeField]
    private UnityEvent<T0, T1, T2> events = new UnityEvent<T0, T1, T2>();

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public override void Raise() => Raise(Value0, Value1, Value2);

    /// <summary>
    /// Invokes the event.
    /// </summary>
    public void Raise(T0 value0, T1 value1, T2 value2)
    {
      Value0 = value0;
      Value1 = value1;
      Value2 = value2;

      events.Invoke(value0, value1, value2);
    }

    /// <summary>
    /// Subscribes call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Subscribe(UnityAction<T0, T1, T2> call)
    {
      Check.IsNotNull(call);
      events.AddListener(call);
    }

    /// <summary>
    /// Unsubscribe call to the event.
    /// </summary>
    /// <param name="call">Action</param>
    public void Unsubscribe(UnityAction<T0, T1, T2> call)
    {
      Check.IsNotNull(call);
      events.RemoveListener(call);
    }

    /// <summary>
    /// Remove all event subscriptions.
    /// </summary>
    public override void UnsubscribeAll() => events.RemoveAllListeners();
  }
}
