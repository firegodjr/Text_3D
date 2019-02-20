using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Text_3D_Engine.Input
{
    public enum MouseAxis
    {
        X, Y
    }

    public struct KeyAction
    {
        /// <summary>
        /// The key that triggers the action
        /// </summary>
        public Keys Key;
        /// <summary>
        /// The action to be performed on keypress
        /// </summary>
        public Action Action;

        public KeyAction(Keys key, Action action)
        {
            this.Key = key;
            this.Action = action;
        }
    }

    public delegate void MouseAxisAction(int mouseAxis);
    
    public class InputHandler
    {
        private Dictionary<Keys, Action> keyActions = new Dictionary<Keys, Action>();
        private Dictionary<MouseAxis, List<MouseAxisAction>> mouseActions = new Dictionary<MouseAxis, List<MouseAxisAction>>();

        private bool acceptInput = true;

        private const int KeyPressed = 0x8000;

        public void RegisterKeyAction(Keys key, Action action)
        {
            if(keyActions.ContainsKey(key))
            {
                keyActions[key] = action;
            }
            else
            {
                keyActions.Add(key, action);
            }
        }

        public void RegisterKeyActions(params KeyAction[] keyActions)
        {
            foreach(KeyAction ka in keyActions)
            {
                RegisterKeyAction(ka.Key, ka.Action);
            }
        }

        public void RegisterMouseMovementAction(MouseAxis axis, MouseAxisAction action)
        {
            if (mouseActions.ContainsKey(axis))
            {
                mouseActions[axis].Add(action);
            }
            else
            {
                mouseActions.Add(axis, new List<MouseAxisAction>());
                mouseActions[axis].Add(action);
            }
        }

        public void EnableInput()
        {
            acceptInput = true;
        }

        public void DisableInput()
        {
            acceptInput = false;
        }

        public static bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetKeyState(int key);

        public void PerformKeyActions()
        {
            if (!acceptInput) return;

            foreach (Keys keyCode in keyActions.Keys)
            {
                if(IsKeyDown(keyCode))
                {
                    keyActions[keyCode]();
                }
            }
        }

        public void PerformMouseActions(int windowWidth, int windowHeight)
        {
            if (!acceptInput) return;

            foreach (MouseAxis axis in mouseActions.Keys)
            {
                foreach(MouseAxisAction action in mouseActions[axis])
                {
                    action(axis == MouseAxis.X ? Cursor.Position.X - (windowWidth / 2) : Cursor.Position.Y - (windowHeight / 2));
                }
            }

            ResetMousePosition(windowWidth, windowHeight);
        }

        public void ResetMousePosition(int windowWidth, int windowHeight)
        {
            Cursor.Position = new Point(windowWidth / 2, windowHeight / 2);
        }

        public void PerformInputActions(int windowWidth, int windowHeight)
        {
            PerformKeyActions();
            PerformMouseActions(windowWidth, windowHeight);
        }
    }
}
