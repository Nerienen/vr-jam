namespace VRJammies.Framework.Core.Boss
{
    public interface IAttack
    {
        public bool CanAttack();
        public void Attack();
    }
}