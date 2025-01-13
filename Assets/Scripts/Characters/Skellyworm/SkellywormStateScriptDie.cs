using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// CLASS: SkellywormStateScriptDie
//++++++++++++++++++++++++++++++++++++++++//

[CreateAssetMenu(fileName = "Skellyworm Die State", menuName = "Scriptable Objects/Enemies/Skellyworm Die State", order = 4)]
public class SkellywormStateScriptDie : SkellywormStateScript {
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public override void Enter() {
        base.Enter();
        
        // change to die animation
        Skellyworm.SetAnimState(4);
    }
}