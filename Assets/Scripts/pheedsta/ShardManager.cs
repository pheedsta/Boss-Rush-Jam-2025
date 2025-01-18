using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: ShardManager
//++++++++++++++++++++++++++++++//

public class ShardManager : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://

    [SerializeField] private Shard[] shards; // this is just for testing
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://

    private Player Player => GetPlayer();
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly List<Shard> _collectShards = new();
    private readonly HashSet<Shard> _shards = new();
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private Player _player;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void Awake() {
        // for now add starting shards to the HashSet
        foreach (var shard in shards) _shards.Add(shard);
    }

    private void Update() {
        // get player instance
        var player = Player;

        foreach (var shard in _shards) {
            // calculate the distance from this shard to the player
            var distance = Vector3.Distance(player.transform.position, shard.transform.position);

            if (distance <= shard.CollectDistance) {
                // shard is within range for collection; add it to the list for collection
                _collectShards.Add(shard);
            } else if (shard.isMovingTowardsPlayer || distance <= shard.VacuumDistance) {
                // shard is within range for vacuum OR is already following player; move shard towards player
                MoveShardTowardsPlayer(player, shard);
            }
        }

        // if there are no shards to collect, we're done
        if (0 == _collectShards.Count) return;
        
        // increment player shard count
        player.CollectShards(_collectShards.Count);

        // collect shards
        foreach (var shard in _collectShards) {
            _shards.Remove(shard);
            ReusablePool.ReturnReusable(shard);
        }
        
        // clear list
        _collectShards.Clear();
    }
    
    //:::::::::::::::::::::::::::::://
    // Shard Methods
    //:::::::::::::::::::::::::::::://

    private static void MoveShardTowardsPlayer(Player player, Shard shard) {
        // move shard towards player
        var direction = (player.transform.position - shard.transform.position).normalized;
        shard.transform.position += shard.VacuumSpeed * Time.deltaTime * direction;
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
