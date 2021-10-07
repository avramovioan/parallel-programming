using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WordAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = "prikazki.txt";
            SingleThreadWordAnalyzer stwa = new SingleThreadWordAnalyzer();
            MultiTreadWordAnalyzer mtwa = new MultiTreadWordAnalyzer();

            var watch = Stopwatch.StartNew();
            stwa.Invoke(path);
            watch.Stop();

            Console.WriteLine("------------------------");
            Console.WriteLine("Elapsed time: " + watch.ElapsedMilliseconds);
            Console.WriteLine("------------------------");



            var watch2 = Stopwatch.StartNew();
            mtwa.InvokeAsync(path);
            watch2.Stop();

            Console.WriteLine("------------------------");
            Console.WriteLine("Elapsed time: " + watch2.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}

/* ---------------------------OUTPUT-------------------------------
 * Number of words: 688510
 * Shortest Word: и
 * Longest Word: Проклетияткомшийскисивкотакпакседовлече
 * Average Word length: 4
 * Five Most Common Words: -,да,се,и,на
 * Five Least Common Words: жизненоважни,коткоотглеждането,разпознаваме,менте,безспорна
 * ------------------------ 
 * Elapsed time: 446  
 * ------------------------ 
 * The encoding used was System.Text.UTF8Encoding+UTF8EncodingSealed.  
 * Number of words: 688510                                            
 * Average Word length: 4                                            
 * Longest Word: Проклетияткомшийскисивкотакпакседовлече    
 * Shortest Word: и                                        
 * Five Most Common Words: -,да,се,и,на 
 * Five Least Common Words: жизненоважни,коткоотглеждането,разпознаваме,менте,безспорна  
 * ------------------------                                                   
 * Elapsed time: 394       
 * 
 */
