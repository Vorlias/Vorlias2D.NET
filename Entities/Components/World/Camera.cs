﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Andromeda2D.Entities.Components.Internal;
using SFML.System;
using Andromeda2D.System;
using Andromeda2D.System.Utility;

namespace Andromeda2D.Entities.Components
{
    public class ScaledResolution
    {
        public Vector2f Resolution
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }
    }

    public class Camera : Component, IUpdatableComponent
    {
        public override bool AllowsMultipleInstances => false;

        View view;
        public View View
        {
            get => view;
        }

        CameraType cameraType = CameraType.World;
        public CameraType CameraType
        {
            get => cameraType;
            set
            {
                if (value == CameraType.Interface)
                {
                    updatePriority = UpdatePriority.Interface - 1;
                    cameraType = value;
                }
                else
                {
                    updatePriority = UpdatePriority.Camera;
                    cameraType = value;
                }
            }
        }

        ScaledResolution _res = new ScaledResolution();
        public ScaledResolution ScaledResolution
        {
            get => _res;
        }

        /// <summary>
        /// The position of the camera relative to the world's center
        /// </summary>
        public Vector2f WorldPosition
        {
            get => Entity.Transform.Position;
            set => Entity.Transform.Position = value;
        }

        /// <summary>
        /// The position on the camera where the world's center (0, 0) is
        /// </summary>
        internal Vector2f WorldZeroPosition
        {
            get => Entity.Transform.Position - View.Center;
        }

        private UpdatePriority updatePriority = UpdatePriority.Camera;
        public UpdatePriority UpdatePriority => updatePriority;

        public RenderOrder RenderOrder
        {
            get => RenderOrder.Camera;
            set => throw new NotImplementedException();
        }

        public override void OnComponentInit(Entity entity)
        {
            StateApplication app = StateApplication.Application;

            entity.AddComponent<Transform>();
        }

        public void AfterUpdate()
        {
            
        }

        public void BeforeUpdate()
        {
            
        }

        /// <summary>
        /// Resets the camera position and rotation
        /// </summary>
        public void Reset()
        {
            WorldPosition = new Vector2f(0, 0);
            Entity.Transform.Rotation = 0;
        }

        public void Update()
        {
            var application = StateApplication.Application;
            var window = application.Window;
            var windowSize = window.Size.ToFloat();

            if (cameraType == CameraType.Interface)
            {
                if (view == null)
                    view = new View(application.InterfaceView);

                view.Rotation = 0;
                view.Center = application.Window.Size.ToFloat() / 2;
            }
            else
            {
                if (view == null)
                    view = new View(application.WorldView);

                if (ScaledResolution.Enabled)
                {
                    view.Size = new Vector2f(
                        ((windowSize.X / ScaledResolution.Resolution.X) * ScaledResolution.Resolution.X), 
                        ((windowSize.Y / ScaledResolution.Resolution.Y) * ScaledResolution.Resolution.Y)
                    ) * (ScaledResolution.Resolution.Y / windowSize.Y); //ScaledResolution.Resolution / 2.0f;
                }

                view.Center = WorldPosition;
                view.Rotation = Entity.Transform.LocalRotation;
            }
                
            application.Window.SetView(view);
        }

        public Vector2f ScreenToWorldPoint(Vector2f screenPoint)
        {
            var window = StateApplication.Application.Window;
            var windowSize = window.Size.ToFloat();

            if (ScaledResolution.Enabled)
            {
                var relativeSize = new Vector2f(view.Size.X * 2 / windowSize.X, view.Size.Y * 2 / windowSize.Y);
                return new Vector2f(screenPoint.X * relativeSize.X, screenPoint.Y * relativeSize.Y);
            }
            else
            {
                return screenPoint;
            }
        }
    }
}
