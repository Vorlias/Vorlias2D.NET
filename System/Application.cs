﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace VorliasEngine2D.System
{
    /// <summary>
    /// An application
    /// </summary>
    public class Application
    {
        RenderWindow window;
        VideoMode mode;
        string title;
        Styles styles;
        float deltaTime;
        float fps;

        /// <summary>
        /// The delta time of the window
        /// </summary>
        public float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

        /// <summary>
        /// The TextureManager object
        /// </summary>
        public TextureManager TextureManager
        {
            get
            {
                return TextureManager.Instance;
            }
        }

        /// <summary>
        /// The framerate of the window
        /// </summary>
        public float FPS
        {
            get
            {
                return fps;
            }
        }

        /// <summary>
        /// The render window
        /// </summary>
        public RenderWindow Window
        {
            get
            {
                return window;
            }
        }

        /// <summary>
        /// Resize the window
        /// </summary>
        public Vector2u Size
        {
            get
            {
                return window.Size;
            }
            set
            {
                window.Size = value;
            }
        }

        /// <summary>
        /// The VideoMode of the application
        /// </summary>
        public VideoMode VideoMode
        {
            get
            {
                return mode;
            }
        }

        /// <summary>
        /// The title of the Application
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                window.SetTitle(value);
            }
        }

        public Styles Styles
        {
            get
            {
                return styles;
            }
        }

        public Application(VideoMode mode, string title, Styles styles = Styles.Default)
        {
            this.mode = mode;
            this.title = title;
            this.styles = styles;
        }
        
        /// <summary>
        /// Method called when the game ends (Window is closed)
        /// </summary>
        protected virtual void End()
        {

        }

        /// <summary>
        /// Method called when the game starts
        /// </summary>
        protected virtual void Start()
        {

        }

        /// <summary>
        /// Function called on update
        /// </summary>
        protected virtual void Update()
        {

        }

        /// <summary>
        /// Function called on render
        /// </summary>
        protected virtual void Render()
        {

        }

        protected virtual void BeforeStart()
        {

        }

        /// <summary>
        /// Runs the application
        /// </summary>
        public void Run()
        {
            Clock deltaClock = new Clock();
            window = new RenderWindow(mode, title, styles);
            window.Closed += WindowClosed;

            BeforeStart();
            Start();

            while (window.IsOpen)
            {
                deltaTime = deltaClock.ElapsedTime.AsSeconds();
                deltaClock.Restart();
                
                fps = 1.0f / deltaTime;

                window.Clear();

                window.DispatchEvents();

                Update();

                Render();

                window.Display();
            }

            End();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        public void Close()
        {
            window.Close();
        }

        /// <summary>
        /// Called when the window close is requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void WindowClosed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
