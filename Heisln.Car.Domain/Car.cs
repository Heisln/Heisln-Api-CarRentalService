
using System;

using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Domain
{
    public class Car
    {
        public readonly Guid Id;

        public readonly string Brand;

        public readonly string Name;

        public readonly int Horsepower;

        public readonly double Consumption;

        public double Priceperday { get; private set; }

        public Car(Guid id, string brand, string name, int horsepower, double consumption, double priceperday)
        {
            Id = id;
            Brand = brand;
            Name = name;
            Horsepower = horsepower;
            Consumption = consumption;
            Priceperday = priceperday;
        }
    }
}
