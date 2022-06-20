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
  /// Close console.
  /// </summary>
  [CreateAssetMenu(fileName = "GameObject", menuName = "Game:Work/Development/Command/GameObject")]
  public class GameObjectCommand : DevelopmentCommand
  {
    public GameObjectCommand()
    {
      Id = "gameobject";
      Usage = "gameobject name [destroy|activate|deactivate|move]";
      Description = "Operations on GameObjects.";
    }

    public override bool Execute(string[] args)
    {
      if (args.Length > 1)
      {
        string name = args[0];
        string command = args[1];

        GameObject gameObject = null;
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();  // @HACK: Active and inactive GameObjects.
        for (int i = 0; i < gameObjects.Length && gameObject == null; ++i)
        {
          if (name.Equals(gameObjects[i].name.ToLower()) == true)
            gameObject = gameObjects[i];
        }

        if (gameObject != null)
        {
          switch (command)
          {
            case "destroy":
              GameObject.Destroy(gameObject);
              return true;

            case "activate":
              gameObject.SetActive(true);
              return true;

            case "deactivate":
              gameObject.SetActive(false);
              return true;

            case "move":
              if (args.Length == 3)
              {
                string[] components = args[2].Split(',');
                if (components.Length == 3)
                {
                  Vector3 position = Vector3.zero;
                  float.TryParse(components[0], out position.x);
                  float.TryParse(components[1], out position.y);
                  float.TryParse(components[2], out position.z);

                  gameObject.transform.position = position;
                }
              }
              return true;
          }
        }
      }

      return false;
    }
  }
}
