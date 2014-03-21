using System;
using ShingleInterfaces;

namespace Shingles.Shingle.Factory
{
    public class ShingleFactory
    {
        public IShingleResolver GetShingleResolver(bool isShingle, bool isSuperShingle, bool isMegaShingle)
        {
            if (isShingle)
                return new ShingleResolver();

            if (isSuperShingle)
                return new SuperShingleResolver();

            if (isMegaShingle)
                return new MegaShingleResolver();

            throw new Exception("ShingleFactory: class not found");
        }
    }

}
