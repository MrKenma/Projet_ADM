using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetMath {
    #region classeCouts
    class Couts {
        public double ClientsPrioritaires = 0;
        public double ClientsOrdinaires = 0;
        public double StationsOrdinairesOccupées = 0;
        public double StationsPrioritairesOccupées = 0;
        public double StationsInoccupées = 0;
        public double PrioritairesDéchus = 0;
        public int nbStations;

        public Couts(int nbStations) {
            this.nbStations = nbStations;
        }
    }
    #endregion 
    class Program {
        #region Partie 1 : Génération d'une suite de nombres aléatoires
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

        static bool EstPremier(int nb) {
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
                if (m % i == 0 && EstPremier(i)) {
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
        #endregion
        #region Partie 1 : Test des courses
        static void CreationTabSaut(int[] nombres, int[] tabSaut) {

            int i = 0;
            int nbSaut = 0;
            int saut;
            int val;
            int cptIgnored = 0;

            while (i < (nombres.Length - 1)) {
                saut = 1;
                val = nombres[i];


                while (i < (nombres.Length - 1) && val < nombres[i + 1]) {
                    saut++;
                    i++;
                    val = nombres[i];
                }
                cptIgnored++;
                i += 2;
                tabSaut[nbSaut] = saut;
                nbSaut++;
            }
            Console.WriteLine("cptIgnored : " + cptIgnored);
        }

        static double CalculFactorielle(int a) {
            double fact = 1;
            for (int x = 1; x <= a; x++) {
                fact *= x;
            }

            return fact;
        }

        static void TestDesCourses(int[] nombres, int[] tabSaut) {
            // 6 étapes :
            // Etape 1 : poser une hypothèse H0 (suite générée acceptable ou non)
            // Etape 2 : fixer le niveau d'incertitude alpha (en général alpha = 5%)
            // Etape 3 : tableau recensé des fréquences observées et des fréquences théoriques + calcul de la statistique observable X2v
            // Etape 4 : vérifier les contraintes à respecter pour chaque test et, le cas échéant, retourner à l'étape 3 en effectuant les regroupements
            // Etape 5 : établir la zone de non-rejet en fonction du nombre de degrés de liberté
            // Etape 6 : prendre une décision, rejet ou non-rejet de l'hypothèse

            CreationTabSaut(nombres, tabSaut);

            int max = 0;

            for (int i = 0; i < tabSaut.Length; i++) {

                if (max < tabSaut[i]) {
                    max = tabSaut[i];
                }
            }


            int[] tabCourses = new int[max];
            int valeur = 0;

            for (int i = 0; i < tabSaut.Length; i++) {

                if (tabSaut[i] > 0) {
                    valeur = tabSaut[i];

                    tabCourses[valeur - 1]++;
                }
            }

            double pi;
            double fact;
            for (int i = 0; i < max; i++) {
                Console.WriteLine(i + 1);
                Console.WriteLine(tabCourses[i]);
                fact = CalculFactorielle(i + 2);
                pi = (i + 1) / fact;
                Console.WriteLine(pi);
                Console.WriteLine("\n");

            }
        }

        #endregion
        #region Partie 2 : Calcul du nombre de stations optimal
        // Partie 2 : implémentation du DA
        static void InitTab(int[] tab) {
            for (int i = 0; i < tab.Length; i++) {
                tab[i] = 0;
            }
        }

        static int NbArriveesGenerees(int x0, int a, int c, int m, out int nbArrivees) {
            int x1 = (a * x0 + c) % m;
            double u1 = (double)x1 / m;

            if (u1 < 0.1827)
                nbArrivees = 0;
            else if (u1 < 0.4932)
                nbArrivees = 1;
            else if (u1 < 0.7572)
                nbArrivees = 2;
            else if (u1 < 0.9068)
                nbArrivees = 3;
            else if (u1 < 0.9704)
                nbArrivees = 4;
            else if (u1 < 0.9920)
                nbArrivees = 5;
            else if (u1 < 0.9981)
                nbArrivees = 6;
            else if (u1 < 0.9996)
                nbArrivees = 7;
            else if (u1 < 0.9999)
                nbArrivees = 8;
            else
                nbArrivees = 9;

            return x1;
        }

        static int NbClientsPrioritaires(int nbArrivees, int x0, int a, int c, int m, out int nbOrdinaires, out int nbPrioritaires) {
            int x1 = x0;
            double u1 = 0;
            nbOrdinaires = 0;
            nbPrioritaires = 0;

            for (int i = 0; i < nbArrivees; i++) {
                x1 = (a * x0 + c) % m;
                u1 = (double)x1 / m;

                if (u1 < 0.30)
                    nbPrioritaires++;
                else
                    nbOrdinaires++;

                x0 = x1;
            }
            return x1;
        }

        static int DureeGeneree(int x0, int a, int c, int m, int i, int[] stations) {
            int x1 = (a * x0 + c) % m;
            double u1 = (double)x1 / m;

            if (u1 < 18.0 / 59)
                stations[i] = 1;
            else if (u1 < 39.0 / 59)
                stations[i] = 2;
            else if (u1 < 54.0 / 59)
                stations[i] = 3;
            else if (u1 < 57.0 / 59)
                stations[i] = 4;
            else if (u1 < 58.0 / 59)
                stations[i] = 5;
            else
                stations[i] = 6;

            return x1;
        }

        static int NbStationsOptimal(int nbStationsMin, int nbStationsMax, int tempsSimulation, int X0, int a, int c, int m) {
            Couts[] couts = new Couts[nbStationsMax - nbStationsMin + 1];
            int iCouts = 0;
            int X0Init = X0;

            for (int nbStations = nbStationsMin; nbStations <= nbStationsMax; nbStations++) {
                couts[iCouts] = new Couts(nbStations);
                int[] stations = new int[nbStations];
                int fileOrdinaire = 0;
                int filePrioritaire = 0;
                X0 = X0Init;

                //InitTab(stations);

                Console.WriteLine("Nombre de stations : " + nbStations);

                for (int temps = 0; temps < tempsSimulation; temps++) {
                    int nbArrivées, nbOrdinaires, nbPrioritaires, nbPrioritairesDéchus;
                    X0 = NbArriveesGenerees(X0, a, c, m, out nbArrivées);
                    X0 = NbClientsPrioritaires(nbArrivées, X0, a, c, m, out nbOrdinaires, out nbPrioritaires);

                    Console.WriteLine("Temps : " + (temps + 1));
                    Console.WriteLine(nbArrivées);

                    while (filePrioritaire < 5 && nbPrioritaires > 0) {
                        filePrioritaire++;
                        nbPrioritaires--;
                    }
                    nbPrioritairesDéchus = nbPrioritaires;
                    fileOrdinaire += nbOrdinaires + nbPrioritairesDéchus;
                    couts[iCouts].PrioritairesDéchus += 30 * nbPrioritairesDéchus;

                    if (stations[0] == 0) {
                        if (filePrioritaire != 0) {
                            filePrioritaire--;
                            X0 = DureeGeneree(X0, a, c, m, 0, stations);
                            couts[iCouts].ClientsPrioritaires += 1.0 / 60 * stations[0] * 40;
                            couts[iCouts].StationsPrioritairesOccupées += 1.0 / 60 * stations[0] * 75;
                            stations[0]--;
                        } else {
                            couts[iCouts].StationsInoccupées += 1.0 / 60 * 20;
                        }
                    } else {
                        stations[0]--;
                    }

                    for (int iStation = 1; iStation < nbStations; iStation++) {
                        if (stations[iStation] == 0) {
                            if (fileOrdinaire != 0) {
                                fileOrdinaire--;
                                X0 = DureeGeneree(X0, a, c, m, iStation, stations);
                                couts[iCouts].ClientsOrdinaires += 1.0 / 60 * stations[iStation] * 25;
                                couts[iCouts].StationsOrdinairesOccupées += 1.0 / 60 * stations[iStation] * 50;
                                stations[iStation]--;
                            } else if (filePrioritaire != 0) {
                                filePrioritaire--;
                                X0 = DureeGeneree(X0, a, c, m, iStation, stations);
                                couts[iCouts].ClientsPrioritaires += 1.0 / 60 * stations[iStation] * 40;
                                couts[iCouts].StationsOrdinairesOccupées += 1.0 / 60 * stations[iStation] * 50;
                                stations[iStation]--;
                            } else {
                                couts[iCouts].StationsInoccupées += 1.0 / 60 * 20;
                            }
                        } else {
                            stations[iStation]--;
                        }
                    }

                    couts[iCouts].ClientsPrioritaires += 1.0 / 60 * filePrioritaire * 40;
                    couts[iCouts].ClientsOrdinaires += 1.0 / 60 * fileOrdinaire * 25;
                }

                iCouts++;
            }

            double coutOptimal = double.MaxValue;
            int iCoutOptimal = 0;
            int i = 0;
            // Vérification des valeurs
            foreach (Couts cout in couts) {
                double sommeCouts = cout.ClientsOrdinaires + cout.ClientsPrioritaires + cout.PrioritairesDéchus + cout.StationsInoccupées + cout.StationsOrdinairesOccupées + cout.StationsPrioritairesOccupées;
                if (sommeCouts < coutOptimal) {
                    coutOptimal = sommeCouts;
                    iCoutOptimal = i;
                }
                i++;
            }

            return (int)couts[0].ClientsOrdinaires;
        }
        #endregion
        static void Main(string[] args) {
            int X0 = 19;  
            int a = 261;
            int c = 7;
            int m = 13000;
            bool nombresValides = VerificationValeurs(X0, a, c, m);

            if (!nombresValides) {
                Console.WriteLine("Les nombres ne sont pas valides");
            } else {
                Console.WriteLine("Les nombres sont valides");
                int[] nombresAleatoires = new int[m];
                int X1;

                // éviter de placer les valeurs dans un tableau, ça va utiliser beaucoup de place mémoire
                /*
                nombresAleatoires[0] = X0;
                for (int i = 1; i < m; i++) {
                    X1 = (a * X0 + c) % m;
                    nombresAleatoires[i] = X1;
                    X0 = X1;
                }
                */
                // Vérifier si la suite est assez longue pour nos tests => Voir DA
                // Si non : 600 * (1 + 9 + 1 + (9 * 1 + 1)) = 600 * 21 = 12600 
                //TestDesCourses(nombresAleatoires);

                int nbStationsOptimal = NbStationsOptimal(2, 4, 600, X0, a, c, m);
                Console.WriteLine($"Nombre de stations optimal : {nbStationsOptimal}");
            }
            Console.ReadLine();
        }
    }
}
