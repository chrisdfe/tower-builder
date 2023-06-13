using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.GameWorld.Lights;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorLights
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldInteriorLight : MonoBehaviour
    {

        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        InteriorLight interiorLight;
        new Light light;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
            light = TransformUtils.FindDeepChild(transform, "Light").GetComponent<Light>();

            // Temporary fix until things get z indecies
            light.transform.position = new Vector3(0, 0, -1f);
        }

        void Start()
        {
            interiorLight = GetComponent<GameWorldEntity>().entity as InteriorLight;

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            LightsManager.Find().interiorLights.Add(light);

            AssetList assetList = GameWorldInteriorLightsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.ValueFromKey(interiorLight.definition.key);

            // entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = interiorLight.absoluteCellCoordinatesList;
            entityMeshWrapper.Setup();

            GameWorldEntity gameWorldEntity = GetComponent<GameWorldEntity>();
            gameWorldEntity.customColorUpdater = UpdateColor;
            gameWorldEntity.Setup();
        }

        public void Teardown()
        {
            LightsManager.Find().interiorLights.Remove(light);
        }

        void UpdateColor(GameWorldEntity gameWorldEntity) { }

        /* 
            Static API
         */
        public static GameWorldInteriorLight Create(Transform parent)
        {
            GameWorldInteriorLightsManager InteriorlightsManager = GameWorldInteriorLightsManager.Find();
            GameObject prefab = InteriorlightsManager.assetList.ValueFromKey("Light");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldInteriorLight gameWorldInteriorLight = gameObject.GetComponent<GameWorldInteriorLight>();
            return gameWorldInteriorLight;
        }
    }
}
