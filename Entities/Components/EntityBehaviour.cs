﻿using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VorliasEngine2D.Entities.Components.Internal;
using VorliasEngine2D.System;
using VorliasEngine2D.System.Utility;

namespace VorliasEngine2D.Entities.Components
{
    /// <summary>
    /// Base class for custom scripted components
    /// </summary>
    public class EntityBehaviour : Component
    {
        private bool initialized = false;

        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        /// <summary>
        /// Access the InputManager for this entity
        /// </summary>
        public UserInputManager Input
        {
            get
            {
                return entity.Input;
            }
        }

        public Vector2f MousePosition
        {
            get
            {
                Vector2i mousePosition = Mouse.GetPosition(Application.Window);
                return new Vector2f(mousePosition.X, mousePosition.Y);
            }
        }

        /// <summary>
        /// Return the StateApplication
        /// </summary>
        public StateApplication Application
        {
            get
            {
                return StateApplication.Application;
            }
        }

        /// <summary>
        /// The SpriteRenderer associated with the entity
        /// </summary>
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return entity.SpriteRenderer;
            }
        }

        /// <summary>
        /// The Transform associated with the entity
        /// </summary>
        public Transform Transform
        {
            get
            {
                return entity.Transform;
            }
        }

        /// <summary>
        /// Called when the component is initialized
        /// </summary>
        public virtual void Init()
        {

        }

        public virtual void Start()
        {

        }

        /// <summary>
        /// Called when the entity is updated
        /// </summary>
        public virtual void Update()
        {

        }

        public virtual void Collision(Entity other)
        {

        }

        /// <summary>
        /// Called when the entity is rendered
        /// </summary>
        public virtual void Render()
        {
           
        }

        /// <summary>
        /// Called before the child entities are updated
        /// </summary>
        public virtual void BeforeUpdate()
        {

        }

        /// <summary>
        /// Called after the child entities are updated
        /// </summary>
        public virtual void AfterUpdate()
        {

        }

        /// <summary>
        /// Called when the component is added to an entity
        /// </summary>
        /// <param name="entity">The entity it is added to</param>
        /// <exception cref="SetEntityInvalidException">Called if the user tries to set it</exception>
        public override void OnComponentInit(Entity entity)
        {
            Init();
        }

        public override string Name
        {
            get
            {
                return "EntityBehaviour";
            }
        }

        public override bool AllowsMultipleInstances
        {
            get
            {
                return true;
            }
        }

        public new virtual void OnComponentCopy(Entity source, Entity copy)
        {
            // Do nothing, this is a custom component.
        }
    }
}
