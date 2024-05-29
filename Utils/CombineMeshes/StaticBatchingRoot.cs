using UnityEngine;

public class StaticBatchingRoot : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        StaticBatchingUtility.Combine(this.gameObject);
    }

    // Update is called once per frame
    void Update() {

    }
}
