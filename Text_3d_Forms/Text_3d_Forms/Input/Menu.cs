using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Text_3D_Renderer;
using Text_3D_Renderer.Rendering;

namespace Text_3D_Engine.Input
{
    public class TextMenu
    {
        string title;
        int selectedIndex;
        bool visible;
        int msInputDelay = 50;

        Stopwatch inputdelay;

        TextMenuAction[] menuOptions;
        InputHandler inputHandler;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public TextMenu(string title, params TextMenuAction[] menuOptions)
        {
            this.title = title;
            this.menuOptions = menuOptions;
            visible = false;

            inputdelay = new Stopwatch();
            inputdelay.Start();

            inputHandler = new InputHandler();
            inputHandler.RegisterKeyAction(Keys.W, () => MoveCursor(-1));
            inputHandler.RegisterKeyAction(Keys.S, () => MoveCursor(1));
            inputHandler.RegisterKeyAction(Keys.Space, () => Confirm());
        }

        /// <summary>
        /// Tells the menu to process keyboard inputs once
        /// </summary>
        public void HandleInput()
        {
            inputHandler.PerformKeyActions();
        }

        public bool MoveTo(int option)
        {
            if(inputdelay.ElapsedMilliseconds > msInputDelay)
            {
                inputdelay.Restart();
                if (option >= menuOptions.Length || option < 0)
                {
                    return false;
                }
                else
                {
                    selectedIndex = option;
                    return true;
                }
            }

            return false;
        }

        public void MoveCursor(int xMovement)
        {
            if(inputdelay.ElapsedMilliseconds > msInputDelay)
            {
                inputdelay.Restart();
                if ((xMovement > 0 && selectedIndex < menuOptions.Length - 1) || (xMovement < 0 && selectedIndex > 0))
                {
                    selectedIndex += xMovement;
                }
            }
        }

        public void Confirm()
        {
            menuOptions[selectedIndex].Action();
        }

        public void SetVisible(bool value)
        {
            visible = value;
        }

        public void SetInputDelayTimer(int delay)
        {
            msInputDelay = delay;
        }

        /// <summary>
        /// Draws the menu to the screenMgr for processing and rendering
        /// </summary>
        /// <param name="mgr"></param>
        public void Draw(ScreenMgr mgr)
        {
            int centerX = mgr.Width / 2;
            int centerY = mgr.Height / 2;

            mgr.AddTextToDraw(new Coords(centerX - title.Length / 2, centerY + menuOptions.Length + 4), title);

            for(int i = 0; i < menuOptions.Length; ++i)
            {
                string s = (i == selectedIndex ? $"[ {menuOptions[i].Text} ]" : menuOptions[i].Text);
                mgr.AddTextToDraw(new Coords(centerX - s.Length / 2, centerY + menuOptions.Length - i*2), s);
            }
        }
    }

    public class TextMenuAction
    {
        Action action;
        string optiontext;

        public Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public string Text
        {
            get { return optiontext; }
            set { optiontext = value; }
        }

        public TextMenuAction(string optiontext, Action action)
        {
            this.action = action;
            this.optiontext = optiontext;
        }
    }
}
