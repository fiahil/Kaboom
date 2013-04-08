using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Kaboom.Serializer;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    internal class Ladder
    {

        public class LadderEntry
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public bool Default { get; set; }

            public LadderEntry(string name, int score, bool dflt = false)
            {
                Name = name;
                Score = score;
                Default = dflt;
            }
        }

        private List<LadderEntry> ladder_;
        public bool IsDisplay;

        public Ladder()
        {
            ladder_ = new List<LadderEntry>();
            IsDisplay = false;
            //Todo : Load current ladder from xml file
        }


        public List<LadderEntry> GetLadder()
        {
            return this.ladder_;
        }

        private void Sort()
        {
            ladder_.Sort((l1, l2) =>
            {
                if (l1.Score < l2.Score) return 1;
                if (l1.Score > l2.Score) return -1;
                return 0;
            }
                );
        }

        public void AddEntry(int score, string name)
        {
            ladder_.Add(new LadderEntry(name, score));

            var numb = ladder_.Count(ladderEntry => !ladderEntry.Default);

            if (numb > 5)
            {
                var min = int.MaxValue;
                foreach (var ladderEntry in ladder_.Where(ladderEntry => !ladderEntry.Default && ladderEntry.Score < min))
                {
                    min = ladderEntry.Score;
                }
                ladder_.RemoveAll(l => !l.Default && (l.Score <= min));
            }
            Sort();
        }

        public void ResetLadder()
        {
            ladder_.RemoveAll((l) => !(l.Default));
            Sort();
        }


    }
}