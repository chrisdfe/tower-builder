using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

public class SelectableEntity : MonoBehaviour
{
    public EntityType entityType = EntityType.Room;
    public EntityBase entity;

    Material originalMaterial;

    void Awake()
    {
        originalMaterial = transform.GetComponent<Material>();
    }

    public void SetRoomEntity(Room room)
    {
        this.entity = new RoomEntity(room);
    }

    public void SetFurnitureEntity(Furniture furniture)
    {
        this.entity = new FurnitureEntity(furniture);
    }

    public void SetResidentEntity(Resident resident)
    {
        this.entity = new ResidentEntity(resident);
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
