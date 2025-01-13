using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterStateScriptSkellywormMeleeAttack
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Melee Attack State", menuName = "Scriptable Objects/Enemies/Skellyworm Melee Attack State", order = 2)]
public class CharacterStateScriptSkellywormMeleeAttack : CharacterStateScript {
    
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
        _skellywormEnemy!.ChangeAnimationState(2);
    }

    public override void Update() {
        base.Update();
        
        // increment attack duration
        _attackDuration += Time.deltaTime;
        
        // if we have reached maximum attack duration; change to walk state
        if (_attackDuration >= k_MaxAttackDuration) Character.StateMachine.ChangeState(_skellywormEnemy!.WalkState);
    }
}