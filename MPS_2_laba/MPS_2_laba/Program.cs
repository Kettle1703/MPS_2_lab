using System.Text;
using static System.Console;

namespace Local
{
    class Program
    {
        static int i, j, k;  // итераторы
        static int n = 10;  // размер поля
        static int user_input = 2;  // пользовательские вводы
        static int condition = 1;  // состояние
        static string key;  // ключ для словаря
        static StringBuilder output = new StringBuilder();  // более быстрый вывод в консоль
        static char[,] arr = new char[n, 2 * n];  // поле вывода
        static Dictionary<string, double[]> trans = new Dictionary<string, double[]>()
        {
            {"1-1", new double[] { 0.3, 0.6, 0.05, 0.05 } },
            {"1-2", new double[] { 0.05, 0.05, 0.8, 0.1 } },
            {"2-1", new double[] { 0, 0, 1, 0 } },
            {"2-2", new double[] { 0, 0, 1, 0 } }
        };  // таблица переходов

        static async Task Main(string[] args)
        {
            foreach (string key in trans.Keys)  // обновляем значения ключей в словаре
                for (i = 1; i < trans[key].Length; i++)
                    trans[key][i] += trans[key][i - 1];

            //foreach (var pair in trans)
            //    WriteLine($"Ключ: {pair.Key}, Значения: [{string.Join(", ", pair.Value)}]");
            
            // Запуск таймера для обновления поля через определённый интервал времени
            Timer timer = new Timer(Update_standard, null, 0, 500);

            // Запуск асинхронного метода для обработки пользовательского ввода
            await User_input();
        }

        static async Task User_input()
        {
            while (true)
            {
                
                if (KeyAvailable)
                {
                    ConsoleKeyInfo input = ReadKey(intercept: true); // intercept: true - не отображает нажатую клавишу в консоли
                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            user_input = 1;
                            break;
                        case ConsoleKey.D2:
                            user_input = 2;
                            break;
                    }
                }
            }
        }

        static void Update_standard(object no_use)
        {
            key = $"{user_input}-{condition}";  // определили ключ
            // во время обработки поля состояние не может поменяться и пользовательские нажатия не учитываются
            int col;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < 2 * n; j++)
                {
                    col = Role_rv();
                    if (col == 0 || col == 2)
                        arr[i, j] = ' ';
                    else
                        arr[i, j] = '|';
                }
                col = Role_rv();  // смена состояния
                if (col == 0 || col == 1)
                    condition = 1;
                else
                    condition = 2;
            }
                

            output.Clear();
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < 2 * n; j++)
                    output.Append(arr[i, j]);
                output.Append('\n');
            }
            Clear();
            Write(output.ToString());
        }

        static int Role_rv()
        {
            // возвращает номер столбика, который выпадет после рола случайной величины
            Random rnd = new Random();
            double ran_var;
            ran_var = Math.Round(rnd.NextDouble(), 2);
            for (k = 0; k < trans[key].Length; k++)
            {
                if(ran_var <= trans[key][k])
                    return k;
            }
            return trans[key].Length-1;  // не должны доходить
        }

        
    }
}
