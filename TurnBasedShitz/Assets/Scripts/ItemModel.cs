using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ItemModel
    {
        public string ItemName { get; set; } = "";
        public string Description { get; set; } = "";
        public Dictionary<string, Stat> Stats { get; set; } = new Dictionary<string, Stat>();

        public ItemModel(string name, string desc, Dictionary<string, Stat> statList)
        {
            ItemName = name;
            Description = desc;
            Stats = statList;
        }

    }

    public class Stat
    {
        public int AttackPower = 0;
        public int DefensePower = 0;
        public int HealthPower = 0;
        public int ManaPower = 0;
    }

}
