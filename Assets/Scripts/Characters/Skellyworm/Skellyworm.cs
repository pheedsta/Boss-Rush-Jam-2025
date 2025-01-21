using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

[RequireComponent(typeof(Health))]

//++++++++++++++++++++++++++++++//
// CLASS: Skellyworm
//++++++++++++++++++++++++++++++//

public class Skellyworm : Character, IReusable {
    
    //------------------------------//
    // IReusable Properties
    //------------------------------//

    public string Identifier => identifier;
    
    //------------------------------//
    // Character States
    //------------------------------//
    
    public CharacterState WalkState { get; private set; }
    public CharacterState DieState { get; private set; }
    
    //------------------------------//
    // Properties
    //------------------------------//

    public Health Health => GetHealth();
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("IReusable")]
    [SerializeField] private string identifier;
    
    [Header("Character States")]
    [SerializeField] private SkellywormStateScript spawnStateScript;
    [SerializeField] private SkellywormStateScript walkStateScript;
    [SerializeField] private SkellywormStateScript meleeAttackStateScript;
    [SerializeField] private SkellywormStateScript projectileAttackStateScript;
    [SerializeField] private SkellywormStateScript dieStateScript;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 1f;
    
    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private float meleeCooldown = 1f;
    
    [Header("Projectile Attack")]
    [SerializeField] private Projectile projectilePrefab;
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
    // Properties
    //:::::::::::::::::::::::::::::://
    
    private Player Player => GetPlayer();
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Player _player;
    private Health _health;
    //private Animator _animator;
    
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
        
        // register Skellyworm with ComponentRegistry
        ComponentRegistry.Register(this);
        
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

    protected override void LateUpdate() {
        base.LateUpdate();

        // if skellyworm is dead, do nothing
        if (!_health.IsAlive) return;
        
        // determine which direction to rotate towards
        var targetDirection = Player.transform.position - transform.position;
        
        // rotate towards the player
        var rotation = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    protected override void OnDisable() {
        base.OnDisable();
        
        // deregister Skellyworm from ComponentRegistry
        ComponentRegistry.Deregister(this);
    }

    //------------------------------//
    // Movement
    //------------------------------//

    public void MoveTowardsPlayer() {
        // if skellyworm is dead, do nothing
        if (!_health.IsAlive) return;
        
        // get the direction vector from the enemy to the player
        //var direction = (Player.transform.position - transform.position).normalized;
        
        // add motion towards the player (just move in direction we're facing instead)
        AddMotion(moveSpeed * Time.deltaTime * transform.forward);
    }

    public void ShootProjectileTowardsPlayer() {
        // if there is no projectile prefab, we're done
        if (!projectilePrefab) return;
        
        // we need the acid to come from the top of the worm
        var position = transform.position;
        position.y += 1.555f;
        
        // instantiate fireball
        _ = ReusablePool.FetchReusable(projectilePrefab, position, transform.rotation);
        
    }

    public void AttackPlayer() {
        // get the distance to the player
        var distance = Vector3.Distance(transform.position, Player.transform.position);

        // no melee attack for now????
        //if (distance <= meleeRange && meleeCooldown <= _lastAttack) {
            // within melee range and melee attack is not on cooldown; change to melee attack state
            //StateMachine.ChangeState(_meleeAttackState);
        //} else if (distance <= projectileRange && projectileCooldown <= _lastAttack) {
        
        if (distance <= projectileRange && projectileCooldown <= _lastAttack) {
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
        //_animator.SetInteger(_hashAnimState, state);
    }
    
    // Getters

    private Player GetPlayer() {
        // if a player has already been found, we're done
        if (_player) return _player;
        
        // get the player instance
        _player = Player.Instance;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_player, "Player instance is null");
        //++++++++++++++++++++++++++++++//
        
        // return the player instance
        return _player;
    }
    
    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private Health GetHealth() {
        // if Health component has already been found, we're done
        if (_health) return _health;
        
        // Health is a required component so it will never be null
        _health = GetComponent<Health>();
        
        // return Health component
        return _health;
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // get required components (these won't be null)
        _health = GetComponent<Health>();
        
        // get components
        //_animator = transform.GetComponentInChildren<Animator>();
        //++++++++++++++++++++++++++++++//
        //Debug.Assert(_animator, "Animator component is null");
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