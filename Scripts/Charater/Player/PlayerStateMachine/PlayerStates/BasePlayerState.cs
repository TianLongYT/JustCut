
using iysy.GameInput;

namespace iysy.JustCut
{

    /// <summary>
    /// 状态类需要InputBuffer,InteractC,Triggle,ainmator等，以及StateMachine完成状态转换。
    /// </summary>
    public abstract class BasePlayerState : iysy.StateMachine.IState
    {
        protected PlayerStateMachine stateMachine;
        //输入因为每个状态都需要，所以特意放出来防止重复获取。
        protected IInputBuffer<PlayerInputType> inputBuffer;
        protected PlayerLockController lockController;
        protected readonly ChannelData channelData = new ChannelData { channelMode = ChannelMode.Int ,Channel = 0};
        public BasePlayerState(PlayerStateMachine sM)
        {
            this.stateMachine = sM;
            this.inputBuffer = sM.Input.inputBuffer;
            this.lockController = sM.Input.lockController;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnPrepare(float deltaTime) { }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnTrigger(string msg) { }

        /// <summary>
        /// 外界碰撞框接触时调用。
        /// </summary>
        /// <param name="interactType">传入打击信息</param>
        /// <returns>true:玩家处于打击无敌状态。false:玩家处于非无敌状态，要扣血等。</returns>
        public virtual bool OnInteract(AttackData attackData) 
        {
            return false; 
        }
        /// <summary>
        /// 击中敌人时需要处理不同情况
        /// </summary>
        /// <param name="hitType">0-击中敌人肉体，1-拼刀成功，2-击中前出现拼刀需要跳转动画进度</param>
        public virtual void OnHit(int hitType,float hitIntensity) { }
    }
}