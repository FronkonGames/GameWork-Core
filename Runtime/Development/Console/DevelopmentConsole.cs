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
                                    IGUI
  {
    /// <summary>
    /// Show console.
    /// </summary>
    /// <value>Value</value>
    public bool Show { get; set; }

    /// <summary>
    /// Is it initialized?
    /// </summary>
    /// <value>Value</value>
    public bool Initialized { get; set; }

    private string consoleInput;

    private List<string> output = new List<string>();

    private Dictionary<string, DevelopmentCommandBase> commands = new Dictionary<string, DevelopmentCommandBase>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    public void AddCommand(DevelopmentCommandBase command)
    {
      if (commands.ContainsKey(command.Id) == false)
        commands.Add(command.Id, command);
      else
        Log.Error($"Command '{command.Id}' already exists.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    public void RemoveCommand(DevelopmentCommand command) => RemoveCommand(command.Id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    public void RemoveCommand(string id)
    {
      if (commands.ContainsKey(id) == true)
        commands.Remove(id);
      else
        Log.Error($"Command '{id}' not found.");
    }

    public void ClearCommands() => commands.Clear();

    /// <summary>
    /// When initialize.
    /// </summary>
    public void OnInitialize()
    {
    }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public void OnInitialized()
    {
      AddCommand(new DevelopmentCommand("?", "List all available commands.", () => { }));
    }

    /// <summary>
    /// When deinitialize.
    /// </summary>
    public void OnDeinitialize()
    {
      ClearCommands();
    }

    /// <summary>
    /// Ondrawgizmos event.
    /// </summary>
    public void OnDrawGizmos()
    {
    }

    /// <summary>
    /// OnGUI event.
    /// </summary>
    public void OnGUI()
    {
      if (Show == true)
      {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), string.Empty);
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), string.Empty);

        string input = GUI.TextField(new Rect(0, 0, Screen.width, 24), consoleInput);

        const float y = 24.0f;
        GUI.Box(new Rect(0, y, Screen.width, Screen.height - 24), "");

        // ...

        consoleInput = input;

        UnityEngine.Event e = UnityEngine.Event.current;
        if (e.isKey == true)
        {
          if (e.keyCode == KeyCode.Escape)
            Show = false;
        }
      }
    }
  }
}
