using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: SkellywormStateScriptMeleeAttack
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Melee Attack State", menuName = "Scriptable Objects/Enemies/Skellyworm Melee Attack State", order = 2)]
public class SkellywormStateScriptMeleeAttack : SkellywormStateScript {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_AnimationDuration = 1f; // this should be the same as melee attack animation duration
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _progress;
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // reset field to defaults
        _progress = 0f;
        
        // change to walk animation
        Skellyworm.SetAnimState(2);
    }

    public override void Update() {
        base.Update();
        
        // increment progress
        _progress += Time.deltaTime;

        if (!Skellyworm.Health.IsAlive) {
            // Skellyworm is dead; change to die state
            Skellyworm.StateMachine.ChangeState(Skellyworm.DieState);
        } else if (k_AnimationDuration <= _progress) {
            // Skellyworm is alive AND we've reached maximum animation duration; change to walk state
            Skellyworm.StateMachine.ChangeState(Skellyworm.WalkState);
        }
    }
}