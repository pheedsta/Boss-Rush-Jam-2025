using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterStateScriptSkellywormProjectileAttack
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Projectile Attack State", menuName = "Scriptable Objects/Enemies/Skellyworm Projectile Attack State", order = 3)]
public class CharacterStateScriptSkellywormProjectileAttack : CharacterStateScript {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_MaxAttackDuration = 1f; // attack duration in seconds
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _attackDuration;
    private SkellywormEnemy _skellywormEnemy;
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // reset field to defaults
        _attackDuration = 0f;
        _skellywormEnemy = Character as SkellywormEnemy; // this is a bit of a hack...
        
        // change to walk animation
        _skellywormEnemy!.ChangeAnimationState(3);
    }

    public override void Update() {
        base.Update();
        
        // increment attack duration
        _attackDuration += Time.deltaTime;
        
        // if we have reached maximum attack duration; change to walk state
        if (_attackDuration >= k_MaxAttackDuration) Character.StateMachine.ChangeState(_skellywormEnemy!.WalkState);
    }
}