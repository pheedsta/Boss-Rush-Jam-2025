using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: CollectableManager
//++++++++++++++++++++++++++++++//

public class CollectableManager : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://

    [SerializeField] private Collectable[] collectables; // this is just for testing
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://

    private Player Player => GetPlayer();
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly List<Collectable> _collectList = new();
    private readonly HashSet<Collectable> _collectables = new();
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private Player _player;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        // for now add starting collectables to the HashSet
        foreach (var collectable in collectables) _collectables.Add(collectable);
    }

    private void Update() {
        // get player instance
        var player = Player;

        foreach (var collectable in _collectables) {
            // calculate the distance from this collectable to the player
            var distance = Vector3.Distance(player.transform.position, collectable.transform.position);

            if (distance <= collectable.CollectDistance) {
                // collectable is within range for collection; add it to the list for collection
                _collectList.Add(collectable);
            } else if (collectable.isMovingTowardsPlayer || distance <= collectable.VacuumDistance) {
                // collectable is within range for vacuum OR is already following player; move collectable towards player
                MoveCollectableTowardsPlayer(player, collectable);
            }
        }

        // if there are no collectables to collect, we're done
        if (0 == _collectList.Count) return;

        foreach (var collectable in _collectList) {
            // register collectable with player
            player.Collect(collectable);
            
            // play collect sound
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            collectable.collectSound.Post(collectable.gameObject);
            
            // remove collectable from HashSet
            _collectables.Remove(collectable);
            
            // return collectable to ReusablePool
            ReusablePool.ReturnReusable(collectable);
        }
        
        // clear list
        _collectList.Clear();
    }
    
    //:::::::::::::::::::::::::::::://
    // Shard Methods
    //:::::::::::::::::::::::::::::://

    private static void MoveCollectableTowardsPlayer(Player player, Collectable collectable) {
        // move shard towards player
        var direction = (player.transform.position - collectable.transform.position).normalized;
        collectable.transform.position += collectable.VacuumSpeed * Time.deltaTime * direction;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Player GetPlayer() {
        // if player has already been set we're done
        if (_player) return _player;
        
        // get player instance
        _player = Player.Instance;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_player, "Player instance is null");
        //++++++++++++++++++++++++++++++//
        
        // return player
        return _player;
    }
}
