using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shingles
{
    public class ShingleResolver:IShingleResolver
    {
        public const int hash_count = 84;

        private Random _rand;
        private IEnumerable<Crc32> hash_functions;

        public ShingleResolver()
        {
            _rand = new Random(Math.Abs((int)DateTime.Now.Ticks));
        }

        public double Calculate(string firstText, string secondText, int shingleSize)
        {
            hash_functions = GenerateHashFunctions().ToList();

            var hashes1 = GenerateHashs(Canonize(firstText), shingleSize).ToList();
            var hashes2 = GenerateHashs(Canonize(secondText), shingleSize).ToList();

            return compare(hashes1, hashes2);
        }

        private IEnumerable<Crc32> GenerateHashFunctions()
        {
            for (int i = 0; i < hash_count; i++)
                yield return new Crc32((uint) _rand.Next(), (uint) _rand.Next());
        }

        private IList<string> Canonize(string text)
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

        private IEnumerable<string> GenerateHashs(IList<string> words, int shingleSize)
        {
            var length = words.Count - shingleSize + 1;

            if (length <= 0)
                throw new InSufficientTextLengthException("Передан текст недостаточной длины для размера шингла");

            var shingles = new string[length];

            for (int i = 0; i < shingles.Length; i++)
                shingles[i] = String.Join(" ", getSubArray(words, i, shingleSize));

            return hash_functions.Select(hashFunc => shingles.Select(s => GetHash(s, hashFunc)).Min());
        }

        private string GetHash(string str, HashAlgorithm hash_func)
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

        private double compare(IEnumerable<string> shingles1, IEnumerable<string> shingles2)
        {
            int sameCount = shingles1.Count(shingles2.Contains);

            return sameCount * 2/(double)(shingles1.Count() + shingles2.Count());
        }
    }
}
