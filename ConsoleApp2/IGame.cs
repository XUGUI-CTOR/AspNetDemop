using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class IGame
    {

    }
    internal class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; }

        public void Notity(int damageValue)
        {
            HP -= damageValue;
            Console.WriteLine($"给与怪物{Name}{damageValue}伤害");
            if(HP<=0)
                Console.WriteLine($"怪物{Name}挂了");
            else
                Console.WriteLine($"怪物还剩{HP}HP");
        }
    }
    interface IAttackStrategy
    {
        void AttackTarget(Monster monster);
    }
    internal sealed class WoodSword : IAttackStrategy
    {
        public void AttackTarget(Monster monster)
        {
            monster.Notity(20);
        }
    }

    internal sealed class IronSword : IAttackStrategy
    {
        public void AttackTarget(Monster monster)
        {
            monster.Notity(50);
        }
    }

    internal sealed class MagicSword : IAttackStrategy
    {
        private readonly Random random = new Random();
        public void AttackTarget(Monster monster)
        {
            int damageValue = random.NextDouble() > 0.5 ? 200 : 100;
            monster.Notity(damageValue);
        }
    }

    internal class BraveMan
    {
        public IAttackStrategy Weapon { get; set; }
        public void Attack(Monster monster)
        {
            Weapon.AttackTarget(monster);
        }
    }
}
