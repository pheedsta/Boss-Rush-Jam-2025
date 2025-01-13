//++++++++++++++++++++++++++++++++++++++++//
// CLASS: CharacterState
//++++++++++++++++++++++++++++++++++++++++//

public class CharacterState {
    
    //::::::::::::::::::::::::::::://
    // Readonly Fields
    //::::::::::::::::::::::::::::://
    
    private readonly CharacterStateScript _characterStateScript;
    
    //-----------------------------//
    // Constructors
    //-----------------------------//
    
    public CharacterState(Character character, CharacterStateScript characterStateScript) {
        _characterStateScript = characterStateScript;
        _characterStateScript.SetCharacter(character);
    }
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public void Enter() {
        _characterStateScript.Enter();
    }

    public void Update() {
        _characterStateScript.Update();
    }

    public void Exit() {
        _characterStateScript.Exit();
    }
}