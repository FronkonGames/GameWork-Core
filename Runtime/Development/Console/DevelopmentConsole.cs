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
using System.Collections.Generic;
using UnityEngine;
using FronkonGames.GameWork.Foundation;

namespace FronkonGames.GameWork.Core
{
  /// <summary>
  /// 
  /// </summary>
  public class DevelopmentConsole : MonoBehaviourModule,
                                    IInitializable,
                                    IUpdatable,
                                    IGUI
  {
    /// <summary>
    /// Show console.
    /// </summary>
    /// <value>Value</value>
    public bool Show
    {
      get => show;
      set
      {
        if (value == true && show == false)
          needFocus = true;

        show = value;
      }
    }

    /// <summary>
    /// Is it initialized?
    /// </summary>
    /// <value>Value</value>
    public bool Initialized { get; set; }

    /// <summary>
    /// Should be updated?
    /// </summary>
    /// <value>True/false.</value>
    public bool ShouldUpdate { get; set; } = true;

    [SerializeField]
    private KeyCode showKey = KeyCode.Backslash;

    [SerializeField]
    private List<DevelopmentCommand> commands = new List<DevelopmentCommand>();

    private bool show;
    private string input;
    private float lastInputTime = -1.0f;
    private Vector2 scroll;
    private bool needFocus = true;

    private List<string> history = new List<string>();
    private int historyPointer = 0;

    private const string TextInputName = "ConsoleTextInput";

    /// <summary>
    /// When initialize.
    /// </summary>
    public void OnInitialize() { }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public void OnInitialized() { }

    /// <summary>
    /// When deinitialize.
    /// </summary>
    public void OnDeinitialize() { }

    /// <summary>
    /// Update event.
    /// </summary>
    public void OnUpdate() { }

    /// <summary>
    /// FixedUpdate event.
    /// </summary>
    public void OnFixedUpdate() { }

    /// <summary>
    /// LateUpdate event.
    /// </summary>
    public void OnLateUpdate()
    {
      if (Show == false && AcceptNewInput() == true && Input.GetKeyUp(showKey) == true)
      {
        lastInputTime = Time.time;

        Show = true;
      }
    }

    /// <summary>
    /// OnDrawGizmos event.
    /// </summary>
    public void OnGizmos() { }

    /// <summary>
    /// OnGUI event.
    /// </summary>
    public void OnGUI()
    {
      if (Show == true)
      {
        ProcessInput();
        
        ConsoleGUI();
      }
    }

    private bool AcceptNewInput() => Time.time - lastInputTime > 0.1f;
    
    private void ConsoleGUI()
    {
      GUILayout.BeginArea(new Rect(5, 5, Screen.width - 10, 28), GUI.skin.box);
      {
        GUILayout.BeginHorizontal();
        {
          GUI.SetNextControlName(TextInputName);
          input = GUILayout.TextField(input);

          if (GUILayout.Button("X", GUILayout.Width(30)) == true)
            Show = false;
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndArea();
      
      if (needFocus == true)
      {
        GUI.FocusControl(TextInputName);

        needFocus = false;
      }      
    }

    private void ProcessInput()
    {
      UnityEngine.Event e = UnityEngine.Event.current;
      if (e.type != EventType.Layout && e.type != EventType.Repaint)
      {
        if (e.keyCode == KeyCode.Return)
        {
          ProcessCommand();
          e.Use();
        }
        else if (e.keyCode == KeyCode.DownArrow && history.Count > 0 && historyPointer > 0)
        {
          historyPointer--;
          input = history[historyPointer];
          e.Use();
        }
        else if (e.keyCode == KeyCode.UpArrow && historyPointer < history.Count)
        {
          input = history[historyPointer];
          historyPointer++;
          e.Use();
        }
        else if (e.keyCode == KeyCode.Escape || e.keyCode == showKey)
        {
          Show = false;
          e.Use();
        }
      }
    }

    private void ProcessCommand()
    {
      if (string.IsNullOrEmpty(input) == false)
      {
        input = input.Trim();
        
        history.Add(input);
        historyPointer = 0;

        string[] parts = input.Trim().ToLower().Split(' ');
        if (parts.Length > 0)
        {
          string id = parts[0];
          DevelopmentCommand command = null;

          for (int i = 0; i < commands.Count && command == null; ++i)
          {
            if (id.Equals(commands[i].Id) == true)
              command = commands[i];
          }
          
          if (command != null)
          {
            if (command.Execute(parts.Sub(1, parts.Length - 1)) == false)
              Log.Warning($"Error executing command '{id}'.");
          }
          else
            Log.Warning($"Invalid command '{id}'.");
        }

        input = string.Empty;
      }
    }
  }
}
