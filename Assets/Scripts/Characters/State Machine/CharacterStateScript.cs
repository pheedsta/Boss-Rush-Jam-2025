using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++//
// ABSTRACT CLASS: CharacterStateScript
//++++++++++++++++++++++++++++++++++++++++//

public abstract class CharacterStateScript : ScriptableObject {
    
    //:::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::://

    protected Character Character { get; private set; }

    //-----------------------------//
    // Initialisation
    //-----------------------------//
    
    public void SetCharacter(Character character) {
        Character = character;
    }
    
    //-----------------------------//
    // State Methods
    //-----------------------------//

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}