using UnityEngine;

//------------------------------//
// Required Components
//------------------------------//

//[RequireComponent(typeof(Animator))]

//++++++++++++++++++++++++++++++//
// CLASS: SkellywormEnemy
//++++++++++++++++++++++++++++++//

public class SkellywormEnemy : Character {
    
    //------------------------------//
    // Properties
    //------------------------------//
    
    public bool CanMeleeAttack => GetCanMeleeAttack();
    public bool CanProjectileAttack => GetCanProjectileAttack();
    
    //------------------------------//
    // Character State Properties
    //------------------------------//

    public CharacterState SpawnState { get; private set; }
    public CharacterState WalkState { get; private set; }
    public CharacterState MeleeAttackState { get; private set; }
    public CharacterState ProjectileAttackState { get; private set; }
    public CharacterState DieState { get; private set; }
    
    //:::::::::::::::::::::::::::::://
    // Animator Hashes
    //:::::::::::::::::::::::::::::://
    
    private readonly int AnimatorHashAnimState = Animator.StringToHash("AnimState");
    
    //:::::::::::::::::::::::::::::://
    // Serialized Fields
    //:::::::::::::::::::::::::::::://
    
    [Header("Character States")]
    [SerializeField] private CharacterStateScript spawnStateScript;
    [SerializeField] private CharacterStateScript walkStateScript;
    [SerializeField] private CharacterStateScript meleeAttackStateScript;
    [SerializeField] private CharacterStateScript projectileAttackStateScript;
    [SerializeField] private CharacterStateScript dieStateScript;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private float meleeCooldown = 1f;
    
    [Header("Projectile Attack")]
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private float projectileCooldown = 1f;
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://

    private float DistanceToPlayer => GetDistanceToPlayer();
    
    //:::::::::::::::::::::::::::::://
    // Components
    //:::::::::::::::::::::::::::::://
    
    private Animator _animator;
    private Player _player;
    
    //:::::::::::::::::::::::::::::://
    // Local Fields
    //:::::::::::::::::::::::::::::://

    private float _lastMeleeAttack;
    private float _lastProjectileAttack;
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://

    protected override void Awake() {
        base.Awake();
        
        // configure enemy
        Configure();
    }

    private void OnEnable() {
        // reset defaults
        _lastMeleeAttack = meleeCooldown;
        //_lastProjectileAttack = projectileCooldown;
        
        // start at spawn state
        StateMachine.ChangeState(SpawnState);
    }

    protected override void Update() {
        base.Update();
        
        // increment fields
        _lastMeleeAttack += Time.deltaTime;
        _lastProjectileAttack += Time.deltaTime;
    }

    private void OnDisable() {
        // reset fields to defaults
        _lastMeleeAttack = 0f;
        _lastProjectileAttack = 0f;
    }

    //:::::::::::::::::::::::::::::://
    // Getters
    //:::::::::::::::::::::::::::::://

    private bool GetCanMeleeAttack() {
        if (_lastMeleeAttack < meleeCooldown) return false;
        return DistanceToPlayer <= meleeRange;
    }

    private bool GetCanProjectileAttack() {
        if (_lastProjectileAttack < projectileCooldown) return false;
        return DistanceToPlayer <= projectileRange;
    }

    private float GetDistanceToPlayer() {
        return Vector3.Distance(transform.position, _player.transform.position);
    }
    
    //------------------------------//
    // Movement
    //------------------------------//

    public void MoveTowardsPlayer() {
        // get enemy and player positions
        var enemyPosition = transform.position;
        var playerPosition = _player.transform.position;

        // get the direction vector from the enemy to the player
        var direction = (playerPosition - enemyPosition).normalized;
        
        // add motion
        AddMotion(moveSpeed * Time.deltaTime * direction);
    }

    public void MeleeAttackPlayer() {
        // reset field (this makes attack go on cooldown)
        _lastMeleeAttack = 0f;
        
        // change to melee attack state
        StateMachine.ChangeState(MeleeAttackState);
    }

    public void ProjectileAttackPlayer() {
        // reset field (this makes attack go on cooldown)
        _lastProjectileAttack = 0f;
        
        // change to projectile attack state
        StateMachine.ChangeState(ProjectileAttackState);
    }
    
    //------------------------------//
    // Animations
    //------------------------------//

    public void ChangeAnimationState(int state) {
        _animator.SetInteger(AnimatorHashAnimState, state);
    }
    
    //:::::::::::::::::::::::::::::://
    // Configuration
    //:::::::::::::::::::::::::::::://

    private void Configure() {
        // get required components
        _animator = transform.Find("Sprite Renderer").GetComponent<Animator>();
        
        // initialise CharacterStates
        SpawnState = new CharacterState(this, Instantiate(spawnStateScript));
        WalkState = new CharacterState(this, Instantiate(walkStateScript));
        MeleeAttackState = new CharacterState(this, Instantiate(meleeAttackStateScript));
        ProjectileAttackState = new CharacterState(this, Instantiate(projectileAttackStateScript));
        DieState = new CharacterState(this, Instantiate(dieStateScript));
        
        // get the player instance
        _player = Player.Instance;
        //++++++++++++++++++++++++++++++//
        Debug.Assert(_player, "Player instance is null");
        //++++++++++++++++++++++++++++++//
    }
}
