using Urho;

namespace UrhoSharpTutorial
{
    public class StaticScene : MyGame
    {
        private Camera camera;
        private Scene scene;

        public StaticScene(ApplicationOptions options) : base(options)
        {
        }

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
            SimpleMoveCamera3D(timeStep);
        }
        private void CreateScene()
        {
            var cache = ResourceCache;
            scene = new Scene();

            scene.CreateComponent<Octree>();

            Node planeNode = scene.CreateChild("Plane");
            planeNode.Scale = new Vector3(100, 1, 100);
            StaticModel planeObject = planeNode.CreateComponent<StaticModel>();
            planeObject.Model = cache.GetModel("Models/Plane.mdl");
            planeObject.SetMaterial(cache.GetMaterial("Materials/StoneTiled.xml"));

            Node lightNode = scene.CreateChild("DirectionLight");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
            Light light=lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;

            for (int i = 0; i < 100; i++)
            {
               Node mushroom = scene.CreateChild("Mushroom");
                mushroom.Position = new Vector3(NextRandom(90) - 45, 0, NextRandom(90) - 45);
                mushroom.Rotation = new Quaternion(0, NextRandom(360), 0);
                mushroom.SetScale(0.5f + NextRandom(20000) / 10000.0f);
                var mushroomObject = mushroom.CreateComponent<StaticModel>();
                mushroomObject.Model = cache.GetModel("Models/Mushroom.mdl");
                mushroomObject.SetMaterial(cache.GetMaterial("Materials/Mushroom.xml"));
            }

            CameraNode = scene.CreateChild("camera");
            camera = CameraNode.CreateComponent<Camera>();
            CameraNode.Position = new Vector3(0, 5, 0);
        }
        private void SetupViewport()
        {
            var render = Renderer;
            render.SetViewport(0, new Viewport(Context, scene, camera, null));
        }
    }
}
