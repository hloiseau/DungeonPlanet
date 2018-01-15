using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DungeonPlanet.Library
{
    [Serializable]
    public class PlayerInfo
    {
        public int Life { get; set; }
        public int Money { get; set; }
        public int Energy { get; set; }
        public WeaponState SerializableWeapon { get { return ActualWeapon; } set { ActualWeapon = value; } }
        public static WeaponState ActualWeapon;
        public enum WeaponState
        {
            None,
            Fire,
            Slime
        }
        public Level.LevelID Progress { get; set; }

        public PlayerInfo()
        {
            Life = 100;
            Money = 100;
            Energy = 100;
            Progress = Level.LevelID.Five;
        }
       
        public void Save(string filePath)
        {
            BinaryFormatter f = new BinaryFormatter();
            using (var stream = File.OpenWrite(filePath))
            {
                f.Serialize(stream, this);
            }
        }

        static public PlayerInfo LoadFrom(string filePath)
        {
            BinaryFormatter f = new BinaryFormatter();
            using (var stream = File.OpenRead(filePath))
            {
                PlayerInfo playerInfo;
                /*try 
                 {*/
                playerInfo = (PlayerInfo)f.Deserialize(stream);
                /*}*/
                /*catch 
                 * { 
                 * }*/
                playerInfo.Progress = (Level.LevelID)Case.Clamp((int)playerInfo.Progress, 1, 5);
                playerInfo.Life = 100;
                playerInfo.Energy = 100;
                return playerInfo;
            }
        }
    }
}
