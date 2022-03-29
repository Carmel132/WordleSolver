using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 
namespace WordleSolver
{
    class Information
    {

        public List<List<Tuple<char, char>>> Data; // list of rows, tuple goes <letter, color>


        public Information()
        {
            this.Data = new List<List<Tuple<char, char>>>();
        }


        public bool Works(string word)
        {
            //each row
            for (int i = 0; i < this.Data.Count; i++)
            {
                //each letter
                for (int j = 0; j < this.Data[i].Count; j++)
                {
                    switch (this.Data[i][j].Item2)
                    {
                        case 'w':
                            if (word.Contains(this.Data[i][j].Item1)) { return false; }
                            break;

                        case 'y':
                            //reading this is a nightmare 
                            if (!(word.Count(func => func == this.Data[i][j].Item1) >= this.Data[i].Count(func => func.Item1 == this.Data[i][j].Item1 && (func.Item2 == 'y' || func.Item2 == 'g')))) { return false; }
                            break;

                        case 'g':
                            if (word[j] != this.Data[i][j].Item1) { return false; }
                            break;
                    }
                }
            }
            return true;
        }

        public void Update(Tuple<string, string> data)
        {
            List<Tuple<char, char>> row = new List<Tuple<char, char>>();
            for (int i = 0; i < data.Item1.Length; i++)
            {
                row.Add(new Tuple<char, char>(data.Item1[i], data.Item2[i]));
            }
            this.Data.Add(row);
        }

        public string CharToColor(char color) // probably doesn't work but thought I'd include this anyways
        {
            switch (color)
            {
                case 'g':
                    return "🟩";
                case 'y':
                    return "🟨";
                case 'w':
                    return "⬜";
            }
            return "Wrong input color!";
        }

        public void Print()
        {
            foreach (List<Tuple<char, char>> row in this.Data)
            {
                foreach (Tuple<char, char> letter in row)
                {
                    Console.Write(char.ToUpper(letter.Item1) + " : " + char.ToUpper(letter.Item2) + " | ");
                    
                }

                Console.WriteLine("\n_______________________________________");
            }
        }
    }


    class WordleClass
    {
        public List<string> Words;
        public Information Data;
        public LetterEliminator LE;

        public WordleClass()
        {
            this.Words = new List<string>(File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Answers.txt")); // Make sure path fits your directory
            this.Data = new Information();
            this.LE = new LetterEliminator();
        }

        public Tuple<string, string> GetInput(string last)
        {
            Console.WriteLine("Enter colors (in order, w is white, y is yellow, and g is green)");
            string colors = Console.ReadLine();
            return new Tuple<string, string>(last, colors);

        }

        public string UseWord(string last)
        {

            this.Data.Update(this.GetInput(last));
            //remove words that dont work
            var temp = this.Words.ToList();
            foreach (string word in temp)
            {
                if (Data.Works(word)) { continue; }
                else { this.Words.Remove(word); }
            }
            LE.Update(this.Data);
            Console.WriteLine("Possible Words: " + Convert.ToString(this.Words.Count));
            this.Data.Print();
            this.LE.Weights();
            if (this.Words.Count > 1)
            {
                return LE.Choose();
            }
            else
            {
                return Words[0];
            }
        }



        public void Run()
        {
            string last = this.LE.Choose();
            Console.WriteLine(last);
            for (int i = 0; i < 5 && this.Words.Count != 1; i++)
            {
                last = UseWord(last);
                Console.WriteLine(last);
            }
            if (this.Words.Count == 0)
            {
                Console.WriteLine("No words!");
            }
            else if (this.Words.Count > 1)
            {
                Console.WriteLine("Could not find exact word with current data!");
                Console.WriteLine("Words remaining:");
                foreach (string word in this.Words)
                {
                    Console.WriteLine(word);
                }
            }
        }
    }
}
