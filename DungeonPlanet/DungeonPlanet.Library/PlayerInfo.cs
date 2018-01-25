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
        public BulletState SerializableBullet { get { return ActualBullet; } set { ActualBullet = value; } }
        public static BulletState ActualBullet;
        public enum BulletState
        {
            None,
            Fire,
            Slime
        }

        [Flags]
        public enum WeaponState
        {
            Normal = 0,
            Shotgun = 1,
            Launcher = 2
        }
        public Level.LevelID Progress { get; set; }
        public WeaponState Unlocked { get; set; }

        public PlayerInfo()
        {
            Life = 100;
            Money = 0;
            Energy = 100;
            Progress = Level.LevelID.One;
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
                if(playerInfo.Progress == (Level.LevelID)6)
                {
                    Level.ActualState = Level.State.End;
                }
                playerInfo.Progress = (Level.LevelID)Case.Clamp((int)playerInfo.Progress, 1, 5);
                return playerInfo;
            }
        }
    }
}
