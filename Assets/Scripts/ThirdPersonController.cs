﻿using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.UI;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Test")]
        [Tooltip("테스트용이라고")]
        public AnimatorOverrideController spearAOC;
        public AnimatorOverrideController origin;//애니메이터 오버라이드 컨트롤러, 테스트용
        public float hp = 100;
        public Image hpBar;
        private bool die = false;

        [Header("AnimatorOverrideController")]
        [Tooltip("AnimatorOverrideController 저장용")]
        [SerializeField] private AnimatorOverrideController[] animators;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        //추가
        private float _atkCoolTime = 0.0f;
        private int _atkComSeq = 0;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        //추가
        public List<int> Atks = new List<int>();
        private int _animIDSwdAttack_1;
        private int _animIDSwdAttack_2;
        private int _animIDSwdAttack_3;
        private int _animIDSwdAtkEnd;
        private bool _nextAtk = false;
        private bool inUI = false;

        public bool invincibility = false;
        private bool isStuned;




#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {

            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();
            Atks.Add(_animIDSwdAttack_1);
            Atks.Add(_animIDSwdAttack_2);
            Atks.Add(_animIDSwdAttack_3);
            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            hpBar.fillAmount = hp / 100.0f;
            if (Input.GetKeyUp(KeyCode.Escape) && inUI == false)
            {
                inUI = true;
            }
            else if (Input.GetKeyUp(KeyCode.Escape) && inUI == true)
            {
                inUI = false;
            }
            if (!inUI)
            {
                if (_atkCoolTime > 0)
                {
                    //Debug.Log("쿨타임 진행중");
                    GroundedCheck();
                    Attack();
                }
                else
                {
                    JumpAndGravity();
                    GroundedCheck();
                    Move();
                    Attack();
                    if (_input.weapon)
                    {
                        //_animator.runtimeAnimatorController = spearAOC;//테스트용 무기 교체
                    }
                }
            }
        }


        private void CoolTime()
        {
            switch (_atkComSeq)
            {
                case 0:
                    _atkCoolTime = animators[Data.data.nowWeapon]["FirstAttack"].length; break;
                case 1:
                    _atkCoolTime = animators[Data.data.nowWeapon]["SecondAttack"].length; break;
                case 2:
                    _atkCoolTime = animators[Data.data.nowWeapon]["ThirdAttack"].length; break;
                default: break;
            }
        }

        //무기 교체했을 때, 총 콤보크기 변환이 안되는 문제가 있다.

        private void Attack()
        {
            //bug.Log(_animator.GetBool(Atks[_atkComSeq]));
            //재설계: 3개의 클립이 이어지는 코드를 만들고, 연결부위가 닫혀있으면 원래로 돌아가게 한다.
            //int.Parse(ComboList[_atkComSeq].Substring(2)): ComboList리스트의 현 시퀀스인덱스에서 추출한 기술코드에서 anim동작코드 추출
            if (_atkCoolTime > 0)//공격이 실행 중에
            {
                _atkCoolTime -= Time.deltaTime;//쿨타임이 줄고

                if (_input.attack && _atkComSeq != 0 && !_animator.GetBool(Atks[_atkComSeq]))//추가 공격 입력 시, 콤보루트 애니메이션 활성화
                {//기본 상태가 아니면 마지막 공격에서 트리거를 죄다 켜버리는 버그가 있다. 해결완료
                    _nextAtk = true;
                    _animator.SetTrigger(Atks[_atkComSeq]);
                    _animator.ResetTrigger(_animIDSwdAtkEnd);
                }
            }
            else//공격이 실행 중이지 않을 때, 혹은 공격이 끝났을 때
            {
                if (_nextAtk)//다음 동작이 열려있을 때
                {
                    Data.data.equipWeapon.GetComponent<Weapon>().Use();
                    _nextAtk = false;
                    CoolTime();//쿨타임 돌리고
                    _animator.SetTrigger(_animIDSwdAtkEnd);//동작 종료 트리거 켜두고
                    if (_atkComSeq >= Data.data.ComboList[Data.data.nowWeapon].Count - 1)//마지막 콤보라면 처음으로, 아니라면 다음 공격으로
                    {
                        _atkComSeq = 0;
                    }
                    else
                    {
                        _atkComSeq++;
                    }
                }
                else//_SwordAtks[_aktComSeq] == false일 때. 즉, 다음 공격 미입력 시
                {
                    _atkComSeq = 0;
                }

                if (_input.attack && Grounded)//첫 공격 발생, 땅에 있을 때만
                {
                    Data.data.equipWeapon.GetComponent<Weapon>().Use();
                    _animator.SetTrigger(Atks[0]);
                    CoolTime();
                    if (Data.data.ComboList[Data.data.nowWeapon].Count > 1)
                        _atkComSeq++;
                    _animator.SetTrigger(_animIDSwdAtkEnd);
                }
            }
            _input.attack = false;//어택 인풋 해제
        }


        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDSwdAttack_1 = Animator.StringToHash("Attack 1");
            _animIDSwdAttack_2 = Animator.StringToHash("Attack 2");
            _animIDSwdAttack_3 = Animator.StringToHash("Attack 3");
            _animIDSwdAtkEnd = Animator.StringToHash("AtkEnd");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
        public void KnuckBack(Vector3 vec3, int knuckBackP)
        {
            invincibility = true;
            GetComponent<CharacterController>().enabled = false;
            transform.LookAt(new Vector3(vec3.x, transform.position.y, vec3.z));
            _animator.ResetTrigger("AtkEnd");
            _animator.ResetTrigger("Attack 1");
            _animator.ResetTrigger("Attack 2");
            _animator.ResetTrigger("Attack 3");
            _animator.SetTrigger("KnuckBack");
            GetComponent<Rigidbody>().AddForce((transform.position - new Vector3(vec3.x, transform.position.y, vec3.z)).normalized * knuckBackP, ForceMode.Impulse);
            isStuned = true;
            hp -= 10;
        }

        public void Die()
        {
            if (die)
            {
                GetComponent<Animator>().enabled = false;
                GetComponent<CharacterController>().enabled = false;
                GetComponent<ThirdPersonController>().enabled = false;
                GetComponent<BasicRigidBodyPush>().enabled = false;
                GetComponent<StarterAssetsInputs>().enabled = false;
                GetComponent<PlayerInput>().enabled = false;
            }
        }
        public void StunEnd()
        {
            GetComponent<CharacterController>().enabled = true;
            Invoke("ProtectEnd", 0.5f);
            isStuned = false;
        }

        public void ProtectEnd()
        {
            invincibility = false;
        }

        public void SlowStart()
        {
            MoveSpeed = 1.0f;
            SprintSpeed = 2.5f;
            SpeedChangeRate = 5.0f;
        }
        public void SlowEnd()
        {
            MoveSpeed = 2.0f;
            SprintSpeed = 5.335f;
            SpeedChangeRate = 10.0f;
        }
    }
}