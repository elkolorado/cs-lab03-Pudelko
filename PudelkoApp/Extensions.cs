using PudelkoLibrary;

namespace PudelkoApp
{
    public static class PudelkoExtensions
    {
        public static Pudelko Compress(this Pudelko pudelko)
        {
            double volume = pudelko.Volume;
            double edge = Math.Pow(volume, 1.0 / 3.0);

            return new Pudelko(edge, edge, edge);
        }
    }

}