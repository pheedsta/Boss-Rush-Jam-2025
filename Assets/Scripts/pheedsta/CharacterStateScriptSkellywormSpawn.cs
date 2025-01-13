using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterStateScriptSkellywormSpawn
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Spawn State", menuName = "Scriptable Objects/Enemies/Skellyworm Spawn State", order = 0)]
public class CharacterStateScriptSkellywormSpawn : CharacterStateScript {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_MaxSpawnDuration = 1f; // spawn duration in seconds
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _spawnDuration;
    private SkellywormEnemy _skellywormEnemy;
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // reset field to defaults
        _spawnDuration = 0f;
        _skellywormEnemy = Character as SkellywormEnemy; // this is a bit of a hack...
        
        // change to spawn animation
        _skellywormEnemy!.ChangeAnimationState(0);
    }

    public override void Update() {
        base.Update();
        
        // increment spawn duration
        _spawnDuration += Time.deltaTime;
        
        // if we have reached maximum spawn duration; change to walk state
        if (_spawnDuration >= k_MaxSpawnDuration) Character.StateMachine.ChangeState(_skellywormEnemy!.WalkState);
    }
}