using System.Collections;
using System.Collections.Generic;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Residents;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResident : MonoBehaviour
    {
        public Resident resident;


        public void Initialize()
        {
            UpdatePosition();
        }

        void Awake()
        {
            Registry.Stores.Residents.onResidentPositionUpdated += OnResidentPositionUpdated;
        }

        void OnResidentPositionUpdated(Resident resident)
        {
            if (resident != this.resident)
            {
                return;
            }

            UpdatePosition();
        }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(resident.coordinates);
        }
    }
}
