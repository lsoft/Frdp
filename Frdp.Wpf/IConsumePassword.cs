using System.Security;

namespace Frdp.Wpf
{
    /// <summary>
    /// ����������� ������
    /// </summary>
    public interface IConsumePassword
    {
        /// <summary>
        /// ���������� ������ ����, ��� ��� ���������� �� ���� �����, ��� �� ���������
        /// </summary>
        /// <param name="password">������</param>
        void SetPassword(SecureString password);
    }
}