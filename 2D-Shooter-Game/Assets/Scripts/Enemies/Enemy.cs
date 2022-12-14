using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which controls enemy behaviour
/// </summary>

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The speed at which the enemy moves.")]
    public float moveSpeed = 5.0f;
    [Tooltip("The score value for defeating this enemy")]
    public int scoreValue = 5;

    [Header("Following Settings")]
    [Tooltip("The transform of the object that this enemy should follow.")]
    public Transform followTarget = null;
    [Tooltip("The distance at which the enemy begins following the follow target.")]
    public float followRange = 10.0f;

    [Header("Shooting")]
    [Tooltip("The enemy's gun components")]
    public List<ShootingController> guns = new List<ShootingController>();

    /// <summary>
    /// Enum to help with shooting modes
    /// </summary>
    public enum ShootMode { None, ShootAll };

    [Tooltip("The way the enemy shoots:\n" +
        "None: Enemy does not shoot.\n" +
        "ShootAll: Enemy fires all guns whenever it can.")]
    public ShootMode shootMode = ShootMode.ShootAll;

    //Enum to help wih different movement modes
    public enum MovementModes { NoMovement, FollowTarget, Scroll };

    [Tooltip("The way this enemy will move\n" +
        "NoMovement: This enemy will not move.\n" +
        "FollowTarget: This enemy will follow the assigned target.\n" +
        "Scroll: This enemy will move in one horizontal direction only.")]
    public MovementModes movementMode = MovementModes.FollowTarget;

    //The direction that this enemy will try to scroll if it is set as a scrolling enemy.
    [SerializeField] private Vector3 scrollDirection = Vector3.right;

    //Standard Unity function called after update every frame
    private void LateUpdate()
    {
        HandleBehaviour();       
    }

    //Standard Unity function called once before the first call to Update
    private void Start()
    {
        if (movementMode == MovementModes.FollowTarget && followTarget == null)
        {
            if (GameManager.instance != null && GameManager.instance.player != null)
            {
                followTarget = GameManager.instance.player.transform;
            }
        }
    }

    //Handles moving and shooting in accordance with the enemy's set behaviour
    private void HandleBehaviour()
    {
        // Check if the target is in range, then move
        if (followTarget != null && (followTarget.position - transform.position).magnitude < followRange)
        {
            MoveEnemy();
        }
        // Attempt to shoot, according to this enemy's shooting mode
        TryToShoot();
    }

    //This is meant to be called before destroying the gameobject associated with this script
    //It can not be replaced with OnDestroy() because of Unity's inability to distiguish between unloading a scene
    //and destroying the gameobject from the Destroy function
    public void DoBeforeDestroy()
    {
        AddToScore();
        IncrementEnemiesDefeated();
    }

    //Adds to the game manager's score the score associated with this enemy if one exists
    private void AddToScore()
    {
        if (GameManager.instance != null && !GameManager.instance.gameIsOver)
        {
            GameManager.AddScore(scoreValue);
        }
    }

    //Increments the game manager's number of defeated enemies
    private void IncrementEnemiesDefeated()
    {
        if (GameManager.instance != null && !GameManager.instance.gameIsOver)
        {
            GameManager.instance.IncrementEnemiesDefeated();
        }       
    }

    //Moves the enemy and rotates it according to it's movement mode
    private void MoveEnemy()
    {
        // Determine correct movement
        Vector3 movement = GetDesiredMovement();

        // Determine correct rotation
        Quaternion rotationToTarget = GetDesiredRotation();

        // Move and rotate the enemy
        transform.position = transform.position + movement;
        transform.rotation = rotationToTarget;
    }

    //Calculates the movement of this enemy
    protected virtual Vector3 GetDesiredMovement()
    {
        Vector3 movement;
        switch(movementMode)
        {
            case MovementModes.FollowTarget:
                movement = GetFollowPlayerMovement();
                break;
            case MovementModes.Scroll:
                movement = GetScrollingMovement();
                break;
            default:
                movement = Vector3.zero;
                break;
        }
        return movement;
    }

    //Calculates and returns the desired rotation of this enemy
    protected virtual Quaternion GetDesiredRotation()
    {
        Quaternion rotation;
        switch (movementMode)
        {
            case MovementModes.FollowTarget:
                rotation = GetFollowPlayerRotation();
                break;
            case MovementModes.Scroll:
                rotation = GetScrollingRotation();
                break;
            default:
                rotation = transform.rotation; ;
                break;
        }
        return rotation;
    }

    //Tries to fire all referenced ShootingController scripts depends on shootMode variable
    private void TryToShoot()
    {
        switch (shootMode)
        {
            case ShootMode.None:
                break;
            case ShootMode.ShootAll:
                foreach (ShootingController gun in guns)
                {
                    gun.Fire();
                }
                break;
        }
    }

    //The direction and magnitude of the enemy's desired movement in follow mode
    private Vector3 GetFollowPlayerMovement()
    {
        Vector3 moveDirection = (followTarget.position - transform.position).normalized;
        Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
        return movement;
    }

    //The desired rotation of follow movement mode
    private Quaternion GetFollowPlayerRotation()
    {
        float angle = Vector3.SignedAngle(Vector3.down, (followTarget.position - transform.position).normalized, Vector3.forward);
        Quaternion rotationToTarget = Quaternion.Euler(0, 0, angle);
        return rotationToTarget;
    }

    //The direction and magnitude of the enemy's desired movement in scrolling mode
    private Vector3 GetScrollingMovement()
    {
        scrollDirection = GetScrollDirection();
        Vector3 movement = scrollDirection * moveSpeed * Time.deltaTime;
        return movement;
    }

    //The desired rotation of scrolling movement mode
    private Quaternion GetScrollingRotation()
    {
        return Quaternion.identity;
    }

    //Determines the direction to move in with scrolling movement mode
    private Vector3 GetScrollDirection()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            Vector2 screenPosition = camera.WorldToScreenPoint(transform.position);
            Rect screenRect = camera.pixelRect;
            if (!screenRect.Contains(screenPosition))
            {
                return scrollDirection * -1;
            }
        }
        return scrollDirection;
    }
}
