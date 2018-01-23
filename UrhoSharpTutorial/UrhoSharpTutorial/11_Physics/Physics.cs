using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Physics;

namespace UrhoSharpTutorial
{
    public class Physics : MyGame
    {
        private Camera camera;
        private Scene scene;
        public Physics(ApplicationOptions options) : base(options)
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
        private void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

        private void CreateScene()
        {
            var cache = ResourceCache;
            scene = new Scene();

            scene.CreateComponent<Octree>();
            scene.CreateComponent<PhysicsWorld>();
            scene.CreateComponent<DebugRenderer>();

            Node zoneNode = scene.CreateChild("Zone");
            Zone zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
            zone.AmbientColor = new Color(0.15f, 0.15f, 0.15f);
            zone.FogColor = new Color(1.0f, 1.0f, 1.0f);
            zone.FogStart = 300.0f;
            zone.FogEnd = 500.0f;

            Node lightNode = scene.CreateChild("DirectionalLight");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
            Light light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.CastShadows = true;
            light.ShadowBias = new BiasParameters(0.00025f, 0.5f);
            light.ShadowCascade = new CascadeParameters(10.0f, 50.0f, 200.0f, 0.0f, 0.8f);

            Node skyNode = scene.CreateChild("Sky");
            skyNode.SetScale(500.0f); 
            Skybox skybox = skyNode.CreateComponent<Skybox>();
            skybox.Model = cache.GetModel("Models/Box.mdl");
            skybox.SetMaterial(cache.GetMaterial("Materials/Skybox.xml"));


            {

                Node floorNode = scene.CreateChild("Floor");
                floorNode.Position = new Vector3(0.0f, -0.5f, 0.0f);
                floorNode.Scale = new Vector3(1000.0f, 1.0f, 1000.0f);
                StaticModel floorObject = floorNode.CreateComponent<StaticModel>();
                floorObject.Model = cache.GetModel("Models/Box.mdl");
                floorObject.SetMaterial(cache.GetMaterial("Materials/StoneTiled.xml"));


                floorNode.CreateComponent<RigidBody>();
                CollisionShape shape = floorNode.CreateComponent<CollisionShape>();
                shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);
            }

            CameraNode = scene.CreateChild("Camera");
            camera = CameraNode.CreateComponent<Camera>();
            camera.FarClip = 500;
            CameraNode.Position = new Vector3(0, 5, 20);
        }
    }
}
