using Urho;
using Urho.Gui;

namespace UrhoSharpTutorial
{
    public class HelloWorld : Application
    {
        public HelloWorld(ApplicationOptions options = null) : base(options) { }

        protected override void Start()
        {
            var cache = ResourceCache;
            var helloText = new Text()
            {
                Value = "Hello World from UrhoSharp",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            helloText.SetColor(new Color(0f, 1f, 0f));
            helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);
            UI.Root.AddChild(helloText);

            Graphics.SetWindowIcon(cache.GetImage("Textures/UrhoIcon.png"));
            Graphics.WindowTitle = "UrhoSharp Sample";

            // Subscribe to Esc key:
            Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });
        }
    }
}
