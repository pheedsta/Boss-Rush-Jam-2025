using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(CharacterHealth))]

//++++++++++++++++++++++++++++++//
// CLASS: Skellyworm
//++++++++++++++++++++++++++++++//

public class Skellyworm : Character {
    
    //------------------------------//
    // Character States
    //------------------------------//
    
    public CharacterState WalkState { get; private set; }
    public CharacterState DieState { get; private set; }
    
    //------------------------------//
    // Properties
    //------------------------------//
    
    public bool IsAlive => _characterHealth.IsAlive;
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Character States")]
    [SerializeField] private SkellywormStateScript spawnStateScript;
    [SerializeField] private SkellywormStateScript walkStateScript;
    [SerializeField] private SkellywormStateScript meleeAttackStateScript;
    [SerializeField] private SkellywormStateScript projectileAttackStateScript;
    [SerializeField] private SkellywormStateScript dieStateScript;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private float meleeCooldown = 1f;
    
    [Header("Projectile Attack")]
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private float projectileCooldown = 1f;
    
    //:::::::::::::::::::::::::::::://
    // Animator Hashes
    //:::::::::::::::::::::::::::::://
    
    private readonly int _hashAnimState = Animator.StringToHash("AnimState");
    
    //:::::::::::::::::::::::::::::://
    // Character States
    //:::::::::::::::::::::::::::::://

    private CharacterState _spawnState;
    private CharacterState _meleeAttackState;
    private CharacterState _projectileAttackState;
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Player _player;
    private Animator _animator;
    private CharacterHealth _characterHealth;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _lastAttack;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected override void Awake() {
        base.Awake();
        Configure();
    }

    protected override void OnEnable() {
        base.OnEnable();
        
        // reset fields to defaults
        _lastAttack = 0f;
        
        // start at spawn state
        StateMachine.ChangeState(_spawnState);
    }

    protected override void Update() {
        base.Update();
        
        // increment fields
        _lastAttack += Time.deltaTime;
    }
    
    //------------------------------//
    // Movement
    //------------------------------//

    public void MoveTowardsPlayer() {
        // get the direction vector from the enemy to the player
        var direction = (_player.transform.position - transform.position).normalized;
        
        // add motion towards the player
        AddMotion(moveSpeed * Time.deltaTime * direction);
    }

    public void AttackPlayer() {
        // get the distance to the player
        var distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance <= meleeRange && meleeCooldown <= _lastAttack) {
            // within melee range and melee attack is not on cooldown; change to melee attack state
            StateMachine.ChangeState(_meleeAttackState);
        } else if (distance <= projectileRange && projectileCooldown <= _lastAttack) {
            // within projectile range and projectile attack is not on cooldown; change to projectile attack state
            StateMachine.ChangeState(_projectileAttackState);
        } else {
            // skellyworm can't attack, we're done
            return;
        }

        // skellyworm attacked player, reset cooldown
        _lastAttack = 0f;
    }
    
    //------------------------------//
    // Animations
    //------------------------------//

    public void SetAnimState(int state) {
        _animator.SetInteger(_hashAnimState, state);
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // get required components (these won't be null)
        _characterHealth = GetComponent<CharacterHealth>();
        
        // get components
        _animator = transform.GetComponentInChildren<Animator>();
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_animator, "Animator component is null");
        //++++++++++++++++++++++++++++++//
        
        // get the player instance
        _player = Player.Instance;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_player, "Player instance is null");
        //++++++++++++++++++++++++++++++//
        
        // initialise public CharacterStates
        WalkState = new CharacterState(this, Instantiate(walkStateScript));
        DieState = new CharacterState(this, Instantiate(dieStateScript));
        
        // initialise private CharacterStates
        _spawnState = new CharacterState(this, Instantiate(spawnStateScript));
        _meleeAttackState = new CharacterState(this, Instantiate(meleeAttackStateScript));
        _projectileAttackState = new CharacterState(this, Instantiate(projectileAttackStateScript));
    }
}