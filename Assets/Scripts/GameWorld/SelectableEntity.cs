using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

public class SelectableEntity : MonoBehaviour
{
    public EntityType entityType = EntityType.Room;
    public EntityBase entity;

    Material originalMaterial;

    void Awake()
    {
        switch (entityType)
        {
            case (EntityType.Room):
                entity = new RoomEntity();
                break;
            case (EntityType.Furniture):
                entity = new FurnitureEntity();
                break;
            case (EntityType.Resident):
                entity = new ResidentEntity();
                break;
            default:
                break;
        }

        originalMaterial = transform.GetComponent<Material>();
    }

    public void OnSelect()
    {
        Debug.Log("select");
    }

    public void OnDeselect()
    {
        Debug.Log("deselect");
    }
}
