﻿using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.Entities.Components.Internal;
using Andromeda.Serialization;
using Andromeda.System;
using Andromeda.Linq;
using System;
using Andromeda.System;

namespace Andromeda.Entities.Components
{
    /// <summary>
    /// User Interface Transform Component - Overrides the default Transform.
    /// </summary>
    [DisallowMultiple]
    public sealed class UITransform : Component, IUpdatableComponent
    {
        private Transform transform;

        UICoordinateMode sizeMode = UICoordinateMode.ParentXY;
        /// <summary>
        /// How the size is calculated
        /// </summary>
        public UICoordinateMode SizeMode
        {
            get => sizeMode;
            set => sizeMode = value;
        }

        /// <summary>
        /// The UserInterface this transform is attached to
        /// </summary>
        public UserInterface UserInterface
        {
            get
            {
                entity.Ancestors.FirstComponent(out UserInterface ui);
                return ui;
            }
        }

        UICoordinateMode posMode = UICoordinateMode.ParentXY;
        /// <summary>
        /// How the position is calculated
        /// </summary>
        public UICoordinateMode PositionMode
        {
            get => posMode;
            set => posMode = value;
        }

        [SerializableProperty("Size", PropertyType = SerializedPropertyType.UICoordinates)]
        public UICoordinates LocalSize
        {
            get;
            set;
        }

        [SerializableProperty("Position", PropertyType = SerializedPropertyType.UICoordinates)]
        public UICoordinates LocalPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the position based on alignment anchor rules
        /// </summary>
        /// <param name="anchor">The alignment anchor</param>
        /// <param name="offset">The offset from the anchor</param>
        public void SetAlignmentAnchor(UIPositionAnchor anchor, UICoordinates offset = default(UICoordinates))
        {
            SetAlignmentPosition((UIPositionAlign)anchor, offset);
        }
        
        /// <summary>
        /// Set the position based on alignment rules
        /// </summary>
        /// <param name="anchor">The alignment rules</param>
        /// <param name="offset">The UICoordinate offset</param>
        public void SetAlignmentPosition(UIPositionAlign anchor, UICoordinates offset = default(UICoordinates), bool relativeToParent = false)
        {
            var offsetAbsolute =  relativeToParent ? RelativeToParentSize(offset).GlobalAbsolute : offset.GlobalAbsolute;
            var offsetX = offsetAbsolute.X;
            var offsetY = offsetAbsolute.Y;
            float scaleX = 0;
            float scaleY = 0;

            if ((anchor & UIPositionAlign.Left) != 0)
            {
                scaleX = 0f;
            }
            else if ((anchor & UIPositionAlign.LeftCenter) != 0)
            {
                scaleX = 0.5f;
            }
            else if ((anchor & UIPositionAlign.Right) != 0)
            {
                scaleX = 1.0f;
            }
            
            if ((anchor & UIPositionAlign.Top) != 0)
            {
                scaleY = 0f;
            }
            else if ((anchor & UIPositionAlign.TopCenter) != 0)
            {
                scaleY = 0.5f;
            }
            else if ((anchor & UIPositionAlign.Bottom) != 0)
            {
                scaleY = 1.0f;
            }

            var absoluteSize = GlobalSize.GlobalAbsolute;

            if ((anchor & UIPositionAlign.CenterWidth) != 0)
            {
                offsetX = offsetX - (absoluteSize.X / 2);
            }

            if ((anchor & UIPositionAlign.CenterHeight) != 0)
            {
                offsetY = offsetY - (absoluteSize.Y / 2);
            }

            if ((anchor & UIPositionAlign.InverseHeight) != 0)
            {
                offsetY = offsetY - absoluteSize.Y;
            }

            if ((anchor & UIPositionAlign.InverseWidth) != 0)
            {
                offsetX = offsetX - absoluteSize.X;
            }

            LocalPosition = new UICoordinates(scaleX, offsetX, scaleY, offsetY);
        }

        internal UICoordinates RelativeToSize(UICoordinates size, UICoordinates position)
        {
            return new UICoordinates(
                new UIAxis(0, ( position.X.Scale * size.GlobalAbsolute.X ) + position.X.Offset),
                new UIAxis(0, ( position.Y.Scale * size.GlobalAbsolute.Y ) + position.Y.Offset)
            );
        }

        private UICoordinates RelativeToParentSize(UICoordinates position)
        {
            var parentTransform = Entity?.Parent?.GetComponent<UITransform>();
            if (parentTransform != null && posMode == UICoordinateMode.ParentXY)
            {
                return RelativeToSize(parentTransform.LocalSize, position);
            }
            else
            {
                return LocalSize;
            }
        }

        /// <summary>
        /// The size relative to the parent
        /// </summary>
        public UICoordinates GlobalSize
        {
            get
            {
                var parentTransform = Entity?.Parent?.GetComponent<UITransform>();
                if (parentTransform != null && posMode == UICoordinateMode.ParentXY)
                {
                    return RelativeToSize(parentTransform.LocalSize, LocalSize);
                }
                else
                {
                    return LocalSize;
                }
            }
        }

        /// <summary>
        /// The position relative to the parent
        /// </summary>
        public UICoordinates GlobalPosition
        {
            get
            {
                var parentTransform = Entity?.Parent?.GetComponent<UITransform>();
                if (parentTransform != null && posMode == UICoordinateMode.ParentXY)
                {
                    var rel = RelativeToSize(parentTransform.LocalSize, LocalPosition);
                    var result = parentTransform.GlobalPosition + rel; //Position + parentTransform.PositionRelative;
                    return result;
                }
                else
                {
                    return LocalPosition;
                }
            }
        }

        /// <summary>
        /// The size relative to the parent
        /// </summary>
        internal UICoordinates SizeRelative
        {
            get
            {
                var parentTransform = Entity?.Parent.GetComponent<UITransform>();
                if (parentTransform != null && sizeMode == UICoordinateMode.ParentXY)
                {
                    return LocalSize * parentTransform.SizeRelative.GlobalAbsolute;
                }
                else
                {
                    return LocalSize;
                }
            }
        }

        public void Update()
        {
            // We want to update the transform to reflect this ;)
            Entity.Transform.Position = GlobalPosition.GlobalAbsolute;
            Entity.Transform.Origin = new Vector2f(0, 0);
        }

        protected override void OnComponentInit(Entity entity)
        {
           // Create the transform if it doesn't exist.
           entity.FindComponent(out transform, true);
           LocalPosition = new UICoordinates(0, 0, 0, 0);
           LocalSize = new UICoordinates(0, 0, 0, 0);
        }

        public void AfterUpdate()
        {
            
        }

        public void BeforeUpdate()
        {
            
        }

        public override void OnComponentCopy(Entity source, Entity copy)
        {
            var copyTransform = copy.AddComponent<UITransform>();
            copyTransform.LocalPosition = LocalPosition;
            copyTransform.LocalSize = LocalSize;
        }

        public override string ToString()
        {
            return  "UITransform - Position: " + LocalPosition + ", Size: " + LocalSize;
        }

        public UpdatePriority UpdatePriority => UpdatePriority.Interface;
    }
}
