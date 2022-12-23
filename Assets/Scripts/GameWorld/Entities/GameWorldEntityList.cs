using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class GameWorldEntityList : MonoBehaviour
    {
        public GameObject entityPrefab;

        [HideInInspector]
        public List<GameWorldEntity> entities = new List<GameWorldEntity>();

        // [HideInInspector]
        // public Type entityType;

        Transform entitiesInstanceWrapper;

        void Awake()
        {
            entitiesInstanceWrapper = GameObject.Find("EntitiesInstanceWrapper").transform;
        }

        void Start()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
        }

        public void Teardown()
        {
        }

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

            entities.Add(gameWorldEntity);

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

        public void UpdateEntityColors()
        {
            foreach (GameWorldEntity gameWorldEntity in entities)
            {
                gameWorldEntity.UpdateEntityColor();
            }
        }

        public GameWorldEntity FindByEntity(Entity entity) => entities.Find(gameWorldEntity => gameWorldEntity.entity == entity);
    }
}