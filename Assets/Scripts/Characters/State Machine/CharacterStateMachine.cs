//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterStateMachine
//++++++++++++++++++++++++++++++++++++++++//

public class CharacterStateMachine {
    
    //-----------------------------//
    // Properties
    //-----------------------------//
    
    public CharacterState CurrentCharacterState { get; private set; }
    
    //-----------------------------//
    // Changing Animation States
    //-----------------------------//

    public void ChangeState(CharacterState characterState) {
        // exit / enter CharacterState
        CurrentCharacterState?.Exit();
        CurrentCharacterState = characterState;
        CurrentCharacterState?.Enter();
    }
}