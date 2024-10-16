using System;
using System.Collections.Generic;

public interface ICloneable
{
    object Clone();
}

public class Character : ICloneable
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }
    public List<Skill> Skills { get; set; }

    public Character()
    {
        Skills = new List<Skill>();
    }

    public object Clone()
    {
        Character clone = (Character)this.MemberwiseClone();
        clone.Weapon = (Weapon)this.Weapon.Clone();
        clone.Armor = (Armor)this.Armor.Clone();
        clone.Skills = new List<Skill>();
        foreach (var skill in Skills)
        {
            clone.Skills.Add((Skill)skill.Clone());
        }
        return clone;
    }
}

public class Weapon : ICloneable
{
    public string Name { get; set; }
    public int Damage { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class Armor : ICloneable
{
    public string Name { get; set; }
    public int Defense { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class Skill : ICloneable
{
    public string Name { get; set; }
    public int Power { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

class Program
{
    static void Main()
    {
        Weapon sword = new Weapon { Name = "Sword", Damage = 15 };
        Armor shield = new Armor { Name = "Shield", Defense = 10 };
        Skill fireball = new Skill { Name = "Fireball", Power = 20 };

        Character hero = new Character
        {
            Name = "Hero",
            Health = 100,
            Strength = 20,
            Agility = 15,
            Intelligence = 10,
            Weapon = sword,
            Armor = shield
        };
        hero.Skills.Add(fireball);

        Character clonedHero = (Character)hero.Clone();
        clonedHero.Name = "Cloned Hero";
        clonedHero.Health = 90;
        clonedHero.Weapon.Damage = 18;
        clonedHero.Skills[0].Power = 25;

        Console.WriteLine($"{hero.Name}: {hero.Health}, {hero.Weapon.Name}, {hero.Skills[0].Power}");
        Console.WriteLine($"{clonedHero.Name}: {clonedHero.Health}, {clonedHero.Weapon.Name}, {clonedHero.Skills[0].Power}");
    }
}
