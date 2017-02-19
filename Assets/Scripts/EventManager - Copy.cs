//using UnityEngine;
//using UnityEngine.Events;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//[Serializable]
//public class GameObjectEvent : UnityEvent<GameObject> { }

//public class EventManager : MonoBehaviour
//{

//    private Dictionary<string, GameObjectEvent> objectEventDictionary;
//    private Dictionary<string, UnityEvent> eventDictionary;
//    private List<GameObjectEvent> eventQueue;
//    private Queue eventQueue1;

//    private static EventManager eventManager;

//    public static EventManager instance
//    {
//        get
//        {
//            if (!eventManager)
//            {
//                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

//                if (!eventManager)
//                {
//                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
//                }
//                else
//                {
//                    eventManager.Init();
//                }
//            }

//            return eventManager;
//        }
//    }

//    void Init()
//    {
//        if (eventDictionary == null)
//        {
//            objectEventDictionary = new Dictionary<string, GameObjectEvent>();
//            eventDictionary  = new Dictionary<string, UnityEvent>();
//        }
//    }

//    public static void StartListening(string eventName, UnityAction listener)
//    {
//        UnityEvent thisEvent = null;
//        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.AddListener(listener);
//        }
//        else
//        {
//            thisEvent = new UnityEvent();
//            thisEvent.AddListener(listener);
//            instance.eventDictionary.Add(eventName, thisEvent);
//        }
//    }

//    public static void StartListening(string eventName, UnityAction<GameObject> listener)
//    {
//        GameObjectEvent thisEvent = null;
//        if (instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.AddListener(listener);
//        }
//        else
//        {
//            thisEvent = new GameObjectEvent();
//            thisEvent.AddListener(listener);
//            instance.objectEventDictionary.Add(eventName, thisEvent);
//        }
//    }

//    public static void StopListening(string eventName, UnityAction listener)
//    {
//        if (eventManager == null) return;
//        UnityEvent thisEvent = null;
//        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.RemoveListener(listener);
//        }
//    }

//    //Queue Event()
//    public static void QueueEvent(string eventName)
//    {

//        GameObjectEvent thisObjectEvent2 =  Test<GameObject>;
//        UnityEvent thisEvent = null;
//        GameObjectEvent thisObjectEvent = null;
//        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//        {
//            instance.eventQueue1.Enqueue(thisEvent);
//        }
//        else if (instance.objectEventDictionary.TryGetValue(eventName, out thisObjectEvent)) { 
        
//                instance.eventQueue1.Enqueue(thisEvent);
            
//        }
//    }

//    public static void Test<T>(GameObject eventName)
//    { }

//    public static void TriggerEvent(string eventName)
//    {
//        UnityEvent thisEvent = null;
//        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//        {
//            thisEvent.Invoke();
            
//        }
//    }
//}