using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetMath {
    class Program {
        // Générer une suite de nombres pseudo-aléatoires à partir de valeurs de X0, a, c et m
        // Vérifier 
        static int PGCD(int a, int b) {
            while (a != b) {
                if (a > b) {
                    a -= b;
                } else {
                    b -= a;
                }
            }
            return a;
        }

        static bool estPremier(int nb) {
            if (nb <= 3) {
                return true;
            } else {
                int racineCarreedeNb = (int)Math.Sqrt(nb);
                for (int i = 2; i <= racineCarreedeNb; i++) {
                    if (nb % i == 0) {
                        return false;
                    }
                }
                return true;
            }
        }

        static bool VerificationHullDobell(int X0, int a, int c, int m) {
            List<int> facteursPremiersDeM = new List<int>();

            // c et m premiers entre eux (pas de diviseurs communs)
            if (PGCD(c, m) != 1) {
                Console.WriteLine("c et m premiers entre eux : KO");
                return false;
            }
            Console.WriteLine("c et m premiers entre eux : OK");

            // Pour tout p, facteur premier de m, on a (a-1) multiple de p
            for (int i = 1; i < m; i++) {
                if (m % i == 0 && estPremier(i)) {
                    facteursPremiersDeM.Add(i);
                }
            }
            foreach (int num in facteursPremiersDeM) {
                if ((a - 1) % num != 0) {
                    Console.WriteLine("Pour tout p, facteur premier de m, on a (a-1) multiple de p : KO");
                    return false;
                }
            }
            Console.WriteLine("Pour tout p, facteur premier de m, on a (a-1) multiple de p : OK");

            // Si m est multiple de 4, alors (a-1) est multiple de 4
            if (m % 4 == 0 && (a - 1) % 4 != 0) {
                Console.WriteLine("Si m est multiple de 4, alors (a-1) est multiple de 4 : KO");
                return false;
            }
            Console.WriteLine("Si m est multiple de 4, alors (a-1) est multiple de 4 : OK");

            return true;
        }

        static bool VerificationValeurs(int X0, int a, int c, int m) {
            // m : de préférence une valeur élevée
            // c : si c = 0, calculs rapides mais période courte => à éviter
            // a : rejeter a = 0 et a = 2, a devrait être au moins plus grand que 2
            if (m <= 0) {
                Console.WriteLine("m > 0 : KO");
                return false;
            } else if (a < 0 || a >= m) {
                Console.WriteLine("0 <= a < m : KO");
                return false;
            } else if (c < 0 || c >= m) {
                Console.WriteLine("0 <= c < m : KO");
                return false;
            } else if (X0 < 0 || X0 >= m) {
                Console.WriteLine("0 <= X0 < m : KO");
                return false;
            } else {
                if (c == 0) {
                    Console.WriteLine("ATTENTION : c = 0 -> les calculs seront rapides mais la période courte");
                }
                if (a < 2) {
                    Console.WriteLine("ATTENTION : a est plus petit que 2 (à rejeter)");
                }
                return VerificationHullDobell(X0, a, c, m);
            }
        }

        static void Main(string[] args) {
            int X0 = 19;
            int a = 22;
            int c = 4;
            int m = 63;
            bool nombresValides;

            nombresValides = VerificationValeurs(X0, a, c, m);
            if (!nombresValides) {
                Console.WriteLine("Les nombres ne sont pas valides");
            } else {
                Console.WriteLine("Les nombres sont valides");
            }
            
            Console.ReadLine();
        }
    }
}
