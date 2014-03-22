using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ShingleInterfaces;
using Shingles.Errors;
using Shingles.HashFunctions;

namespace Shingles.Shingle
{
    public class ShingleResolver:IShingleResolver
    {
        public const int hash_count = 84;

        private Random _rand;
        protected IEnumerable<Crc32> hash_functions;

        public ShingleResolver()
        {
            _rand = new Random(Math.Abs((int)DateTime.Now.Ticks));

            hash_functions = GenerateHashFunctions().ToList();        
        }

        public virtual double CalculateShingles(string firstText, string secondText, int shingleSize)
        {
            var hashes2 = generateHashes(secondText, shingleSize);
            var hashes1 = generateHashes(firstText, shingleSize);

            return compare(hashes1, hashes2);
        }

        protected List<string> generateHashes(string str, int shingleSize)
        {
            return GenerateHashs(Canonize(str), shingleSize).ToList();
        }

        //public double CalculateMegaShingles(string firstText, string secondText, int shingleSize)
        //{
        //    hash_functions = GenerateHashFunctions().ToList();

        //    var hashes1 = String.Join(" ", GenerateHashs(Canonize(firstText), shingleSize).OrderBy(s => s).ToList());
        //    var hashes2 = String.Join(" ", GenerateHashs(Canonize(secondText), shingleSize).OrderBy(s => s).ToList());

        //    return CalculateShingles(hashes1, hashes2, shingleSize);
        //}

        protected IEnumerable<Crc32> GenerateHashFunctions()
        {
            for (int i = 0; i < hash_count; i++)
                yield return new Crc32((uint) _rand.Next(), (uint) _rand.Next());
        }

        protected IList<string> Canonize(string text)
        {
            var words = text.Split(' ', '\t', '\r', '\n').Select(w => w.Trim(' ', '\t')).Where(w => !String.IsNullOrWhiteSpace(w));

            var stopSymbols = new[] {".", ",", "!", "?", ":", ";", "-", "\n", "\r", "(", ")"};

            var stopWords = new[]
            {
                "это", "как", "так", "и", "в", "над",
                "к", "до", "не", "на", "но", "за",
                "то", "с", "ли",
                "а", "во", "от",
                "со", "для", "о",
                "же", "ну", "вы",
                "бы", "что", "кто",
                "он", "она"
            };

            return words.Where(w => !stopWords.Contains(w) && !stopSymbols.Contains(w)).ToList();
        }

        protected IEnumerable<string> GenerateHashs(IList<string> words, int shingleSize)
        {
            var length = words.Count - shingleSize + 1;

            if (length <= 0)
                throw new InSufficientTextLengthException("Передан текст недостаточной длины для размера шингла");

            var shingles = new string[length];

            for (int i = 0; i < shingles.Length; i++)
                shingles[i] = String.Join(" ", getSubArray(words, i, shingleSize));

            var min_hashs = new List<string>();

            var file = File.CreateText(@"shingles.txt");
            foreach (var hashFunc in hash_functions)
            {
                var hashs = shingles.Select(s => GetHash(s, hashFunc)).ToList();

                write_to_file(file, hashs, hashs.IndexOf(hashs.Min()));
                min_hashs.Add(hashs.Min());
            }

            file.Flush();
            file.Close();

            return min_hashs;
        }

        private void write_to_file(StreamWriter file, List<string> hashs, int minHashIndex)
        {
            file.Write(minHashIndex);
            file.Write("\t\t");
            
            for (int i = 0; i < hashs.Count(); i++)
            {
                file.Write(i == minHashIndex ? hashs[i].ToUpper() : hashs[i]);
                file.Write("\t");
            }   

            file.WriteLine();
        }

        protected string GetHash(string str, HashAlgorithm hash_func)
        {
            var data = hash_func.ComputeHash(Encoding.Default.GetBytes(str));
 
            var builder = new StringBuilder();
 
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private IEnumerable<string> getSubArray(IList<string> words, int startIndex, int size)
        {
            var selected = new string[size];

            for (int i = 0; i < size; i++)
                selected[i] = words[i + startIndex];

            return selected;
        }

        protected double compare(IEnumerable<string> shingles1, IEnumerable<string> shingles2)
        {
            int sameCount = shingles1.Count(shingles2.Contains);

            return sameCount * 2/(double)(shingles1.Count() + shingles2.Count());
        }
    }
}
