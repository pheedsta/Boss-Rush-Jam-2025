using TMPro;
using UnityEngine;

public class CrosshairCanvas : MonoBehaviour {
    [SerializeField] private Player player;
    private TextMeshProUGUI _textMesh;

    private void Awake() {
        _textMesh = transform.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    }

    //private void Update() {
        //_textMesh.text = $"Speed: {player.Speed}";
    //}
}
