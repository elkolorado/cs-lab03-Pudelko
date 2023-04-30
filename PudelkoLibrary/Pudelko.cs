using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IEnumerable, IFormattable
    {
        private readonly double[] _wymiary;
        public double A { get => Math.Round(_a, 3); }
        public double B { get => Math.Round(_b, 3); }
        public double C { get => Math.Round(_c, 3); }
        public enum UnitOfMeasure { milimeter, centimeter, meter }
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public Pudelko(double a, double b, double c) : this(a, b, c, UnitOfMeasure.meter) => _wymiary = new double[] { a, b, c };

        public Pudelko(double a, double b) : this(a, b, null) { }

        public Pudelko(double a, double b, UnitOfMeasure unit) : this(a, b, null, unit) { }

        public Pudelko(double a) : this(a, null, null) { }

        public Pudelko(double a, UnitOfMeasure unit) : this(a, null, null, unit) { }

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            double defaultDim = unit == UnitOfMeasure.milimeter ? 100 : unit == UnitOfMeasure.centimeter ? 10 : 0.1;
            _a = a ?? defaultDim;
            _b = b ?? defaultDim;
            _c = c ?? defaultDim;

            if (_a <= 0 || _b <= 0 || _c <= 0) throw new ArgumentOutOfRangeException();

            double maxDimensionInMeters = 10.0;
            if (unit == UnitOfMeasure.milimeter)
            {
                _a = Math.Truncate(_a) / 1000;
                _b = Math.Truncate(_b) / 1000;
                _c = Math.Truncate(_c) / 1000;
            }
            else if (unit == UnitOfMeasure.centimeter)
            {
                _a = Math.Truncate(_a * 10) / 1000;
                _b = Math.Truncate(_b * 10) / 1000;
                _c = Math.Truncate(_c * 10) / 1000;
            }

            if (_a > maxDimensionInMeters || _b > maxDimensionInMeters || _c > maxDimensionInMeters || _a == 0 || _b == 0 || _c == 0) throw new ArgumentOutOfRangeException();

            if (unit == UnitOfMeasure.meter)
            {
                _a = Math.Truncate(_a * 1000) / 1000;
                _b = Math.Truncate(_b * 1000) / 1000;
                _c = Math.Truncate(_c * 1000) / 1000;
            }
            _wymiary = new double[] { _a, _b, _c };
        }

        public static explicit operator double[](Pudelko p) => new double[] { p.A, p.B, p.C };

        public static implicit operator Pudelko((int A, int B, int C) dimensions) => new Pudelko(dimensions.A, dimensions.B, dimensions.C, UnitOfMeasure.milimeter);

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double a = Math.Max(p1.A, p2.A);
            double b = Math.Max(p1.B, p2.B);
            double c = Math.Max(p1.C, p2.C);

            double x = Math.Max(Math.Max(p1.A, p2.A), Math.Max(p1.B, p2.B));
            double y = Math.Max(Math.Max(p1.B, p2.B), Math.Max(p1.C, p2.C));
            double z = Math.Max(Math.Max(p1.C, p2.C), Math.Max(p1.A, p2.A));

            while (x + y + z > a + b + c)
            {
                double maxDiff = Math.Max(Math.Max(x - a, y - b), z - c);

                if (maxDiff == x - a)
                {
                    x -= maxDiff;
                }
                else if (maxDiff == y - b)
                {
                    y -= maxDiff;
                }
                else
                {
                    z -= maxDiff;
                }
            }

            return new Pudelko(x, y, z);
        }

        public double this[int index]
        {
            get
            {
                if (index == 0) return A;
                if (index == 1) return B;
                if (index == 2) return C;
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the dimensions of the Pudelko.
        /// </summary>
        /// <returns>An enumerator that iterates through the dimensions of the Pudelko.</returns>
        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the dimensions of this Pudelko object.
        /// </summary>
        /// <returns>An enumerator that iterates through the dimensions of this Pudelko object.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Pudelko other = (Pudelko)obj;
            double[] sortedDimensions = _wymiary.OrderBy(d => d).ToArray();
            double[] sortedOtherDimensions = other._wymiary.OrderBy(d => d).ToArray();
            return sortedDimensions.SequenceEqual(sortedOtherDimensions);
        }

        public override int GetHashCode()
        {
            double[] sortedDimensions = _wymiary.OrderBy(d => d).ToArray();
            int hash = 17;
            foreach (double dimension in sortedDimensions) { hash = hash * 23 + dimension.GetHashCode(); }
            return hash;
        }

        public static bool operator ==(Pudelko pudelko1, Pudelko pudelko2)
        {
            if (ReferenceEquals(pudelko1, pudelko2)) return true;
            if (pudelko1 is null || pudelko2 is null) return false;
            return pudelko1.Equals(pudelko2);
        }

        public static bool operator !=(Pudelko pudelko1, Pudelko pudelko2) => !(pudelko1 == pudelko2);

        public bool Equals(Pudelko other)
        {
            if (other == null) return false;
            double[] sortedDimensions = _wymiary.OrderBy(d => d).ToArray();
            double[] sortedOtherDimensions = other._wymiary.OrderBy(d => d).ToArray();
            return sortedDimensions.SequenceEqual(sortedOtherDimensions);
        }

        public double Area => Math.Round(2 * (_wymiary[0] * _wymiary[1] + _wymiary[0] * _wymiary[2] + _wymiary[1] * _wymiary[2]), 6);

        public double Volume => Math.Round(_wymiary[0] * _wymiary[1] * _wymiary[2], 9);

        public override string ToString() => $"{_wymiary[0]:F3} m × {_wymiary[1]:F3} m × {_wymiary[2]:F3} m";


        /// <summary>
        /// Formats the dimensions of the box as a string in the specified format.
        /// </summary>
        /// <param name="format">The format to use. Can be "m", "cm", or "mm".</param>
        /// <param name="formatProvider">An optional IFormatProvider object that provides culture-specific formatting information.</param>
        /// <returns>A string representation of the dimensions of the box in the specified format.</returns>
        /// <exception cref="FormatException">Thrown when an unsupported format is specified.</exception>
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (format == null || format == "m") return ToString();
            else if (format == "cm") return $"{_wymiary[0] * 100:F1} cm × {_wymiary[1] * 100:F1} cm × {_wymiary[2] * 100:F1} cm";
            else if (format == "mm") return $"{_wymiary[0] * 1000:F0} mm × {_wymiary[1] * 1000:F0} mm × {_wymiary[2] * 1000:F0} mm";
            else throw new FormatException();
        }

        public static Pudelko Parse(string input)
        {
            List<double> sizes = new List<double>();
            string[] inputs = input.Split('×');

            foreach (var i in inputs)
            {
                string[] values = i.Trim().Split(' ');
                if (double.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
                {
                    double size = temp;
                    if (values.Length > 1)
                    {
                        switch (values[1])
                        {
                            case "m":
                                size *= 1.0;
                                break;
                            case "cm":
                                size /= 100.0;
                                break;
                            case "mm":
                                size /= 1000.0;
                                break;
                            default:
                                throw new FormatException();
                        }
                    }
                    sizes.Add(size);
                }
            }
            return sizes.Count == 3 ? new Pudelko(sizes[0], sizes[1], sizes[2]) : throw new FormatException();
        }
    }
}

