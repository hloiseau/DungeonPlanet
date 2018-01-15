using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonPlanet.Library
{
    public class NPCDialogLib
    {
        Random _random;
        int _cursor;
        List<String> _wiseLign;
        List<String> _chroniclerLign;

        public NPCDialogLib()
        {
            _random = new Random();
            _cursor = 0;
            _wiseLign = new List<string>();
            _chroniclerLign = new List<string>();
        }

        public string ChooseSentenceForWise()
        {
            _wiseLign.Add("Connaissez vous le jeune Clyde Vanilla ? il est tres connu lui et ses compagnons, menes par le magnifique sergent Moustachios.");
            _wiseLign.Add("Allumer le feux !!! hey, tu sais tu peux acheter des munitions enflammees chez l armurier.");
            _wiseLign.Add("I play DonjonPlanet every day I play Donjon...  Ho ho ho j adore cette chanson !");
            _wiseLign.Add("On m appelle l ovni tatata... J ai cette musique dans la tete depuis plus d'une heure et je ne l aime meme pas !");
            _wiseLign.Add("Un vieux proverbe dit que si vous nous notez bien vous aurez des potins. Noter qui ? J'en sais rien moi !");
            _wiseLign.Add("C etait mieux avant !");
            _wiseLign.Add("SEUL LINK PEUT VAINCRE GANON !! ... Au fait c'est qui ganon ?");

            int Numberlign = RandomNumber(_wiseLign.Count);

            return _wiseLign[Numberlign];
        }

        public string ChooseSentenceForChronicler()
        {
            _chroniclerLign.Add("Bonjour chere scientifique ! Que puis-je pour vous aujourd hui ?");
            _chroniclerLign.Add("2");
            _chroniclerLign.Add("3");
            _chroniclerLign.Add("4");

            string toSay = _chroniclerLign[_cursor];

            _cursor++;

            return toSay;
        }

        public int RandomNumber(int max)
        {
            int lign = _random.Next(0, max);
            return lign;
        }
    }
}
