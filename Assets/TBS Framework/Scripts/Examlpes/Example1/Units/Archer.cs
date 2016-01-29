public class Archer : MyUnit
{
    protected override void Defend(Unit other, int damage)
    {
        var realDamage = damage;
        if (other is Paladin)
            realDamage *= 2;//Paladin deals double damage to archer.

        base.Defend(other, realDamage);
    }
}
