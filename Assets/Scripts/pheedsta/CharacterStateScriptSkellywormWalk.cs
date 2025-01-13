using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterStateScriptSkellywormWalk
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Walk State", menuName = "Scriptable Objects/Enemies/Skellyworm Walk State", order = 1)]
public class CharacterStateScriptSkellywormWalk : CharacterStateScript {
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private SkellywormEnemy _skellywormEnemy;
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // reset field to defaults
        _skellywormEnemy = Character as SkellywormEnemy; // this is a bit of a hack...
        
        // change to walk animation
        _skellywormEnemy!.ChangeAnimationState(1);
    }

    public override void Update() {
        base.Update();

        if (_skellywormEnemy!.CanMeleeAttack) {
            // can melee attack
            _skellywormEnemy!.MeleeAttackPlayer();
        } else if (_skellywormEnemy!.CanProjectileAttack) {
            // can projectile attack
            _skellywormEnemy!.ProjectileAttackPlayer();
        } else {
            // just move towards player for now
            _skellywormEnemy!.MoveTowardsPlayer();
        }
    }
}