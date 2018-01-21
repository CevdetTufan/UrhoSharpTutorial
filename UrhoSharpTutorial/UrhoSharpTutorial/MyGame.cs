using System;
using System.Diagnostics;
using Urho;
using Urho.Gui;
using Urho.IO;
using Urho.Resources;

namespace UrhoSharpTutorial
{
    public class MyGame : Application
    {
        private UrhoConsole console;
        private DebugHud debugHud;
        private ResourceCache cache;
        private Sprite logoSprite;
        private UI ui;

        protected Node CameraNode { get; set; }
        protected float Yaw { get; set; }
        protected float Pitch { get; set; }

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
        protected void SimpleMoveCamera3D(float timeStep, float moveSpeed = 10.0f)
        {
            if (UI.FocusElement != null) return;

            const float mouseSensitive = 0.1f;

            var mouseMove = Input.MouseMove;
            Yaw += mouseSensitive * mouseMove.X;
            Pitch += mouseSensitive * mouseMove.Y;

            Pitch = MathHelper.Clamp(Pitch, -90, 90);
            CameraNode.Rotation = new Quaternion(Pitch, Yaw, 0);

            if (Input.GetKeyDown(Key.W)) CameraNode.Translate(Vector3.UnitZ * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.S)) CameraNode.Translate(-Vector3.UnitZ * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.A)) CameraNode.Translate(-Vector3.UnitX * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.D)) CameraNode.Translate(Vector3.UnitX * moveSpeed * timeStep);
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
