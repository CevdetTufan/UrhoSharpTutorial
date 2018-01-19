using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Urho2D;

namespace UrhoSharpTutorial
{
    public class Sprites : MyGame
    {
        private const int NumSprites = 100;
        public Sprites(ApplicationOptions options) : base(options)
        {

        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void OnUpdate(float timeStep)
        {
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
            }
        }
    }
}
