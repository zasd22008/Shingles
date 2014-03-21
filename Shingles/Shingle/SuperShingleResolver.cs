using System;
using System.Collections.Generic;
using System.Linq;

namespace Shingles.Shingle
{
    public class SuperShingleResolver : ShingleResolver
    {
        public const int superShinglesCount = 6;

        public override double CalculateShingles(string firstText, string secondText, int shingleSize)
        {
            var hashes1 = GetSuperShingles(firstText, shingleSize);
            var hashes2 = GetSuperShingles(secondText, shingleSize);

            return compare(hashes1, hashes2);
        }

        protected IEnumerable<string> GetSuperShingles(string text, int shingleSize)
        {
            var shingles = generateHashes(text, shingleSize).OrderBy(s => s).ToList();
            // разбиваем на n групп и от каждой берем хеш
            var superHashs = new List<string>();

            for (int i = 0; i < hash_count; i += superShinglesCount)
            {
                superHashs.Add(GetHash(String.Join(" ", shingles.Skip(i).Take(superShinglesCount)), hash_functions.First()));
            }

            return superHashs;
        }
    }
}