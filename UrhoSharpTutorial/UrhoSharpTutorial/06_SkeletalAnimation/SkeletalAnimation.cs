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
        public SkeletalAnimation(ApplicationOptions options) : base(options)
        {

        }
        protected override void Setup()
        {
            base.Setup();
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

        }
        private void SetupViewort()
        {
        }
    }
}
