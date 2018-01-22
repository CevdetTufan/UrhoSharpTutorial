using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace UrhoSharpTutorial
{
    public class AnimatingScene : MyGame
    {
        private Scene scene;
        private Camera camera;

        public AnimatingScene(ApplicationOptions options) : base(options)
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
            SimpleMoveCamera3D(timeStep,100);
        }
        private void CreateScene()
        {
            scene = new Scene();
            var cache = ResourceCache;

            scene.CreateComponent<Octree>();

            var zoneNode = scene.CreateChild("Zone");
            var zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-1000f, 1000f));
            zone.AmbientColor = new Color(0.05f, 0.1f, 0.15f);
            zone.FogColor = new Color(0.1f, 0.2f, 0.3f);
            zone.FogStart = 10;
            zone.FogEnd = 100;

            Node boxesNode = scene.CreateChild("Boxes");
            for (int i = 0; i < 2000; i++)
            {
                Node boxNode = new Node();
                boxesNode.AddChild(boxNode, 0);
                boxNode.Position = new Vector3(NextRandom(200f) - 100f, NextRandom(200f) - 100f, NextRandom(200f) - 100f);
                boxNode.Rotation = new Quaternion(NextRandom(360.0f), NextRandom(360.0f), NextRandom(360.0f));

                using (var boxObject = boxNode.CreateComponent<StaticModel>())
                {
                    boxObject.Model = cache.GetModel("Models/Box.mdl");
                    boxObject.SetMaterial(cache.GetMaterial("Materials/Stone.xml"));
                }

                var rotationSpeed = new Vector3(100.0f, 20.0f, 30.0f);

                var rotator = new Rotator() { RotationSpeed = rotationSpeed };
                boxNode.AddComponent(rotator);
            }

            //camera
            CameraNode = scene.CreateChild("Camera");
            camera = CameraNode.CreateComponent<Camera>();
            camera.FarClip = 100f;

            var light = CameraNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 30.0f;
        }

        private void SetupViewport()
        {
            var render = Renderer;
            render.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

        class Rotator : Component
        {
            public Rotator()
            {
                ReceiveSceneUpdates = true;
            }

            public Vector3 RotationSpeed { get; set; }

            protected override void OnUpdate(float timeStep)
            {
                Node.Rotate(new Quaternion(
                    RotationSpeed.X * timeStep,
                    RotationSpeed.Y * timeStep,
                    RotationSpeed.Z * timeStep), TransformSpace.Local);
            }
        }
    }
}
