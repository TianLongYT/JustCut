using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [Obsolete]
    /// <summary>
    /// [��������]
    /// ����Ѿ���ס�Ҽ�����ʼ����ʱ��ʼ��������ʱҪ���Ƿ����ǳ���ʱ������ꡣ
    /// ������������ۣ�
    /// ���ع������ܶ�����
    ///     �ɿ��Ҽ���
    ///         �ڳ���ǰҡ�ɿ����ȴ�ǰҡ����������
    ///         �ڳ��ܽ���ǰ�ɿ�������������    
    ///         ���ܽ������ɿ������Ƿ���û��ֹͣ����������������
    ///         �ܽ᣺���ܽ�����ʱ���Ǽ��Ϸ�����ʱ�䡣�����������ڶ����ع���һ�¡�
    ///     ���������
    ///         ת�ɳ��ع�������ǰ������ʱ��Ҫ�洢�����������³��ع���ʱʹ����֡��
    /// �ӳ��ع������ܶ�����
    ///     �ɿ���������Ҽ���
    ///         ���ع������ܶ������߼���ͬ��
    ///         
    /// �����Ϻ�ԭ�����ֶ���û���𡣳����޵к��Զ�ȡ��ʱ�䡣
    /// </summary>
    public class PlayerChargingRollState : BasePlayerState
    {
        public PlayerChargingRollState(PlayerStateMachine sM) : base(sM)
        {
        }
    }
}