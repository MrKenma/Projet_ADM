using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetMath {
    #region Définition des classes
    class Couts {
        public double clientsPrioritaires = 0;
        public double clientsOrdinaires = 0;
        public double stationsOrdinairesOccupées = 0;
        public double stationsPrioritairesOccupées = 0;
        public double stationsInoccupées = 0;
        public double prioritairesDéchus = 0;
        public int nbStations;

        public Couts(int nbStations) {
            this.nbStations = nbStations;
        }

        public double CoutsTotaux() {
            return clientsPrioritaires + clientsOrdinaires + stationsOrdinairesOccupées + stationsPrioritairesOccupées + stationsInoccupées + prioritairesDéchus;
        }
    }

    class Station {
        public int tempsRestant;
        public string statusClient;

        public Station () {
            tempsRestant = 0;
            statusClient = "inexistant";
        }

        public override string ToString() {
            return $"Durée restante = {tempsRestant} min, statut client = \"{statusClient}\"";
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
            Console.WriteLine("-------------------- Vérification Hull-Dobell --------------------");
            List<int> facteursPremiersDeM = new List<int>();

            // c et m premiers entre eux (pas de diviseurs communs)
            if (PGCD(c, m) != 1) {
                Console.WriteLine(" c et m premiers entre eux : KO");
                return false;
            }
            Console.WriteLine(" c et m premiers entre eux : OK");

            // Pour tout p, facteur premier de m, on a (a-1) multiple de p
            for (int i = 1; i < m; i++) {
                if (m % i == 0 && EstPremier(i)) {
                    facteursPremiersDeM.Add(i);
                }
            }
            foreach (int num in facteursPremiersDeM) {
                if ((a - 1) % num != 0) {
                    Console.WriteLine(" Pour tout p, facteur premier de m, on a (a-1) multiple de p : KO");
                    return false;
                }
            }
            Console.WriteLine(" Pour tout p, facteur premier de m, on a (a-1) multiple de p : OK");

            // Si m est multiple de 4, alors (a-1) est multiple de 4
            if (m % 4 == 0 && (a - 1) % 4 != 0) {
                Console.WriteLine(" Si m est multiple de 4, alors (a-1) est multiple de 4 : KO");
                return false;
            }
            Console.WriteLine(" Si m est multiple de 4, alors (a-1) est multiple de 4 : OK");

            return true;
        }

        static bool VerificationValeurs(int X0, int a, int c, int m) {
            Console.WriteLine("-------------------- Vérifications des valeurs --------------------");
            // m : de préférence une valeur élevée
            
            if (m <= 0) {
                Console.WriteLine(" m > 0 : KO");
                return false;
            } else if (a < 0 || a >= m) {
                Console.WriteLine(" 0 <= a < m : KO");
                return false;
            } else if (c < 0 || c >= m) {
                Console.WriteLine(" 0 <= c < m : KO");
                return false;
            } else if (X0 < 0 || X0 >= m) {
                Console.WriteLine(" 0 <= X0 < m : KO");
                return false;
            } else {
                Console.WriteLine(" m > 0 : OK\n 0 <= a < m : OK\n 0 <= c < m : OK\n 0 <= X0 < m : OK\n");
                // c : si c = 0, calculs rapides mais période courte => à éviter
                if (c == 0) {
                    Console.WriteLine(" ATTENTION : c = 0 -> les calculs seront rapides mais la période courte\n");
                }
                // a : rejeter a = 0 et a = 1, a devrait être au moins plus grand que 2
                if (a < 2) {
                    Console.WriteLine(" ATTENTION : a est plus petit que 2 (à rejeter)\n");
                }
                return VerificationHullDobell(X0, a, c, m);
            }
        }
        #endregion
        #region Partie 1 : Test des courses
        static int CreationTabSaut(int[] nombres, int[] tabSaut) {
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
            Console.WriteLine($" Nombre de valeurs ignorées : {cptIgnored}\n");

            return cptIgnored;
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

            Console.WriteLine("-------------------- Test des courses --------------------");

            int cptIgnored = CreationTabSaut(nombres, tabSaut);
            int n = nombres.Length - cptIgnored;

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

            // Regroupement
            double[] pi = new double[max];
            for (int i = 0; i < max; i++) {
                double fact = CalculFactorielle(i + 2);
                pi[i] = (i + 1) / fact;
            }

            int iVal = max - 1;
            while (iVal >= 0) {
                if (n * pi[iVal] < 5) {
                    if (iVal == 0) {
                        Console.WriteLine("Regroupement impossible, n < 5 !");
                    } else {
                        tabCourses[iVal - 1] += tabCourses[iVal];
                        pi[iVal - 1] += pi[iVal];
                        tabCourses[iVal] = 0;
                        pi[iVal] = 0;
                    }
                }

                iVal--;
            }

            for (int i = 0; i < max; i++) {
                if (tabCourses[i] != 0) {
                    Console.WriteLine($" Taille du saut : {i + 1}");
                    Console.WriteLine($" Nombre de sauts (ri) : {tabCourses[i]}");
                    Console.WriteLine($" Probabilité (pi) : {pi[i]}\n");
                }
            }
        }

        #endregion
        #region Partie 2 : Calcul du nombre de stations optimal
        // Partie 2 : implémentation du DA
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

        static int NbClientsPrioritairesEtOrdinaires(int nbArrivees, int x0, int a, int c, int m, out int nbOrdinaires, out int nbPrioritaires) {
            double u1;
            int x1 = x0;
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

        static int DureeGeneree(int x0, int a, int c, int m, Station station) {
            int x1 = (a * x0 + c) % m;
            double u1 = (double)x1 / m;

            if (u1 < 18.0 / 59)
                station.tempsRestant = 1;
            else if (u1 < 39.0 / 59)
                station.tempsRestant = 2;
            else if (u1 < 54.0 / 59)
                station.tempsRestant = 3;
            else if (u1 < 57.0 / 59)
                station.tempsRestant = 4;
            else if (u1 < 58.0 / 59)
                station.tempsRestant = 5;
            else
                station.tempsRestant = 6;

            return x1;
        }

        static int NbStationsOptimal(int nbStationsMin, int nbStationsMax, int tempsSimulation, int X0, int a, int c, int m) {
            Console.WriteLine("-------------------- Sorties de la fonction de recherche du nombre optimal de stations --------------------");
            Couts[] couts = new Couts[nbStationsMax - nbStationsMin + 1];
            int iCouts = 0;
            int X0Init = X0;

            for (int nbStations = nbStationsMin; nbStations <= nbStationsMax; nbStations++) {
                Console.WriteLine($"---------- {nbStations} stations ----------");
                couts[iCouts] = new Couts(nbStations);
                Station[] stations = new Station[nbStations];
                int fileOrdinaire = 0;
                int filePrioritaire = 0;
                X0 = X0Init;

                for (int j = 0; j < stations.Length; j++) {
                    stations[j] = new Station();
                }

                for (int temps = 1; temps <= tempsSimulation; temps++) {
                    int nbArrivées, nbOrdinaires, nbPrioritaires, nbPrioritairesDéchus;

                    if (nbStations == nbStationsMin && temps <= 20) {
                        Console.WriteLine($"Temps : {temps} minute(s)");
                        Console.WriteLine($"- Début de minute :");
                        for (int j = 0; j < nbStations; j++) {
                            Console.WriteLine($" Station n°{j + 1} : {stations[j].ToString()}");
                        }
                        Console.WriteLine(" Avant l'arrivée des nouveaux clients :");
                        Console.WriteLine($"  File ordinaire : {fileOrdinaire} client(s) présent(s) dans la file");
                        Console.WriteLine($"  File prioritaire : {filePrioritaire} client(s) présent(s) dans la file");
                    }

                    X0 = NbArriveesGenerees(X0, a, c, m, out nbArrivées);
                    X0 = NbClientsPrioritairesEtOrdinaires(nbArrivées, X0, a, c, m, out nbOrdinaires, out nbPrioritaires);

                    if (nbStations == nbStationsMin && temps <= 20) {
                        Console.WriteLine($" {nbArrivées} nouveau(x) client(s) : {nbOrdinaires} ordinaire(s) et {nbPrioritaires} prioritaire(s)");
                        Console.WriteLine(" Après placement des nouveaux clients :");
                        Console.WriteLine($"  File ordinaire : {fileOrdinaire} client(s) présent(s) dans la file");
                        Console.WriteLine($"  File prioritaire : {filePrioritaire} client(s) présent(s) dans la file");
                    }

                    while (filePrioritaire < 5 && nbPrioritaires > 0) {
                        filePrioritaire++;
                        nbPrioritaires--;
                    }
                    nbPrioritairesDéchus = nbPrioritaires;
                    fileOrdinaire += nbOrdinaires + nbPrioritairesDéchus;
                    couts[iCouts].prioritairesDéchus += 30 * nbPrioritairesDéchus;

                    if (stations[0].tempsRestant == 0) {
                        if (filePrioritaire != 0) {
                            filePrioritaire--;
                            X0 = DureeGeneree(X0, a, c, m, stations[0]);
                            stations[0].statusClient = "prioritaire";
                            if (nbStations == nbStationsMin && temps <= 20) {
                                Console.WriteLine($" Nouveau client {stations[0].statusClient} dans la station n°{1} : durée d'attente de {stations[0].tempsRestant} minute(s)");
                            }
                            couts[iCouts].clientsPrioritaires += 1.0 / 60 * stations[0].tempsRestant * 40;
                            couts[iCouts].stationsPrioritairesOccupées += 1.0 / 60 * stations[0].tempsRestant * 75;
                            stations[0].tempsRestant--;
                        } else {
                            stations[0].statusClient = "inexistant";
                            couts[iCouts].stationsInoccupées += 1.0 / 60 * 20;
                        }
                    } else {
                        stations[0].tempsRestant--;
                    }

                    for (int iStation = 1; iStation < nbStations; iStation++) {
                        if (stations[iStation].tempsRestant == 0) {
                            if (fileOrdinaire != 0) {
                                fileOrdinaire--;
                                X0 = DureeGeneree(X0, a, c, m, stations[iStation]);
                                stations[iStation].statusClient = "ordinaire";
                                if (nbStations == nbStationsMin && temps <= 20) {
                                    Console.WriteLine($" Nouveau client {stations[iStation].statusClient} dans la station n°{iStation + 1} : durée d'attente de {stations[iStation].tempsRestant} minute(s)");
                                }
                                couts[iCouts].clientsOrdinaires += 1.0 / 60 * stations[iStation].tempsRestant * 25;
                                couts[iCouts].stationsOrdinairesOccupées += 1.0 / 60 * stations[iStation].tempsRestant * 50;
                                stations[iStation].tempsRestant--;
                            } else if (filePrioritaire != 0) {
                                filePrioritaire--;
                                X0 = DureeGeneree(X0, a, c, m, stations[iStation]);
                                stations[iStation].statusClient = "prioritaire";
                                if (nbStations == nbStationsMin && temps <= 20) {
                                    Console.WriteLine($" Nouveau client {stations[iStation].statusClient} dans la station n°{iStation + 1} : durée d'attente de {stations[iStation].tempsRestant} minute(s)");
                                }
                                couts[iCouts].clientsPrioritaires += 1.0 / 60 * stations[iStation].tempsRestant * 40;
                                couts[iCouts].stationsOrdinairesOccupées += 1.0 / 60 * stations[iStation].tempsRestant * 50;
                                stations[iStation].tempsRestant--;
                            } else {
                                stations[iStation].statusClient = "inexistant";
                                couts[iCouts].stationsInoccupées += 1.0 / 60 * 20;
                            }
                        } else {
                            stations[iStation].tempsRestant--;
                        }
                    }

                    couts[iCouts].clientsPrioritaires += 1.0 / 60 * filePrioritaire * 40;
                    couts[iCouts].clientsOrdinaires += 1.0 / 60 * fileOrdinaire * 25;

                    if (nbStations == nbStationsMin && temps <= 20) {
                        Console.WriteLine($"- Fin de minute :");
                        for (int j = 0; j < nbStations; j++) {
                            Console.WriteLine($" Station n°{j + 1} : {stations[j].ToString()}");
                        }
                        Console.WriteLine($" File ordinaire : {fileOrdinaire} client(s) présent(s) dans la file");
                        Console.WriteLine($" File prioritaire : {filePrioritaire} client(s) présent(s) dans la file\n");
                    }
                }

                // Affichage des couts
                Console.WriteLine("Affichage des différents couts");
                Console.WriteLine($" Clients ordinaires : {couts[iCouts].clientsOrdinaires}");
                Console.WriteLine($" Clients prioritaires : {couts[iCouts].clientsPrioritaires}");
                Console.WriteLine($" Occupation des stations ordinaires : {couts[iCouts].stationsOrdinairesOccupées}");
                Console.WriteLine($" Occupation des stations prioritaires : {couts[iCouts].stationsPrioritairesOccupées}");
                Console.WriteLine($" Stations inoccupées : {couts[iCouts].stationsInoccupées}");
                Console.WriteLine($" Changements de statut prioritaire vers ordinaire : {couts[iCouts].prioritairesDéchus}");
                Console.WriteLine($"Total des couts : {couts[iCouts].CoutsTotaux()}\n");

                iCouts++;
            }

            // Partie calculs du nombre de stations optimal
            double coutOptimal = double.MaxValue;
            int iCoutOptimal = 0;
            int i = 0;
            // Vérification des valeurs
            foreach (Couts cout in couts) {
                double sommeCouts = cout.clientsOrdinaires + cout.clientsPrioritaires + cout.prioritairesDéchus + cout.stationsInoccupées + cout.stationsOrdinairesOccupées + cout.stationsPrioritairesOccupées;
                if (sommeCouts < coutOptimal) {
                    coutOptimal = sommeCouts;
                    iCoutOptimal = i;
                }
                i++;
            }

            return couts[iCoutOptimal].nbStations;
        }
        #endregion
        static void Main(string[] args) {
            int X0 = 19;
            int a = 261;
            int c = 7;
            int m = 13000;
            bool nombresValides = VerificationValeurs(X0, a, c, m);

            if (!nombresValides) {
                Console.WriteLine(" Les nombres ne sont pas valides\n");
            } else {
                Console.WriteLine(" Les nombres sont valides\n");
                int[] nombresAleatoires = new int[m];
                int[] tabSaut = new int[m/2];
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
                TestDesCourses(nombresAleatoires, tabSaut);

                int nbStationsOptimal = NbStationsOptimal(2, 9, 600, X0, a, c, m);
                Console.WriteLine($"-------------------- Résultat des tests --------------------");
                Console.WriteLine($" Nombre de stations optimal : {nbStationsOptimal}");
            }
            Console.ReadLine();
        }
    }
}
