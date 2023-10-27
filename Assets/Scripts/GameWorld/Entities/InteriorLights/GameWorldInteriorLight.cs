using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;

using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.GameWorld.Lights;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorLights
{
    public class GameWorldInteriorLight : GameWorldEntity
    {
        Light entityLight;

        /* 
            Lifecycle Methods
        */
        public override void Setup()
        {
            base.Setup();

            if (entityLight != null)
            {
                // Temporary fix until things get z indecies
                entityLight.transform.position = new Vector3(0, 0, -1f);
            }
        }

        public override void Teardown()
        {
            base.Teardown();

            if (entityLight != null)
            {
                LightsManager.Find().interiorLights.Remove(entityLight);
            }
        }
    }
}
