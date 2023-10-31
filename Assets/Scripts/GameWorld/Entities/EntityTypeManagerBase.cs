using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntityTypeManagerBase : MonoBehaviour, ISetupable
    {
        public GameObject entityPrefab;

        public AssetList meshAssets = new();

        [HideInInspector]
        public List<GameWorldEntity> entities = new();

        protected Transform entitiesInstanceWrapper;

        /*
            Lifecycle
        */
        public void Awake()
        {
            entitiesInstanceWrapper = GameObject.Find("EntitiesInstanceWrapper").transform;
        }

        public void Start()
        {
            Setup();
        }

        public void OnDestroy()
        {
            Teardown();
        }

        public virtual void Setup()
        {
            ReplaceMaterials();
        }

        public virtual void Teardown() { }

        /*
            Public Interface
        */
        public void ResetEntity(Entity entity)
        {
            GameWorldEntity gameWorldEntity = FindByEntity(entity);

            RemoveEntity(entity);
            CreateEntity(entity);
        }

        public GameWorldEntity CreateEntity(Entity entity)
        {
            GameObject entityGameObject = Instantiate(entityPrefab);
            GameWorldEntity gameWorldEntity = entityGameObject.GetComponent<GameWorldEntity>();

            gameWorldEntity.entity = entity;
            gameWorldEntity.transform.SetParent(entitiesInstanceWrapper, false);
            gameWorldEntity.transform.localPosition = Vector3.zero;
            gameWorldEntity.manager = this;

            entities.Add(gameWorldEntity);

            gameWorldEntity.Setup();

            return gameWorldEntity;
        }

        public void RemoveEntity(Entity entity)
        {
            GameWorldEntity gameWorldEntity = FindByEntity(entity);

            entities.Remove(gameWorldEntity);
            Destroy(gameWorldEntity.gameObject);
        }

        public void BuildEntity(Entity entity)
        {
            ResetEntity(entity);
        }

        /*
        public void UpdateEntityPosition(Entity entity)
        {
            FindByEntity(entity)?.UpdatePosition();
        }

        public void UpdateEntityColor(Entity entity)
        {
            FindByEntity(entity)?.UpdateColor();
        }

        public void UpdateAllEntityColors()
        {
            foreach (GameWorldEntity gameWorldEntity in entities)
            {
                gameWorldEntity.UpdateColor();
            }
        }
        */

        public void ReplaceMaterials()
        {
            foreach (MeshAssetList.ValueTypeWrapper valueTypeWrapper in meshAssets.list)
            {
                GameObject gameObject = valueTypeWrapper.value;
                MaterialsReplacer.ReplaceMaterials(gameObject.transform);
            }
        }

        public GameWorldEntity FindByEntity(Entity entity) =>
            entities.Find(gameWorldEntity => gameWorldEntity.entity == entity);
    }
}