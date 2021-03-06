﻿using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.Entities.Components.Internal;
using Andromeda.Serialization;
using Andromeda.System;
using Andromeda.System;

namespace Andromeda.Entities.Components
{
    [DisallowMultiple]
    public sealed class SpriteRenderer : TextureComponent
    {
        private AnchorPoint renderAnchor = new AnchorPoint(0.5f, 0.5f);

        /// <summary>
        /// The origin for rendering
        /// </summary>
        public AnchorPoint RenderOriginAnchor
        {
            get
            {
                return renderAnchor;
            }
            set
            {
                renderAnchor = value;
            }
        }

        Vector2f scale = new Vector2f(1, 1);

        /// <summary>
        /// The scale of this sprite
        /// </summary>
        public Vector2f Scale
        {
            get => scale;
            set => scale = value;
        }

        public Vector2f Origin
        {
            get
            {
                return renderAnchor.AppliedTo(this);
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            //target.SetView(StateApplication.Application.WorldView);

            Transform transform = entity.Transform;
            Vector2f position = transform.Position;

            if (Texture == null)
            {
                RectangleShape rs = new RectangleShape(new Vector2f(100, 100));
                rs.Origin = new Vector2f( 100 * renderAnchor.X, 100 * renderAnchor.Y);
                rs.Position = transform.Position;
                target.Draw(rs);
            }
            else
            {
                Sprite sprite = new Sprite(Texture);
                sprite.Origin = new Vector2f(Texture.Size.X * renderAnchor.X, Texture.Size.Y * renderAnchor.Y);
                sprite.Position = transform.Position;
                sprite.Scale = scale;
                sprite.Rotation = transform.Rotation;
                sprite.Color = Color;
                target.Draw(sprite);
            }
        }

        public override void OnComponentCopy(Entity source, Entity copy)
        {
            SpriteRenderer renderer = copy.AddComponent<SpriteRenderer>();

            // Set the texture to be the same
            if (Texture != null)
                renderer.Texture = Texture;
        }
    }
}
