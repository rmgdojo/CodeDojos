namespace DiningPhilosophers
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] philosopherNames = new string[] {
                "Immanuel Kant",
                "Martin Heidegger",
                "David Hume",
                "Georg Wilhelm Friedrich Hegel",
                "Ludwig Wittgenstein",
                "Karl Wilhelm Friedrich Schlegel",
                "Friedrich Nietzche",
                "Socrates",
                "John Stuart Mill",
                "Plato",
                "Aristotle",
                "Thomas Hobbes",
                "Rene Descartes"
            };

            Table table = new Table(4, philosopherNames.Length, philosopherNames);
            table.BeginDinner();

            Console.ReadLine();
        }
    }
}