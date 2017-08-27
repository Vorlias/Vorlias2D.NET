﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vorlias2D.Entities.Components.Internal;
using Vorlias2D.System;
using Vorlias2D.System.Debug;

namespace Vorlias2D.Entities.Components.UI
{
    /// <summary>
    /// Abstract UI class for interactive elements, like buttons etc.
    /// </summary>
    public abstract class UIInteractable : UIComponent, IComponentEventListener
    {
        public event InterfaceEvent OnMouseEnter, OnMouseLeave;

        public override abstract string Name
        {
            get;
        }

        private bool mouseDown = false;

        /// <summary>
        /// Whether or not the mouse is down
        /// </summary>
        public bool IsMouseDown
        {
            get
            {
                return mouseDown;
            }
        }

        /// <summary>
        /// Used to determine collision between button and mouse (For hover detection)
        /// </summary>
        public PolygonRectCollider ButtonCollider
        {
            get
            {
                return Entity.GetComponent<PolygonRectCollider>();
            }
        }

        public abstract override void OnComponentCopy(Entity source, Entity copy);

        public abstract void OnButtonInit(Entity entity);

        public override void OnComponentInit(Entity entity)
        {
            var rectCollider = Entity.AddComponent<PolygonRectCollider>();
            rectCollider.CreateRectCollider(new Vector2f(100, 20));

            OnButtonInit(entity);
        }

        public abstract override void Draw(RenderTarget target, RenderStates states);

        /// <summary>
        /// Called when the mouse is clicked on the button
        /// </summary>
        /// <param name="inputAction">The mouse input action</param>
        public abstract void ButtonClick(MouseInputAction inputAction);

        public void InputRecieved(UserInputAction inputAction)
        {
            var mouseAction = inputAction.Mouse;
            if (mouseAction?.InputState == InputState.Active && mouseAction?.Button == Mouse.Button.Left)
            {
                // if left click
                if (IsMouseOver && !IsMouseDown)
                {
                    mouseDown = true;
                    ButtonClick(mouseAction);
                }
            }
            else if (mouseAction?.InputState == InputState.Inactive && mouseAction.Button == Mouse.Button.Left)
            {
                mouseDown = false;
            }
        }

        bool hoverState = false;
        public override void Update()
        {
            if (IsMouseOver && !hoverState)
            {
                hoverState = true;
                OnMouseEnter?.Invoke(new UserInterfaceAction(UIActionType.MouseEnter, this));
            }
            else if (!IsMouseOver && hoverState)
            {
                hoverState = false;
                OnMouseLeave?.Invoke(new UserInterfaceAction(UIActionType.MouseLeave, this));
            }
        }
    }
}
