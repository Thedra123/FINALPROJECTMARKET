using System;
using static System.Formats.Asn1.AsnWriter;

namespace MarketProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();
            Menu menu = new Menu(store);
            menu.MainMenu();
        }
    }
}
