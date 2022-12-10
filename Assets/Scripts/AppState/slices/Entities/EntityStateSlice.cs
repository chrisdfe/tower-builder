using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public class EntityStateSlice<ListWrapperType, EntityType, EventsType> : ListStateSlice<ListWrapperType, EntityType, EventsType>
        where ListWrapperType : ListWrapper<EntityType>, new()
        where EntityType : Entity
        where EventsType : EntityStateSlice<ListWrapperType, EntityType, EventsType>.Events, new()
    {
        public new class Events : ListStateSlice<ListWrapperType, EntityType, EventsType>.Events
        {
            public Events.ListEvent onItemsBuilt;
        }

        public EntityStateSlice(AppState appState) : base(appState) { }

        public void Build(EntityType entity)
        {
            // TODO - don't do this here
            entity.validator.Validate(appState);

            if (!entity.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (EntityValidationError validationError in entity.validator.errors.items)
                {
                    appState.Notifications.Add(new Notification(validationError.message));
                }
                return;
            }

            // 
            appState.Wallet.SubtractBalance(entity.price);

            /*
            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = queries.FindRoomsToCombineWith(room);

            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    room.blocks.Add(otherRoom.blocks);
                    Remove(otherRoom);
                }

                room.Reset();
            }
            */

            OnPreBuild(entity);
            entity.OnBuild();

            ListWrapperType listWrapper = new ListWrapperType();
            listWrapper.Add(entity);
            events.onItemsBuilt?.Invoke(listWrapper);
        }

        protected virtual void OnPreBuild(EntityType entity) { }

        public override void Add(EntityType entity)
        {
            entity.validator.Validate(appState);
            base.Add(entity);
        }

        // TODO- rename this destroy
        public override void Remove(EntityType entity)
        {
            OnPreDestroy(entity);

            // TODO - validation
            // TODO - add money back into wallet

            entity.OnDestroy();
            base.Remove(entity);
        }

        protected virtual void OnPreDestroy(EntityType entity) { }
    }
}