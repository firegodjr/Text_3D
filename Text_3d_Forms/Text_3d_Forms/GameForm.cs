using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Text_3D_Engine.Input;
using Text_3D_Engine;
using Text_3D_Renderer;
using Text_3D_Renderer.Rendering;
using Text_3D_Renderer.World;

namespace Text_3D_Engine
{
    public partial class GameWindow : Form
    {
        public static int framecount;
        public static Game game = new Game();
        public static Viewport renderer;
        public static InputHandler InputHandlerGame = new InputHandler();
        public static InputHandler InputHandlerMenu = new InputHandler();
        public static TextMenu mainMenu;
        Stopwatch gunTimer = new Stopwatch();

        // This is the constructor for the GameForm!
        // Put stuff that happens before the game starts in here
        // Like registering objects
        // Or registering key actions
        public GameWindow()
        {
            // Create and register the game viewport
            game.RegisterViewport("renderer1", new Viewport(80, 45, aspect: 1.3f, fardist: 30, showFPS: true));
            renderer = game.ViewportRegistry["renderer1"];

            // Defines a main menu with two options.
            mainMenu = new TextMenu("Main Menu", 
                new TextMenuAction("Start", () => { mainMenu.Visible = false; InputHandlerGame.ResetMousePosition(Width, Height); }), 
                new TextMenuAction("Load From Save", () => { /* TODO: actual saving lol */ }));
            mainMenu.Visible = true;

            InitializeComponent();
            double posX = Program.settings.GetValue<double>("posX");
            double posY = Program.settings.GetValue<double>("posY");
            double posZ = Program.settings.GetValue<double>("posZ");
            renderer.Camera.SetPosition(posX, posY, posZ);

            double pitch = Program.settings.GetValue<double>("pitch");
            double yaw = Program.settings.GetValue<double>("yaw");
            double roll = Program.settings.GetValue<double>("roll");
            renderer.Camera.SetRotation(pitch, yaw, roll);

            // Sets the name of the renderer. You don't really have to do this unless you're juggling multiple worldspace objects, and need worldobjects to know the difference.
            renderer.Worldspace.Name = "main";

            // Initialize text layers
            InitTextLayers("zig_____.ttf", "Zig");

            //Creates all the objects that make up the area
            renderer.Worldspace.RegisterWorldObject(new WOQuad("red").Translate(0, 0, 1).SetColor(Layer.RED));
            renderer.Worldspace.RegisterWorldObject(new WOQuad("yellow").Translate(0, 0, 2).SetColor(Layer.YELLOW));
            renderer.Worldspace.RegisterWorldObject(new WOQuad("green").Translate(0, 0, 3).SetColor(Layer.GREEN));
            renderer.Worldspace.RegisterWorldObject(new WOQuad("blue").Translate(0, 0, 4).SetColor(Layer.BLUE));
            renderer.Worldspace.RegisterWorldObject(new WOQuad("white").Translate(0, 0, 5).SetColor(Layer.WHITE));

            // Registers all the actions that happen when you press keys with the KeyHandler object.
            // You write it in the form RegisterKeyAction(Key, Action)
            // Action is a delegate, you write it like () => { code; }
            InputHandlerGame.RegisterKeyActions(
                new KeyAction(Keys.W, () => { renderer.Worldspace.Camera.Translate(renderer.Worldspace.Camera.GetForwardDirection() * (renderer.Deltatime * 0.01)); }),
                new KeyAction(Keys.A, () => { renderer.Worldspace.Camera.Translate(-renderer.Worldspace.Camera.GetRightDirection() * (renderer.Deltatime * 0.01)); }),
                new KeyAction(Keys.S, () => { renderer.Worldspace.Camera.Translate(-renderer.Worldspace.Camera.GetForwardDirection() * (renderer.Deltatime * 0.01)); }),
                new KeyAction(Keys.D, () => { renderer.Worldspace.Camera.Translate(renderer.Worldspace.Camera.GetRightDirection() * (renderer.Deltatime * 0.01)); }),
                new KeyAction(Keys.NumPad7, () => { renderer.Projection.NearDist += 0.1; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad4, () => { renderer.Projection.NearDist -= 0.1; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad8, () => { renderer.Projection.FarDist += 0.1; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad5, () => { renderer.Projection.FarDist -= 0.1; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad9, () => { renderer.Projection.Fov += 0.01; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad6, () => { renderer.Projection.Fov -= 0.01; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad2, () => { renderer.Projection.Aspect += 0.01; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.NumPad1, () => { renderer.Projection.Aspect -= 0.01; renderer.RecalculateProjection(); }),
                new KeyAction(Keys.Escape, () => { Close(); })
            );

            InputHandlerGame.RegisterMouseMovementAction(MouseAxis.X, (int axis) => { renderer.Camera.Rotate(0, Math.PI / 180 * -axis * (renderer.Deltatime / 100), 0); });
            InputHandlerGame.RegisterMouseMovementAction(MouseAxis.Y, (int axis) => { renderer.Camera.Rotate(Math.PI / 180 * axis * (renderer.Deltatime / 100), 0, 0); });

            // Sets the cursor to the center of the screen to prepare it for mouse movement tracking
            Cursor.Position = new Point(Width / 2, Height / 2);
            Cursor.Hide();

            // Starts rendering frames to the screen.
            renderer.StartRenderThread();
            renderTick.Start();
        }
        
        string[] layers;
        // Aggressively tries to write strings to the labels if they're available
        private void DoGameTick(object sender, EventArgs e)
        {
            // Get all the strings from the renderer, then set the label text to the strings
            // if (renderer != null && renderer.FrameReady)
            {
                if(renderer.StringLayers.Length == renderer.screenMgr.LayerAmnt + 1)
                {
                    layers = renderer.StringLayers;
                    SetTextLayers(layers);

                    renderer.NoteFrameRecieved();

                    //Run correct key handler
                    if (mainMenu.Visible)
                    {
                        mainMenu.HandleInput();
                        mainMenu.Draw(renderer.screenMgr);
                    }
                    else
                    {
                        InputHandlerGame.PerformInputActions(Width, Height);
                    }
                }

                foreach(Label c in Controls)
                {
                    c.Refresh();
                }
            }


            // Does all the key actions for the keys that are currently pressed
            // These are the actions we just defined in the constructor :D

            //TODO: encapsulate; worldspace-wide update() method
        }

        //Prepares all the text layers for the form.
        //Make sure the font file "zig______.ttf" is included with the .exe when you share the program
        private void InitTextLayers(string fontname, string name)
        {
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(Application.StartupPath + "\\" + fontname);
            FontFamily font = new FontFamily(name, collection);

            for(int i = 0; i < Controls.Count; ++i)
            {
                Label l = (Label)Controls[i];
                l.Font = new Font(font, 8);
                l.BackColor = Color.Transparent;
            }

            TextLayerWhite.Parent = this;
            TextLayerGray.Parent = TextLayerWhite;
            TextLayerRed.Parent = TextLayerGray;
            TextLayerGreen.Parent = TextLayerRed;
            TextLayerBlue.Parent = TextLayerGreen;
            TextLayerYellow.Parent = TextLayerBlue;
            TextLayerText.Parent = TextLayerYellow;
        }

        //Sets all the text (string) layers using an existing array of strings
        private void SetTextLayers(string[] layers)
        {
            TextLayerWhite.Text = layers[ScreenMgr.WHITE];
            TextLayerGray.Text = layers[ScreenMgr.GRAY];
            TextLayerRed.Text = layers[ScreenMgr.RED];
            TextLayerGreen.Text = layers[ScreenMgr.GREEN];
            TextLayerBlue.Text = layers[ScreenMgr.BLUE];
            TextLayerYellow.Text = layers[ScreenMgr.YELLOW];
            TextLayerText.Text = layers[ScreenMgr.TEXT];
        }

        #region Overloadable Form Events

        public virtual void GameForm_ResizeEnd(object sender, EventArgs e)
        {

        }

        public virtual void GameForm_ResizeBegin(object sender, EventArgs e)
        {

        }

        #endregion

        #region Private Form Events

        private void GameForm_Activated(object sender, EventArgs e)
        {

        }

        private void GameForm_Deactivate(object sender, EventArgs e)
        {

        }

        #endregion

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Vector3 pos = renderer.Camera.GetPosition();
            Vector3 rotation = renderer.Camera.GetRotation();
            Program.settings.Set("posX", pos.X);
            Program.settings.Set("posY", pos.Y);
            Program.settings.Set("posZ", pos.Z);
            Program.settings.Set("pitch", rotation.X);
            Program.settings.Set("yaw", rotation.Y);
            Program.settings.Set("roll", rotation.Z);
            game.Shutdown();
        }
    }
}
