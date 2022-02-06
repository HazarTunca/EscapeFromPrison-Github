using UnityEngine;

public class MoveCamera : MonoBehaviour {

    [SerializeField] private Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}
