using System;
using Text_3D_Renderer;
using Text_3D_Renderer.World;

namespace Text_3D_Engine
{
    public class Bullet : Cube
    {
        Vector3 direction;
        double speed;
        bool autocull = true;
        public Bullet(Vector3 position, Vector3 direction, double speed) : base(false)
        {
            SetPosition(position);
            this.direction = direction;
            this.speed = speed;
        }

        override public void Update(double deltatime)
        {
            double milliseconds = deltatime / 1000;
            Translate(direction * (speed * milliseconds));
            Rotate(0, Math.PI * milliseconds, 0);

            double distFromCam = WorldUtil.GetWODistance(World.Camera, this);
            double distFromEnemy = WorldUtil.GetWODistance(World.GetWorldObject("player"), this);

            if(!Name.Contains(World.Name))
            {
                if (distFromCam < 1.5)
                {
                    World.Camera.Properties[0]--;
                    World.RemoveWorldObject(Name);
                    Dispose();
                }
                else if (distFromCam > World.Camera.ViewDist)
                {
                    World.RemoveWorldObject(Name);
                }
            }
        }

        public bool CullsAtViewDistance()
        {
            return autocull;
        }

        public override object Clone()
        {
            WorldObject clone = new Bullet(GetVectorPosition(), direction, speed).SetRotation(rotation).SetScale(scale);
            clone.Name = name;
            return clone;
        }
    }
}
