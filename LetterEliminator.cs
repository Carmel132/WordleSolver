using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace WordleCheat
{
    class LetterEliminator
    {
        Dictionary<char, double> Letters;
        List<string> Words;
        bool weighted = true; // weighted letters work better sometimes, but it's still technically luck

        public LetterEliminator()
        {
            this.Letters = new Dictionary<char, double> { { 'a', 8.9 }, { 'b', 2.5 }, { 'c', 3.3 }, { 'd', 3.9 }, { 'e', 10.3 }, { 'f', 1.8 }, { 'g', 2.5 }, { 'h', 2.7 }, { 'i', 5.9 }, { 'j', 0.4 }, { 'k', 2.1 }, { 'l', 5.5 }, { 'm', 3d }, { 'n', 4.5 }, { 'o', 6.7 }, { 'p', 3.1 }, { 'q', 0.2 }, { 'r', 6.5 }, { 's', 10.4 }, { 't', 5.2 }, { 'u', 3.8 }, { 'v', 1.1 }, { 'w', 1.5 }, { 'x', 0.5 }, { 'y', 3.1 }, { 'z', 0.6 } };
            this.Words = new List<string>(File.ReadAllLines(@"C:\Users\User\source\repos\WordleCheat\Takes.txt"));
        }

        public void Update(Information data)
        {
            for (int i = 0; i < this.Words.Count; i++)
            {
                if (!data.Works(this.Words[i])) { this.Words.Remove(this.Words[i]); }
            }
        }
        
        public double Score(string word)
        {
            double retval = 0;
            List<char> used = new List<char>();
            foreach (char c in word)
            {
                if (this.Letters.ContainsKey(c))
                {
                    if (!used.Contains(c))
                    {
                        if (weighted)
                        {
                            retval += this.Letters[c];
                        }
                        else
                        {
                            retval++;
                        }
                        used.Add(c);
                    }
                }
            }
            return retval;
        }
        
        public void Weights()
        {
            foreach (char key in this.Letters.Keys)
            {
                Console.WriteLine(key + " : " + Math.Round(this.Letters[key], 4) + " | ");
            }
            Console.WriteLine();
        }

        public void Count()
        {
            for (int i = 0; i < this.Letters.Keys.Count; i++)
            {
                int count = 0;
                foreach (string word in this.Words)
                {
                    count += word.Count(func => func == this.Letters.Keys.ToArray()[i]);
                }
                this.Letters[this.Letters.Keys.ToArray()[i]] = (double)count / (double)this.Words.Count * 100f;
            }
        }

        public string Choose()
        {
            this.Count();
            string retval = this.Words[0];
            foreach (string word in this.Words)
            {
                if (this.Score(word) > this.Score(retval)) { retval = word; }
            }
            foreach (char letter in retval)
            {
                this.Letters.Remove(letter);
            }
            return retval;
        }
    }
}
