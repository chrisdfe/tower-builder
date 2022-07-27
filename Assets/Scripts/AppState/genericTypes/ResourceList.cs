using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TowerBuilder.DataTypes
{
    public class ResourceList<TItem>
    {
        public class Input
        {
            public List<TItem> items;
        }

        public class Options
        {
            public bool fireEvent = true;
        }

        public List<TItem> items { get; private set; } = new List<TItem>();

        [JsonIgnore]
        public int Count { get { return items.Count; } }

        public delegate void ItemsUpdatedEvent(List<TItem> items);
        public ItemsUpdatedEvent onItemsChanged;

        public delegate void ItemChangeEvent(TItem item);
        public ItemChangeEvent onItemAdded;
        public ItemChangeEvent onItemRemoved;

        public ResourceList(Input input)
        {
            if (input != null)
            {
                this.items = input.items ?? new List<TItem>();
            }
        }

        public ResourceList(List<TItem> items)
        {
            this.items = items;
        }

        public ResourceList() : this(new Input()) { }

        public virtual void Add(TItem item)
        {
            items.Add(item);

            if (onItemAdded != null)
            {
                onItemAdded(item);
            }

            if (onItemsChanged != null)
            {
                onItemsChanged(items);
            }
        }

        public virtual void Add(List<TItem> items)
        {
            this.items = this.items.Concat(items).ToList();

            if (onItemsChanged != null)
            {
                onItemsChanged(items);
            }
        }

        public void Add(ResourceList<TItem> resourceList)
        {
            Add(resourceList.items);
        }

        public virtual void Remove(TItem item)
        {
            items.Remove(item);

            if (onItemRemoved != null)
            {
                onItemRemoved(item);
            }

            if (onItemsChanged != null)
            {
                onItemsChanged(items);
            }
        }

        // Get diff between prev/new & fire "onItemsRemoved" or "onItemsAdded"
        public virtual void Set(List<TItem> items)
        {
            this.items = items;

            if (onItemsChanged != null)
            {
                onItemsChanged(items);
            }
        }

        public virtual bool Contains(TItem item)
        {
            return items.Contains(item);
        }
    }
}