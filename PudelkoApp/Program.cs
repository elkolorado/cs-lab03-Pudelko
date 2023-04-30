using PudelkoLibrary;

namespace PudelkoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var box = new Pudelko();

            //toString
            Console.WriteLine("\n==toString(format)==");
            box = new Pudelko(1, 2, 5);
            Console.WriteLine($"Pudełko wymiary: {box.ToString("m")}");
            Console.WriteLine($"Pudełko wymiary: {box.ToString("cm")}");
            Console.WriteLine($"Pudełko wymiary: {box.ToString("mm")}");

            //volume/area
            Console.WriteLine("\n==Volume/Area==");
            box = new Pudelko(1, 2, 5);
            Console.WriteLine($"Pudełko wymiary: {box.ToString()}");
            Console.WriteLine($"Objętość pudełka: {box.Volume}m^3");
            Console.WriteLine($"Pole pudełka: {box.Area}m^2");

            //equals
            Console.WriteLine("\n==Equals==");
            Pudelko _box1 = new Pudelko(1, 2, 5);
            Console.WriteLine($"Pudełko p1: {_box1.ToString("mm")}");
            Pudelko _box2 = new Pudelko(2, 1, 5);
            Console.WriteLine($"Pudełko p2: {_box2.ToString("mm")}");
            Console.WriteLine($"Czy pudełka są równe?: {_box1.Equals(_box2)}");
            Console.WriteLine($"Czy pudełka są równe (==) ?: {_box1 == _box2}");
            Console.WriteLine($"Czy pudełka są nie są równe (!=) ?: {_box1 != _box2}");

            //operator +
            Console.WriteLine("\n==Operator +==");

            Pudelko p1 = new Pudelko(20, 30, 40, Pudelko.UnitOfMeasure.centimeter);
            Pudelko p2 = new Pudelko(30, 50, 60, Pudelko.UnitOfMeasure.centimeter);
            Pudelko p3 = p1 + p2;
            Console.WriteLine($"Pudełko p1: {p1.ToString("cm")}");
            Console.WriteLine($"Pudełko p2: {p2.ToString("cm")}");
            Console.WriteLine($"Wynik dodawania: {p3.ToString("cm")}");

            // przykład konwersji jawną
            Console.WriteLine("\n==Konwersja jawna==");
            box = new Pudelko(1, 2, 3);
            Console.WriteLine($"Pudełko wymiary: {box.ToString()}");
            double[] arr = (double[])box;
            Console.WriteLine($"Długości krawędzi pudełka: {arr[0]} m, {arr[1]} m, {arr[2]} m");

            // przykład konwersji niejawną
            Console.WriteLine("\n==Konwersja niejawna==");
            (int a, int b, int c) tuple = (1000, 2000, 3000);
            box = tuple;
            Console.WriteLine($"Pudełko wymiary: {box.ToString("mm")}");

            //przeglądanie długości krawędzi - indekser
            Console.WriteLine("\n==Przeglądanie - indekser ==");
            box = new Pudelko(2.5, 3, 4);
            Console.WriteLine($"Pudełko wymiary: {box.ToString()}");
            Console.WriteLine($"Długość krawędzi A: {box[0]}");
            Console.WriteLine($"Długość krawędzi B: {box[1]}");
            Console.WriteLine($"Długość krawędzi C: {box[2]}");

            //Przeglądanie długości krawędzi - pętla foreach
            Console.WriteLine("\n==Przeglądanie - pętla ==");
            box = new Pudelko(1,2,3);
            Console.WriteLine($"Pudełko wymiary: {box.ToString()}");
            foreach (var item in box) { Console.WriteLine($"Bok: {item}"); }

            //parse
            Console.WriteLine("\n==Parsowanie==");
            box = new Pudelko(2.5, 9.321, 0.1);
            Console.WriteLine($"Wymiary: 2.5, 9.321, 0.1");
            Console.WriteLine("Tekst do zparsowania: 2.500 m × 9.321 m × 0.100 m");
            Console.WriteLine($"Wynik zparsowania: {new Pudelko(2.5, 9.321, 0.1) == Pudelko.Parse("2.500 m × 9.321 m × 0.100 m")}");
           
            //kompresja
            Console.WriteLine("\n==Kompresja==");
            box = new Pudelko(2, 3, 4);
            Console.WriteLine($"Pudełko wymiary: {box.ToString()}");
            Console.WriteLine($"Objetość: {box.Volume}");

            var compressBox = box.Compress();
            Console.WriteLine($"Pudełko skompresowane: {compressBox.ToString()}");
            Console.WriteLine($"Objetość pudełka po kompresji: {compressBox.Volume}");
            Console.WriteLine($"Wynik kompresji: {box.Volume == compressBox.Volume}");

            // utworzenie listy pudełek
            Console.WriteLine("\n==Utworzenie listy pudełek==");
            var pudelka = new List<Pudelko>
            {
                new Pudelko(.5, .5, .5),
                new Pudelko(.2, .4),
                new Pudelko(100, 500, 300, Pudelko.UnitOfMeasure.milimeter),
                new Pudelko(5000, 5000, 5000, Pudelko.UnitOfMeasure.milimeter),
                new Pudelko(8, Pudelko.UnitOfMeasure.centimeter),
                new Pudelko(1, 1, Pudelko.UnitOfMeasure.milimeter)
            };

            foreach (var p in pudelka)
            {
                Console.WriteLine(p);
            }

            Comparison<Pudelko> comparator = (p1, p2) =>
            {
                if (p1.Volume < p2.Volume) return -1;
                if (p1.Volume > p2.Volume) return 1;
                if (p1.Area < p2.Area) return -1;
                if (p1.Area > p2.Area) return 1;
                if (p1.A + p1.B + p1.C < p2.A + p2.B + p2.C) return -1;
                if (p1.A + p1.B + p1.C > p2.A + p2.B + p2.C) return 1;
                return 0;
            };

            pudelka.Sort(comparator);
            Console.WriteLine("\nPosortowana lista pudełek:");
            foreach (var p in pudelka)
            {
                Console.WriteLine(p);
            }

        }
    }
}