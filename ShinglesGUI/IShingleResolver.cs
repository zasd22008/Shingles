namespace Shingles
{
    public interface IShingleResolver
    {
        double Calculate(string firstText, string secondText, int shingleSize);
    }

    //public interface IShingleFactory
    //{
    //    IShingleResolver GetSimpleShingleResolver();
    //}
}
