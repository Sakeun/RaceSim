using System;
namespace Model
{
    public class Pod : IEquipment
    {
        public Pod()
        {
            Quality = 0;
            Performance = 0;
            Speed = 0;
            IsBroken = false;
        }

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
    }
}