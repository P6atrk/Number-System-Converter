using System;

/* Számrendszerek átváltása
 * Kérjünk be egy számot, a számrendszerét majd kérjük be azt,
 * hogy melyik számrendszerbe akarjuk átváltani.
 */
namespace _027.SzámrendszerÁtváltás
{
	class Program
	{
		static int restartedWrong = 0;

		static void Main(string[] args)
		{
			int numberT = 0; // numberO tízesszámrendszerbeli alakja lesz
			string numberConvStr = ""; // A végső, konvertált szám (konvTo num.sys beli) stringes alakja
			string letters = "ABCDEFGHIJ";
			int number = 0, convFr = -1, convTo = -1;
			bool convFrSuccess = false, convToSuccess = false;

			// Adatok bekérése
			Console.Write("Szám: ");
			string numberO = Console.ReadLine().Trim().ToUpper(); // A szám amit át kell kovertálni

			do
			{
				Console.Write("Számrendszere: ");
				convFrSuccess = int.TryParse(Console.ReadLine(), out convFr); // Amelyikből akarjuk konvertálni
				if (!convFrSuccess)
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					ClearCurrentConsoleLine();
			} while (!convFrSuccess);

			do
			{
				Console.Write("Ebbe fog váltani: ");
				convToSuccess = int.TryParse(Console.ReadLine(), out convTo); // Amelyikbe akarjuk konvertálni
				if (!convToSuccess)
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					ClearCurrentConsoleLine();
			} while (!convToSuccess);

			// Hibakeresés
			if (numberO == "")
				Restart();

			if (numberO[0] == '-' || numberO[0] == '0')
			{ // minusz-e, 0ával kezdődik-e, 2 és 20 között van-e a 2 számrendsz.
				Restart();
			}

			for (int i = 0; i < numberO.Length; i++)
			{ // Ez vizsgálja a player input-ot
				if (int.TryParse(numberO[i].ToString(), out number) == false)
				{ // akkor fut le, ha nem szám (hanem bármi más) a numberO[i] karakter
					bool allowedNum = false; // Hamis addig, míg meg nem tudja numberO[i] megengedett betű-e 
					for (int j = 0; j < letters.Length; j++)
					{ // megvizsgálja h benne van e a karakter a használható karakterek közt
						if (numberO[i] == letters[j]) // TODO nem jó, mert ha csak egy nincs benne a számok közül akkor hátna
						{ // ha a karakter a megengedett betűk között van
							allowedNum = true;
							break;
						}
					}
					if (allowedNum == false)
					{ // újraindítja, ha a karakter nem a megengedett betűk közt volt
						Restart();
					}
				}
				else
				{
					if (int.Parse(numberO[i].ToString()) >= convFr)
					{ // van-e nagyobb vagy egyenlő szám a numberO számrendsz.-énél a numberO-ban
						Restart();
					}
				}
			}

			if (convFr < 2 || convFr > 20 || convTo < 2 || convTo > 20)
			{ // a számrendszerek hibásak-e
				Restart();
			}

			// Átváltás
			// Átkonvertál 10es számrendszerből a convTo számrendszerébe
			ConvertToTen(numberO, ref numberT, letters, convFr);

			// 10-es num.sys-ből átkonv a convTo num.sys-be.
			numberConvStr = ConvertTo(numberT, convTo, letters);

			// megfordítja a konvertált stringet miután a 10esből átkonvertáltuk convTo num.sys-be.
			char[] charArr = numberConvStr.ToCharArray();
			Array.Reverse(charArr);
			numberConvStr = new string(charArr);

			// kiíratás
			Console.WriteLine("{0} - Átváltott szám\n{1} - Tízes számrendszerbeli alakja\n\nNyomd meg az Entert az újraindításhoz...", numberConvStr, numberT);

			// vége(?)
			ConsoleKeyInfo cki = Console.ReadKey();
			if (cki.Key == ConsoleKey.Enter)
			{ // újraínditás hiba nélkül
				Console.Clear();
				restartedWrong = 0;
				Main(null);
			}
		}

		public static void ClearCurrentConsoleLine()
		{ // igen, lopott kód https://stackoverflow.com/a/8946847, de csak ez az 1, igen egyszerűbben is megoldhattam volna, igen
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}

		static void ConvertToTen(string numFrom, ref int numTen, string allowedLetters, int convFrom)
		{ // átkonvertál 10es számrendszerbe
			int number;
			int k = 1;
			for (int i = numFrom.Length - 1; i >= 0; i--)
			{
				char p = numFrom[i];
				bool ifInt = Int32.TryParse(p.ToString(), out number);
				if (ifInt)
					numTen += int.Parse(p.ToString()) * k;
				else
					numTen += (allowedLetters.IndexOf(p) + 10) * k;
				k *= convFrom;
			}
		}

		static string ConvertTo(int toTurn, int convTo, string letters)
		{ // Átkonvertál 10es számrendszerből a convTo számrendszerébe
			string gotStr = "";
			do
			{
				if (toTurn % convTo < 10)
					gotStr += toTurn % convTo;
				else
					gotStr += letters[(toTurn % convTo) - 10];
				toTurn /= convTo;
			} while (toTurn != 0);
			return gotStr;
		}

		static void Restart()
		{ // újraindítás ha hibás
			Console.Clear();
			Console.WriteLine("Hiba! Kérlek helyesen írd be az adatokat. {0}x", ++restartedWrong);
			Main(null);
		}
	}
}
