using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace UrhoSharpTutorial
{
    public class Billboards : MyGame
    {
        private Scene scene;
        private Camera camera;

        public Billboards(ApplicationOptions options) : base(options)
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

            var zoneNode = scene.CreateChild("Zone");
            Zone zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
            zone.AmbientColor = new Color(0.1f, 0.1f, 0.1f);
            zone.FogStart = 100.0f;
            zone.FogEnd = 300.0f;

            Node lightNode = scene.CreateChild("Light");
            Light light=lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.Color = new Color(0.2f, 0.2f, 0.2f);
            light.SpecularIntensity = 1.0f;

            for (int y = -5; y <= 5; ++y)
            {
                for (int x = -5; x <= 5; ++x)
                {
                    var floorNode = scene.CreateChild("FloorTile");
                    floorNode.Position = new Vector3(x * 20.5f, -0.5f, y * 20.5f);
                    floorNode.Scale = new Vector3(20.0f, 1.0f, 20.0f);
                    var floorObject = floorNode.CreateComponent<StaticModel>();
                    floorObject.Model = cache.GetModel("Models/Box.mdl");
                    floorObject.SetMaterial(cache.GetMaterial("Materials/Stone.xml"));
                }
            }

            const uint numMushroomgroups = 25;
            const uint numMushrooms = 5;

            for (uint i = 0; i < numMushroomgroups; ++i)
            {
                var groupNode = scene.CreateChild("MushroomGroup");
                groupNode.Position = new Vector3(NextRandom(190.0f) - 95.0f, 0.0f, NextRandom(190.0f) - 95.0f);

                for (uint j = 0; j < numMushrooms; ++j)
                {
                    var mushroomNode = groupNode.CreateChild("Mushroom");
                    mushroomNode.Position = new Vector3(NextRandom(25.0f) - 12.5f, 0.0f, NextRandom(25.0f) - 12.5f);
                    mushroomNode.Rotation = new Quaternion(0.0f, NextRandom() * 360.0f, 0.0f);
                    mushroomNode.SetScale(1.0f + NextRandom() * 4.0f);
                    var mushroomObject = mushroomNode.CreateComponent<StaticModel>();
                    mushroomObject.Model = cache.GetModel("Models/Mushroom.mdl");
                    mushroomObject.SetMaterial(cache.GetMaterial("Materials/Mushroom.xml"));
                    mushroomObject.CastShadows = true;
                }
            }

            const uint numBillboardnodes = 25;
            const uint numBillboards = 10;

            for (uint i = 0; i < numBillboardnodes; ++i)
            {
                var smokeNode = scene.CreateChild("Smoke");
                smokeNode.Position = new Vector3(NextRandom(200.0f) - 100.0f, NextRandom(20.0f) + 10.0f, NextRandom(200.0f) - 100.0f);

                var billboardObject = smokeNode.CreateComponent<BillboardSet>();
                billboardObject.NumBillboards = numBillboards;
                billboardObject.Material = cache.GetMaterial("Materials/LitSmoke.xml");
                billboardObject.Sorted = true;

                for (uint j = 0; j < numBillboards; ++j)
                {
                    var bb = billboardObject.GetBillboardSafe(j);
                    bb.Position = new Vector3(NextRandom(12.0f) - 6.0f, NextRandom(8.0f) - 4.0f, NextRandom(12.0f) - 6.0f);
                    bb.Size = new Vector2(NextRandom(2.0f) + 3.0f, NextRandom(2.0f) + 3.0f);
                    bb.Rotation = NextRandom() * 360.0f;
                    bb.Enabled = true;
                }

                billboardObject.Commit();
            }

            const uint numLights = 9;

            for (uint i = 0; i < numLights; ++i)
            {
                lightNode = scene.CreateChild("SpotLight");
                light = lightNode.CreateComponent<Light>();

                float angle = 0.0f;

                Vector3 position = new Vector3((i % 3) * 60.0f - 60.0f, 45.0f, (i / 3) * 60.0f - 60.0f);
                Color color = new Color(((i + 1) & 1) * 0.5f + 0.5f, (((i + 1) >> 1) & 1) * 0.5f + 0.5f, (((i + 1) >> 2) & 1) * 0.5f + 0.5f);

                lightNode.Position = position;
                lightNode.SetDirection(new Vector3((float)Math.Sin(angle), -1.5f, (float)Math.Cos(angle)));

                light.LightType = LightType.Spot;
                light.Range = 90.0f;
                light.RampTexture = cache.GetTexture2D("Textures/RampExtreme.png");
                light.Fov = 45.0f;
                light.Color = color;
                light.SpecularIntensity = 1.0f;
                light.CastShadows = true;
                light.ShadowBias = new BiasParameters(0.00002f, 0.0f);

                light.ShadowFadeDistance = 100.0f; 
                light.ShadowDistance = 125.0f; 
                light.ShadowResolution = 0.5f;
                light.ShadowNearFarRatio = 0.01f;
            }


            CameraNode = scene.CreateChild("Camera");
            camera = CameraNode.CreateComponent<Camera>();
            CameraNode.Position = new Vector3(0, 5, 0);
            camera.FarClip = 300f;
        }
        private void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }
    }
}
