using System;

namespace Quest{
    public class testClasses{
        public static void Main(string[] args) {
            System.Console.WriteLine("I love C#!");

            Quest.AmourCard myCard = new AmourCard("Amour", "Thiccness", 4);
            myCard.printCard();
        }
    }
}