//++++++++++++++++++++++++++++++++++++++++//
// CLASS: PlayerStateScript
//++++++++++++++++++++++++++++++++++++++++//

public class PlayerStateScript : CharacterStateScript {

    //:::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::://
    
    protected Player Player => (Character as Player)!;
}