using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace UrhoSharpTutorial
{
    public class SkeletalAnimation : MyGame
    {
        private Scene scene;
        private Camera camera;
        public SkeletalAnimation(ApplicationOptions options) : base(options)
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
            var cache = ResourceCache;
            scene = new Scene();

            //scene
            scene.CreateComponent<Octree>();
            scene.CreateComponent<DebugRenderer>();

            //plane
            Node planeNode = scene.CreateChild("Plane");
            planeNode.Scale = new Vector3(1000, 1, 1000);
            StaticModel planeObject = planeNode.CreateComponent<StaticModel>();
            planeObject.Model = cache.GetModel("Models/Plane.mdl");
            planeObject.SetMaterial(cache.GetMaterial("Materials/StoneTiled.xml"));

            //zone 
            Node zoneNode = scene.CreateChild("Zone");
            Zone zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
            zone.AmbientColor = new Color(0.15f, 0.15f, 0.15f);
            zone.FogColor = new Color(0.5f, 0.5f, 0.7f);
            zone.FogStart = 100;
            zone.FogEnd = 300;

            //light
            var lightNode = scene.CreateChild("DirectionalLight");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.CastShadows = true;
            light.ShadowBias = new BiasParameters(0.00025f, 0.5f);

            light.ShadowCascade = new CascadeParameters(10.0f, 50.0f, 200.0f, 0.0f, 0.8f);

            var bounds = new BoundingBox(new Vector3(-47.0f, 0.0f, -47.0f), new Vector3(47.0f, 0.0f, 47.0f));
            for (int i = 0; i < 1000; i++)
            {
                var modelNode = scene.CreateChild("Jack");
                modelNode.Position = new Vector3(NextRandom(-45, 45), 0.0f, NextRandom(-45, 45));
                modelNode.Rotation = new Quaternion(0, NextRandom(0, 360), 0);

                var modelObject = new AnimatedModel();
                modelNode.AddComponent(modelObject);
                modelObject.Model = cache.GetModel("Models/Jack.mdl");
                //modelObject.Material = cache.GetMaterial("Materials/Jack.xml");
                modelObject.CastShadows = true;

                var walkAnimation = cache.GetAnimation("Models/Jack_Walk.ani");
                var state = modelObject.AddAnimationState(walkAnimation);
                if (state != null)
                {
                    state.Weight = 1;
                    state.Looped = true;
                }
                var mover = new Mover(2.0f, 100f, bounds);
                modelNode.AddComponent(mover);
            }

            CameraNode = scene.CreateChild("Camera");
            camera = CameraNode.CreateComponent<Camera>();
            camera.FarClip = 300f;
            CameraNode.Position = new Vector3(0.0f, 5.0f, 0.0f);
        }
        private void SetupViewport()
        {
            var render = Renderer;
            render.SetViewport(0, new Viewport(Context, scene, camera, null));
        }


        class Mover : Component
        {
            float MoveSpeed { get; }
            float RotationSpeed { get; }
            BoundingBox Bounds { get; }

            public Mover(float moveSpeed, float rotateSpeed, BoundingBox bounds)
            {
                MoveSpeed = moveSpeed;
                RotationSpeed = rotateSpeed;
                Bounds = bounds;
                ReceiveSceneUpdates = true;
            }

            protected override void OnUpdate(float timeStep)
            {
                Node.Translate(Vector3.UnitZ * MoveSpeed * timeStep, TransformSpace.Local);

                var pos = Node.Position;
                if (pos.X < Bounds.Min.X || pos.X > Bounds.Max.X || pos.Z < Bounds.Min.Z || pos.Z > Bounds.Max.Z)
                    Node.Yaw(RotationSpeed * timeStep, TransformSpace.Local);

                var model = GetComponent<AnimatedModel>();
                if (model.NumAnimationStates > 0)
                {
                    var state = model.AnimationStates.First();
                    state.AddTime(timeStep);
                }
            }
        }
    }
}
