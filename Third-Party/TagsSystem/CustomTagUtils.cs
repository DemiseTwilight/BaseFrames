using UnityEngine;

namespace TagsSystem {
    public  static class CustomTagUtils {
        public static bool ContainsTag(this GameObject obj, string tag) {
            if (obj.CompareTag(tag)) {
                return true;
            }

            
            return obj.TryGetComponent<CustomTag>(out var ct) && ct.HasTag(tag);
        }

        public static void AddTag(this GameObject obj, string tag) {
            if (!obj.TryGetComponent<CustomTag>(out var ct)) {
                ct = obj.AddComponent<CustomTag>();
            }
            ct.AddTags(tag);
        }
    }
}