﻿using System;
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
        List<String> _wiseLine;
        List<String> _chroniclerLine;

        public NPCDialogLib()
        {
            _random = new Random();
            _cursor = 0;
            _wiseLine = new List<string>();
            _chroniclerLine = new List<string>();
        }

        public string ChooseSentenceForWise()
        {
            _wiseLine.Add("Connaissez vous le jeune Clyde Vanilla ? il est tres connu lui et ses compagnons, menes par le magnifique sergent Moustachios.");
            _wiseLine.Add("Allumer le feux !!! hey, tu sais tu peux acheter des munitions enflammees chez l armurier.");
            _wiseLine.Add("I play DonjonPlanet every day I play Donjon...  Ho ho ho j adore cette chanson !");
            _wiseLine.Add("On m appelle l ovni tatata... J ai cette musique dans la tete depuis plus d'une heure et je ne l aime meme pas !");
            _wiseLine.Add("Un vieux proverbe dit que si vous nous notez bien vous aurez des potins. Noter qui ? J'en sais rien moi !");
            _wiseLine.Add("C etait mieux avant !");
            _wiseLine.Add("SEUL LINK PEUT VAINCRE GANON !! ... Au fait c'est qui ganon ?");
            _wiseLine.Add("Cette planete est casse-pied pour parler. On peux pas utiliser d accents et d aspostrophes ! ");

            int Numberlign = RandomNumber(_wiseLine.Count);

            return _wiseLine[Numberlign];
        }

        public string ChooseSentenceForChronicler()
        {
            _chroniclerLine.Add("Bonjour chere scientifique ! Que puis-je pour vous aujourd hui ?");
            _chroniclerLine.Add("");
            _chroniclerLine.Add("");

            string toSay = _chroniclerLine[_cursor];

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
