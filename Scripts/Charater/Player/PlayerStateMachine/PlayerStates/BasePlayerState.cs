
using iysy.GameInput;

namespace iysy.JustCut
{

    /// <summary>
    /// ״̬����ҪInputBuffer,InteractC,Triggle,ainmator�ȣ��Լ�StateMachine���״̬ת����
    /// </summary>
    public abstract class BasePlayerState : iysy.StateMachine.IState
    {
        protected PlayerStateMachine stateMachine;
        //������Ϊÿ��״̬����Ҫ����������ų�����ֹ�ظ���ȡ��
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
        /// �����ײ��Ӵ�ʱ���á�
        /// </summary>
        /// <param name="interactType">��������Ϣ</param>
        /// <returns>true:��Ҵ��ڴ���޵�״̬��false:��Ҵ��ڷ��޵�״̬��Ҫ��Ѫ�ȡ�</returns>
        public virtual bool OnInteract(AttackData attackData) 
        {
            return false; 
        }
        /// <summary>
        /// ���е���ʱ��Ҫ����ͬ���
        /// </summary>
        /// <param name="hitType">0-���е������壬1-ƴ���ɹ���2-����ǰ����ƴ����Ҫ��ת��������</param>
        public virtual void OnHit(int hitType,float hitIntensity) { }
    }
}