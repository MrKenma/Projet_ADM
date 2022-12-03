using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetMath {
    class Couts {
        public int ClientsPrioritaires = 0;
        public int ClientsOrdinaires = 0;
        public int StationsOrdinairesOccupées = 0;
        public int StationsPrioritairesOccupées = 0;
        public int StationsInoccupées = 0;
        public int PrioritairesDéchus = 0;

        public Couts() { }
    }
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
                // c : si c = 0, calculs rapides mais période courte => à éviter
                if (c == 0) {
                    Console.WriteLine("ATTENTION : c = 0 -> les calculs seront rapides mais la période courte");
                }
                // a : rejeter a = 0 et a = 1, a devrait être au moins plus grand que 2
                if (a < 2) {
                    Console.WriteLine("ATTENTION : a est plus petit que 2 (à rejeter)");
                }
                return VerificationHullDobell(X0, a, c, m);
            }
        }

        static void TestDesCourses(int[] nombres) {
            // 6 étapes :
            // Etape 1 : poser une hypothèse H0 (suite générée acceptable ou non)
            // Etape 2 : fixer le niveau d'incertitude alpha (en général alpha = 5%)
            // Etape 3 : tableau recensé des fréquences observées et des fréquences théoriques + calcul de la statistique observable X2v
            // Etape 4 : vérifier les contraintes à respecter pour chaque test et, le cas échéant, retourner à l'étape 3 en effectuant les regroupements
            // Etape 5 : établir la zone de non-rejet en fonction du nombre de degrés de liberté
            // Etape 6 : prendre une décision, rejet ou non-rejet de l'hypothèse
        }

        // Partie 2 : implémentation du DA
        static int NbStationsOptimal(int nbStationsMin, int nbStationsMax, int tempsSimulation, int X0, int a, int c, int m) {
            Couts[] couts = new Couts[nbStationsMax - nbStationsMin + 1];

            for (int nbStations = nbStationsMin; nbStations <= nbStationsMax; nbStations++) {

            }

            return 1;
        }

        static void Main(string[] args) {
            int X0 = 19;  
            int a = 261;
            int c = 7;
            int m = 13000;
            bool nombresValides;

            nombresValides = VerificationValeurs(X0, a, c, m);
            if (!nombresValides) {
                Console.WriteLine("Les nombres ne sont pas valides");
            } else {
                Console.WriteLine("Les nombres sont valides");
                int[] nombresAleatoires = new int[m];
                int X1;

                // éviter de placer les valeurs dans un tableau, ça va utiliser beaucoup de place mémoire
                nombresAleatoires[0] = X0;
                for (int i = 1; i < m; i++) {
                    X1 = (a * X0 + c) % m;
                    nombresAleatoires[i] = X1;
                    X0 = X1;
                }
                // Vérifier si la suite est assez longue pour nos tests => Voir DA
                // Si non : 600 * (1 + 9 + 1 + (9 * 1 + 1)) = 600 * 21 = 12600 
                TestDesCourses(nombresAleatoires);
            }
            Console.ReadLine();
        }
    }
}
