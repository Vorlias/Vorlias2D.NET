﻿using Andromeda2D.Entities;
using Andromeda2D.Entities.Components.Internal;
using Andromeda2D.System;

namespace Andromeda.Entities.Components
{
    /// <summary>
    /// Similar to Controller, but used for objects that are controlled by the user
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class UserController<TModel> : Controller<TModel>
        where TModel : IModel
    {
        protected UserInputManager Input => entity.Input;
        protected MouseCoordinates MouseCoordinates => new MouseCoordinates(StateApplication.Application, Entity.GameView);

        /// <summary>
        /// Used to initialize things like input, etc.
        /// </summary>
        protected abstract void InitController();

        protected override void OnComponentInit(Entity entity)
        {
            InitModel();
            InitController();
        }
    }
}
