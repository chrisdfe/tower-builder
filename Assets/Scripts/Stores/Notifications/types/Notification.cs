using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace TowerBuilder.Stores.Notifications
{
    public class Notification
    {
        public string id { get; private set; }
        public string message { get; private set; }

        public Notification(string message)
        {
            id = Guid.NewGuid().ToString();
            this.message = message;
        }
    }
}