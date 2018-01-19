using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Actions;
using Urho.Shapes;
using Urho.IO;
using Urho.Resources;

namespace UrhoSharpTutorial
{
    public class MyGame : Application
    {
        UrhoConsole console;
        DebugHud debugHud;
        ResourceCache cache;
        Sprite logoSprite;
        UI ui;

        static readonly Random random = new Random();
        public static float NextRandom() => (float)random.NextDouble();
        public static float NextRandom(float range) => (float)random.NextDouble() * range;
        public static float NextRandom(float min, float max) => (float)((random.NextDouble() * (max - min)) + min);
        public static int NextRandom(int min, int max) => random.Next(min, max);

        protected MonoDebugHud MonoDebugHud { get; set; }

        [Preserve]
        public MyGame(ApplicationOptions opts) : base(opts) { }

        static MyGame()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }//

        protected override void Start()
        {
            Log.LogMessage += e => Debug.WriteLine($"[{e.Level}] {e.Message}");
            base.Start();

            if (Platform == Platforms.iOS || Platform == Platforms.Android || Options.TouchEmulation)
            {
                InitTouchInput();
            }

            Input.Enabled = true;
            MonoDebugHud = new MonoDebugHud(this);
            MonoDebugHud.Show();

            CreateLogo();
            SetWindowAndTitleIcon();
            CreateConsoleAndDebugHud();

            // Subscribe to Esc key:
            Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });
        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }
        void InitTouchInput()
        {
            //TODO : init touch input.
        }
        void CreateLogo()
        {
            cache = ResourceCache;
            var logoTexture = cache.GetTexture2D("Textures/LogoLarge.png");
            if (logoTexture == null) return;

            ui = UI;
            logoSprite = ui.Root.CreateSprite();
            logoSprite.Texture = logoTexture;
            int w = logoTexture.Width;
            int h = logoTexture.Height;
            logoSprite.SetScale(256.0f / w);
            logoSprite.SetSize(w, h);
            logoSprite.SetHotSpot(0, h);
            logoSprite.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
            logoSprite.Opacity = 0.75f;
            logoSprite.Priority = -100;
        }
        void SetWindowAndTitleIcon()
        {
            var icon = cache.GetImage("Textures/UrhoIcon.png");
            Graphics.SetWindowIcon(icon);
            Graphics.WindowTitle = "Urho Sahrp Tutorial";
        }
        void CreateConsoleAndDebugHud()
        {
            var xml = cache.GetXmlFile("UI/DefaultStyle.xml");
            console = Engine.CreateConsole();
            console.DefaultStyle = xml;
            console.Background.Opacity = 0.8f;

            debugHud = Engine.CreateDebugHud();
            debugHud.DefaultStyle = xml;
        }


    }
}
