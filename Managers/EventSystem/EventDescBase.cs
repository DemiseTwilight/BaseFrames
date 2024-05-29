
     using System;
     using System.Collections.Generic;

     public class EventDescBase : IEqualityComparer<EventDescBase>{
         public readonly string key;
         public readonly string fullKey;

         public EventDescBase(string key) {
             this.key = key;
             fullKey = GetType().FullName + key;
         }
         
         public bool Equals(EventDescBase x, EventDescBase y) {
             if (x == null || y == null) {
                 return false;
             }

             return x.fullKey == y.fullKey;
         }
         
         public int GetHashCode(EventDescBase obj) {
             return obj.fullKey.GetHashCode();
         }
         
         public void UnRegister(Delegate handler) {
             EventManager.Instance.RemoveListener(this,handler);
         }
         
     }
     public class EventDesc: EventDescBase{
         public EventDesc(string key) : base(key) { }
         public void Broadcast() => EventManager.Instance.DispatchEvent(this);
         public void Register(EventManager.OnEventHandler handler) => EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler handler) => EventManager.Instance.RemoveListener(this,handler);
         
     }
     public class EventDesc<T1>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg) => EventManager.Instance.DispatchEvent(this, arg);
         public void Register(EventManager.OnEventHandler<T1> handler) => 
             EventManager.Instance.AddListener(this, handler);

         public void UnRegister(EventManager.OnEventHandler<T1> handler) => base.UnRegister(handler);
     }
     public class EventDesc<T1,T2>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg1,T2 arg2) => EventManager.Instance.DispatchEvent(this, arg1,arg2);
         public void Register(EventManager.OnEventHandler<T1,T2> handler) => 
             EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler<T1,T2> handler) => base.UnRegister(handler);
     }
     public class EventDesc<T1,T2,T3>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg1,T2 arg2,T3 arg3) => EventManager.Instance.DispatchEvent(this, arg1,arg2,arg3);
         public void Register(EventManager.OnEventHandler<T1,T2,T3> handler) => 
             EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler<T1,T2,T3> handler) => base.UnRegister(handler);
     }
     public class EventDesc<T1,T2,T3,T4>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg1,T2 arg2,T3 arg3,T4 arg4) => EventManager.Instance.DispatchEvent(this, arg1,arg2,arg3,arg4);
         public void Register(EventManager.OnEventHandler<T1,T2,T3,T4> handler) => 
             EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler<T1,T2,T3,T4> handler) => base.UnRegister(handler);
     }
     public class EventDesc<T1,T2,T3,T4,T5>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5) => EventManager.Instance.DispatchEvent(this, arg1,arg2,arg3,arg4,arg5);
         public void Register(EventManager.OnEventHandler<T1,T2,T3,T4,T5> handler) => 
             EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler<T1,T2,T3,T4,T5> handler) => base.UnRegister(handler);
     }
     public class EventDesc<T1,T2,T3,T4,T5,T6>:EventDescBase {
         public EventDesc(string key) : base(key) { }
         public void Broadcast(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6) => EventManager.Instance.DispatchEvent(this, arg1,arg2,arg3,arg4,arg5,arg6);
         public void Register(EventManager.OnEventHandler<T1,T2,T3,T4,T5,T6> handler) => 
             EventManager.Instance.AddListener(this, handler);
         public void UnRegister(EventManager.OnEventHandler<T1,T2,T3,T4,T5,T6> handler) => base.UnRegister(handler);
     }
     