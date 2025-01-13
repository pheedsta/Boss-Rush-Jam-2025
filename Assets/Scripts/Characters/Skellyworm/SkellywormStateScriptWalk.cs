using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: SkellywormStateScriptWalk
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Walk State", menuName = "Scriptable Objects/Enemies/Skellyworm Walk State", order = 1)]
public class SkellywormStateScriptWalk : SkellywormStateScript {
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // change to walk animation
        Skellyworm.SetAnimState(1);
    }

    public override void Update() {
        base.Update();

        if (!Skellyworm.IsAlive) {
            //Skellyworm is dead; change to die state
            Skellyworm.StateMachine.ChangeState(Skellyworm.DieState);
        } else {
            // Skellyworm is not dead; move towards player and attempt to attack player
            // NB: attack will only execute if Skellyworm is in range and not on cooldown
            Skellyworm.MoveTowardsPlayer();
            Skellyworm.AttackPlayer();
        }
    }
}