using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: SkellywormStateScriptSpawn
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Spawn State", menuName = "Scriptable Objects/Enemies/Skellyworm Spawn State", order = 0)]
public class SkellywormStateScriptSpawn : SkellywormStateScript {
    
    //:::::::::::::::::::::::::::::://
    // Constants
    //:::::::::::::::::::::::::::::://

    private const float k_AnimationDuration = 1f; // this should be the same as spawn animation duration
    
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
        
        // change to spawn animation
        Skellyworm.SetAnimState(0);
    }

    public override void Update() {
        base.Update();
        
        // increment progress
        _progress += Time.deltaTime;

        if (!Skellyworm.IsAlive) {
            // Skellyworm is dead; change to die state
            Skellyworm.StateMachine.ChangeState(Skellyworm.DieState);
        } else if (k_AnimationDuration <= _progress) {
            // Skellyworm is alive AND we've reached maximum animation duration; change to walk state
            Skellyworm.StateMachine.ChangeState(Skellyworm.WalkState);
        }
    }
}