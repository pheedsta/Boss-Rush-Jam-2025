using System;
using System.Collections.Generic;
using _App.Scripts.juandeyby;
using UnityEngine;
using Random = UnityEngine.Random;

//++++++++++++++++++++++++++++++//
// CLASS: CollectableManager
//++++++++++++++++++++++++++++++//

public class CollectableManager : MonoBehaviour
{
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://

    [SerializeField] private CollectableHeart collectableHeartPrefab;
    [SerializeField] private CollectableShard collectableShardPrefab;
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://

    private _App.Scripts.juandeyby.Player Player => GetPlayer();
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly List<Collectable> _collectList = new();
    private readonly HashSet<Collectable> _collectables = new();
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private _App.Scripts.juandeyby.Player _player;
    private readonly Queue<CollectableHeart> _collectableHearts = new();
    private readonly Queue<CollectableShard> _collectableShards = new();
    private readonly int _maxCollectableHearts = 2;
    private readonly int _maxCollectableShards = 4;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    private void OnEnable()
    {
        ServiceLocator.Register<CollectableManager>(this);
    }

    private void Awake()
    {
        // create collectable hearts
        for (var i = 0; i < _maxCollectableHearts; i++) {
            var collectableHeart = Instantiate(collectableHeartPrefab, transform);
            collectableHeart.transform.position = GetPosition();
            _collectableHearts.Enqueue(collectableHeart);
            _collectables.Add(collectableHeart);
        }
        
        // create collectable shards
        for (var i = 0; i < _maxCollectableShards; i++) {
            var collectableShard = Instantiate(collectableShardPrefab, transform);
            collectableShard.transform.position = GetPosition();
            _collectableShards.Enqueue(collectableShard);
            _collectables.Add(collectableShard);
        }
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
                MoveCollectableTowardsPlayer(player.transform, collectable);
            }
        }

        // if there are no collectables to collect, we're done
        if (0 == _collectList.Count) return;

        foreach (var collectable in _collectList) {
            // register collectable with player
            player.PlayerCollection.Collect(collectable);
            
            // play collect sound
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            collectable.collectSound.Post(collectable.gameObject);
            
            // reassign collectable position
            collectable.transform.position = GetPosition();
            
            // remove collectable from HashSet
            // _collectables.Remove(collectable);
            
            // return collectable to ReusablePool
            // ReusablePool.ReturnReusable(collectable);
        }
        
        // clear list
        _collectList.Clear();
    }

    private void OnDisable()
    {
        ServiceLocator.Unregister<CollectableManager>();
    }

    //:::::::::::::::::::::::::::::://
    // Shard Methods
    //:::::::::::::::::::::::::::::://

    
    private Vector3 GetPosition()
    {
        var innerRadius = 0f;
        var outerRadius = 0f;

        switch (ServiceLocator.Get<GameManager>().GetGamePhase())
        {
            case GamePhase.Phase3:
                innerRadius = 12f;
                outerRadius = 15f; 
                break;
            case GamePhase.Phase2:
                innerRadius = 18f;
                outerRadius = 23f;
                break;
            case GamePhase.Phase1:
                innerRadius = 26f;
                outerRadius = 31f;
                break;
        }

        var angle = Random.Range(0f, 360f);
        var radians = angle * Mathf.Deg2Rad;

        var radius = Random.Range(innerRadius, outerRadius);

        var x = Mathf.Cos(radians) * radius;
        var z = Mathf.Sin(radians) * radius;

        var position = new Vector3(x, 0.5f, z);

        return position;
    }
    
    private static void MoveCollectableTowardsPlayer(Transform player, Collectable collectable) {
        // move shard towards player
        var direction = (player.position - collectable.transform.position).normalized;
        collectable.transform.position += collectable.VacuumSpeed * Time.deltaTime * direction;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private _App.Scripts.juandeyby.Player GetPlayer() {
        // if player has already been set we're done
        if (_player) return _player;
        
        // get player instance
        _player = _App.Scripts.juandeyby.Player.Instance;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_player, "Player instance is null");
        //++++++++++++++++++++++++++++++//
        
        // return player
        return _player;
    }
}
