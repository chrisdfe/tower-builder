using System.Collections.Generic;
using System.Linq;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.Templates
{
    public class RoomTemplates
    {
        public List<RoomTemplate> roomTemplates { get; private set; } = new List<RoomTemplate>();

        public RoomTemplates()
        {
            RegisterTemplates(DefaultRoomTemplates.roomTemplates);
        }

        // Registration
        public void RegisterTemplate(RoomTemplate roomTemplate)
        {
            this.roomTemplates.Add(roomTemplate);
        }

        public void RegisterTemplates(List<RoomTemplate> roomTemplates)
        {
            this.roomTemplates = this.roomTemplates.Concat(roomTemplates).ToList();
        }

        // Queries
        public RoomTemplate FindByTitle(string title)
        {
            return roomTemplates.Find(template => template.title == title);
        }

        public RoomTemplate FindByKey(string key)
        {
            return roomTemplates.Find(template => template.key == key);
        }

        public List<RoomTemplate> FindByCategory(string category)
        {
            return roomTemplates.FindAll(template => template.category == category).ToList();
        }

        public List<string> FindAllRoomCategories()
        {
            List<string> result = new List<string>();

            foreach (RoomTemplate roomTemplate in roomTemplates)
            {
                if (!result.Contains(roomTemplate.category))
                {
                    result.Add(roomTemplate.category);
                }
            }

            return result;
        }
    }
}