using System.Collections.Generic;
using Urho;
using Urho.Gui;
using Urho.Urho2D;

namespace UrhoSharpTutorial
{
    public class Sprites : MyGame
    {
        private const int NumSprites = 100;
        private Dictionary<Sprite, Vector2> spritesWithVelocities = new Dictionary<Sprite, Vector2>();

        public Sprites(ApplicationOptions options) : base(options)
        {

        }

        protected override void Start()
        {
            base.Start();
            CreateSprites();
        }
        protected override void OnUpdate(float timeStep)
        {
            MoveSprites(timeStep);
            base.OnUpdate(timeStep);
        }
        private void CreateSprites()
        {
            var cache = ResourceCache;
            var graphics = Graphics;
            UI ui = UI;

            Texture2D decalTex = cache.GetTexture2D("Textures/UrhoDecal.dds");

            for (int i = 0; i < NumSprites; i++)
            {
                Sprite sprite = new Sprite();
                sprite.Texture = decalTex;

                sprite.Position = new IntVector2((int)(NextRandom() * graphics.Width), (int)(NextRandom() * graphics.Height));
                sprite.Size = new IntVector2(128, 128);
                sprite.HotSpot = new IntVector2(64, 64);

                sprite.Rotation = NextRandom() * 360.0f;
                sprite.SetScale(NextRandom(1.0f) + 0.5f);

                sprite.SetColor(new Color(NextRandom(0.5f) + 0.5f, NextRandom(0.5f) + 0.5f, NextRandom(0.5f) + 0.5f));
                sprite.BlendMode = BlendMode.Add;

                ui.Root.AddChild(sprite);

                spritesWithVelocities[sprite] = new Vector2(NextRandom(200.0f) - 100.0f, NextRandom(200.0f) - 100.0f);
            }
        }
        private void MoveSprites(float timeStep)
        {
            var graphic = Graphics;
            int width = graphic.Width;
            int height = graphic.Height;

            foreach (var item in spritesWithVelocities)
            {
                var sprite = item.Key;
                var vector = item.Value;

                float newRot = sprite.Rotation + timeStep * 60.0f;
                sprite.Rotation = newRot;

                var x = vector.X * timeStep + sprite.Position.X;
                var y = vector.Y * timeStep + sprite.Position.Y;

                if (x < 0.0f)
                    x += width;
                if (x >= width)
                    x -= width;
                if (y < 0.0f)
                    y += height;
                if (y >= height)
                    y -= height;

                sprite.Position = new IntVector2((int)x, (int)y);
            }
        }
    }
}

