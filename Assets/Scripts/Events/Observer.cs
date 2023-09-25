namespace Events
{
    public static class Observer 
    {
        public static void Subscribe<EventType>(System.Action<object, EventType> eventData) {
            EventHelper<EventType>.Event += eventData;
        }

        public static void Unsubscribe<EventType>(System.Action<object, EventType> eventData) {
            EventHelper<EventType>.Event -= eventData;
        }

        public static void Post<EventType>(object sender, EventType eventData) {
            EventHelper<EventType>.Post(sender, eventData);
        }

        private static class EventHelper<EventType> {
            public static event System.Action<object, EventType> Event;

            public static void Post(object sender, EventType eventData) {
                Event?.Invoke(sender, eventData);
            }
        }
    }
}