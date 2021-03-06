﻿using Andromeda.Entities;
using Andromeda.Entities.Components;
using Andromeda.Entities.Components.Internal;
using Andromeda.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Entities.Models
{

    /// <summary>
    /// A controller for a model
    /// </summary>
    /// <typeparam name="TModel">The model type</typeparam>
    public abstract class Controller<TModel> : Component
        where  TModel : IModel
    {
        Entity _entity;

        /// <summary>
        /// Sets the model of the controller
        /// </summary>
        /// <param name="model">The model to set to this controller</param>
        protected void SetControllerModel(TModel model)
        {
            this._model = model;
        }

        private TModel _model;

        /// <summary>
        /// The model of this controller
        /// </summary>
        public TModel Model
        {
            get => _model;
        }

        protected abstract void InitModel();

        protected override void OnComponentInit(Entity entity)
        {
            InitModel();
        }
    }
}
