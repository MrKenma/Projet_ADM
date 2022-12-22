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

    class TabFrequences {
        public List<int> valeurs = new List<int>();
        public int ri;          // Fréquence observée
        public double pi;
        public double nFoisPi;  // Fréquence théorique
        public double X2Observable;
        
        public TabFrequences(int valeur, int ri, double pi, double nFoisPi) {
            this.ri = ri;
            this.pi = pi;
            this.nFoisPi = nFoisPi;
            valeurs.Add(valeur);
            SetX2Observable();
        }

        public String Valeur(int max) {
            StringBuilder output = new StringBuilder();

            if (valeurs.Contains(max) && ri == 0) {
                output.Append($">={valeurs.Min()}");
            } else {
                if (valeurs.Count > 1) {
                    output.Append($"[{valeurs[0]}");
                    for (int i = 1; i < valeurs.Count(); i++) {
                        output.Append($", {valeurs[i]}");
                    }
                    output.Append("]");
                } else {
                    output.Append(valeurs[0]);
                }
            }

            return output.ToString();
        }

        public void SetX2Observable() {
            X2Observable = Math.Pow(ri - nFoisPi, 2) / nFoisPi;
        }

        public String GetPi() {
            return pi.ToString("0.00000");
        }

        public String GetNFoisPi() {
            return nFoisPi.ToString("0.00000");
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
            foreach (int p in facteursPremiersDeM) {
                if ((a - 1) % p != 0) {
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
            //Console.WriteLine($" Nombre de valeurs ignorées : {cptIgnored}\n");

            return cptIgnored;
        }

        static double CalculFactorielle(int a) {
            double fact = 1;
            for (int x = 1; x <= a; x++) {
                fact *= x;
            }

            return fact;
        }

        static String Etape1() {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Etape 1 :");
            output.AppendLine("H0 : la suite est acceptable pour le test des courses");
            output.AppendLine("H1 : la suite n'est pas acceptable pour le test des courses");

            return output.ToString();
        }

        static String Etape2(double alpha) {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Etape 2 :");
            output.AppendLine($"Le niveau d'incertitude α = {alpha}");

            return output.ToString();
        }

        static String Etape3(int max, int n, List<TabFrequences> tabFrequences, ref bool regroupementNecessaire) {
            StringBuilder output = new StringBuilder();
            double sommePi = tabFrequences.Sum(tab => tab.pi);
            double sommeNFoisPi = tabFrequences.Sum(tab => tab.nFoisPi);
            double sommeX2Observable = tabFrequences.Sum(tab => tab.X2Observable);

            output.AppendLine("Etape 3 :");
            output.AppendLine("-------------------------------------------------------------------------------------------");
            output.AppendLine("     xi     |     ri     |      pi      |      n * pi      |  (ri - n * pi)^(2) / (n * pi)");
            output.AppendLine("-------------------------------------------------------------------------------------------");
            foreach (TabFrequences tab in tabFrequences) {
                output.AppendLine($"     {tab.Valeur(max)}      |    {tab.ri}\t |    {tab.GetPi()}   |    {tab.GetNFoisPi()}\t   |    {tab.X2Observable}");
                output.AppendLine("-------------------------------------------------------------------------------------------");
                if (tab.nFoisPi < 5) {
                    regroupementNecessaire = true;
                }
            }

            output.AppendLine($"    Total   |    {n}    |      {sommePi.ToString("0.00000")}       |       {sommeNFoisPi.ToString("0.00000")}       |  X2obs = {sommeX2Observable}");
            output.AppendLine("-------------------------------------------------------------------------------------------");

            return output.ToString();
        }

        static String Etape4(ref int max, int n, List<TabFrequences> tabFrequences, ref bool regroupementNecessaire, ref bool regroupementImpossible) {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Etape 4 :");

            if (regroupementNecessaire) {
                output.AppendLine("Regroupement nécessaire !");
                regroupementNecessaire = false;

                // Regroupement
                int iVal = max - 1;
                while (iVal >= 0) {
                    if (tabFrequences[iVal].nFoisPi < 5) {
                        if (iVal == 0) {
                            output.AppendLine("Regroupement impossible, n < 5 !");
                            regroupementImpossible = true;
                        } else {
                            foreach (int val in tabFrequences[iVal].valeurs) {
                                tabFrequences[iVal - 1].valeurs.Add(val);
                            }
                            tabFrequences[iVal - 1].ri += tabFrequences[iVal].ri;
                            tabFrequences[iVal - 1].pi += tabFrequences[iVal].pi;
                            tabFrequences[iVal - 1].nFoisPi += tabFrequences[iVal].nFoisPi;
                            tabFrequences[iVal - 1].SetX2Observable();

                            tabFrequences.RemoveAt(iVal);
                        }
                    }

                    iVal--;
                }

                // On ajuste la valeur de max
                max = tabFrequences.Count;

                if (!regroupementImpossible) {
                    // Retour étape 3
                    output.AppendLine("=> Retour à l'étape 3\n");
                    output.AppendLine(Etape3(max, n, tabFrequences, ref regroupementNecessaire));

                    output.AppendLine("\n");
                    output.AppendLine(Etape4(ref max, n, tabFrequences, ref regroupementNecessaire, ref regroupementImpossible));
                }
            } else {
                output.AppendLine("Les contraintes sont respectées !");
            }

            return output.ToString();
        }

        static String Etape5(int max, int n, List<TabFrequences> tabFrequences, bool regroupementImpossible, ref double chiCarré) {
            StringBuilder output = new StringBuilder();
            int v = max - 1;
            double[] tabChiCarré = {3.841, 5.991, 7.815, 9.488, 11.070, 12.592, 14.067, 15.507, 16.919};

            output.AppendLine("Etape 5 :");
            output.AppendLine($"v = {max} - 1 = {v}");
            output.AppendLine($"Contrainte n*pi >= 5 : {(regroupementImpossible ? "KO" : "OK")}");
            if (v > 0) {
                chiCarré = tabChiCarré[v - 1];
                output.AppendLine($"Zone de non-rejet = [0;{chiCarré.ToString("0.000")}]");
            } else {
                chiCarré = 0;
                output.AppendLine("v = 0, impossible d'établir une zone de non rejet");
            }

            return output.ToString();
        }

        static String Etape6(double chiCarré, double alpha, List<TabFrequences> tabFrequences) {
            StringBuilder output = new StringBuilder();
            double X2Observable = tabFrequences.Sum(tab => tab.X2Observable);
            bool valide = X2Observable >= 0 && X2Observable <= chiCarré; 

            output.AppendLine("Etape 6 :");
            output.AppendLine($"La valeur {X2Observable} se trouve {(valide ? "à l'intérieur" : "en dehors")} " +
                $"de la zone de non-rejet [0;{chiCarré.ToString("0.000")}]. Avec un niveau d'incertitude alpha = {alpha}, " +
                $"nous pouvons donc {(valide ? "accepter l'hypothèse H0" : "rejeter l'hypothèse H0 au profit de H1")}.");

            return output.ToString();
        }

        static void TestDesCourses(int[] nombres, int[] tabSaut) {
            Console.WriteLine("-------------------- Test des courses --------------------");
            // Etape 1 : poser une hypothèse H0 (suite générée acceptable ou non)
            Console.WriteLine(Etape1());
            // Etape 2 : fixer le niveau d'incertitude alpha (en général alpha = 5%)
            double alpha = 0.05;
            Console.WriteLine(Etape2(alpha));

            // Création du tableau des sauts
            int cptIgnored = CreationTabSaut(nombres, tabSaut);
            int n = nombres.Length - cptIgnored;

            // Détermination de la taille max des sauts
            int max = 0;
            for (int i = 0; i < tabSaut.Length; i++) {
                if (max < tabSaut[i]) {
                    max = tabSaut[i];
                }
            }

            // Comptage du nombre de saut de chaque taille 
            int[] tabCourses = new int[max];
            int valeur = 0;
            for (int i = 0; i < tabSaut.Length; i++) {
                if (tabSaut[i] > 0) {
                    valeur = tabSaut[i];

                    tabCourses[valeur - 1]++;
                }
            }

            // Remplissage du tableau de valeurs de fréquences (valeur, ri, pi, n*pi et )
            List<TabFrequences> tabFrequences = new List<TabFrequences>();

            for (int i = 0; i < max; i++) {
                double fact = CalculFactorielle(i + 2);
                double pi = (i + 1) / fact;
                tabFrequences.Add(new TabFrequences(i + 1, tabCourses[i], pi, n*pi));
            }

            // Si la somme des n*pi ne donne pas n
            double sommeNFoisPi = tabFrequences.Sum(val => val.nFoisPi);
            if (Math.Round(sommeNFoisPi) != n) {
                int val = tabFrequences.Count + 1;
                double pi = 1 - tabFrequences.Sum(x => x.pi);
                double nFoisPi = n - sommeNFoisPi;

                tabFrequences.Add(new TabFrequences(val, 0, pi, nFoisPi));

                max++;
            }

            // Etape 3 : tableau recensé des fréquences observées et des fréquences théoriques + calcul de la statistique observable X2v
            bool regroupementNecessaire = false;
            Console.WriteLine(Etape3(max, n, tabFrequences, ref regroupementNecessaire));

            // si regroupement, max change
            // Etape 4 : vérifier les contraintes à respecter pour chaque test et, le cas échéant, retourner à l'étape 3 en effectuant les regroupements
            bool regroupementImpossible = false;
            Console.WriteLine(Etape4(ref max, n, tabFrequences, ref regroupementNecessaire, ref regroupementImpossible));

            // Etape 5 : établir la zone de non-rejet en fonction du nombre de degrés de liberté
            double chiCarré = 0;
            Console.WriteLine(Etape5(max, n, tabFrequences, regroupementImpossible, ref chiCarré));

            // Etape 6 : prendre une décision, rejet ou non-rejet de l'hypothèse
            Console.WriteLine(Etape6(chiCarré, alpha, tabFrequences));
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

            // Boucle du nombre de station minimal au nombre de station maximal
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

                // Boucle sur les 600 minutes
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

                    // Génération du nombre de nouveaux clients ordinaires et prioritaires
                    X0 = NbArriveesGenerees(X0, a, c, m, out nbArrivées);
                    X0 = NbClientsPrioritairesEtOrdinaires(nbArrivées, X0, a, c, m, out nbOrdinaires, out nbPrioritaires);

                    if (nbStations == nbStationsMin && temps <= 20) {
                        Console.WriteLine($" {nbArrivées} nouveau(x) client(s) : {nbOrdinaires} ordinaire(s) et {nbPrioritaires} prioritaire(s)");
                    }

                    // Placement des nouveaux clients dans la file prioritaire puis dans la file ordinaire
                    while (filePrioritaire < 5 && nbPrioritaires > 0) {
                        filePrioritaire++;
                        nbPrioritaires--;
                    }
                    nbPrioritairesDéchus = nbPrioritaires;
                    fileOrdinaire += nbOrdinaires + nbPrioritairesDéchus;
                    couts[iCouts].prioritairesDéchus += 30 * nbPrioritairesDéchus;

                    if (nbStations == nbStationsMin && temps <= 20) {
                        Console.WriteLine(" Après placement des nouveaux clients :");
                        Console.WriteLine($"  File ordinaire : {fileOrdinaire} client(s) présent(s) dans la file");
                        Console.WriteLine($"  File prioritaire : {filePrioritaire} client(s) présent(s) dans la file");
                    }

                    // Station pour clients prioritaires
                    if (stations[0].tempsRestant == 0) {        // S'il n'y plus personnes à cette station
                        if (filePrioritaire != 0) {     // S'il reste des clients dans la file prioritaire
                            filePrioritaire--;
                            X0 = DureeGeneree(X0, a, c, m, stations[0]);
                            stations[0].statusClient = "prioritaire";
                            if (nbStations == nbStationsMin && temps <= 20) {
                                Console.WriteLine($" Nouveau client {stations[0].statusClient} dans la station n°{1} : durée d'attente de {stations[0].tempsRestant} minute(s)");
                            }
                            couts[iCouts].clientsPrioritaires += 1.0 / 60 * stations[0].tempsRestant * 40;
                            couts[iCouts].stationsPrioritairesOccupées += 1.0 / 60 * stations[0].tempsRestant * 75;
                            stations[0].tempsRestant--;
                        } else {    // S'il n'y a personne dans la file prioritaire
                            stations[0].statusClient = "inexistant";
                            couts[iCouts].stationsInoccupées += 1.0 / 60 * 20;
                        }
                    } else {    // S'il a toujours quelqu'un à cette station
                        stations[0].tempsRestant--;
                    }

                    // Stations normales
                    for (int iStation = 1; iStation < nbStations; iStation++) {     // Pour chaque station ordinaire
                        if (stations[iStation].tempsRestant == 0) {     // S'il n'y plus personnes à cette station
                            if (fileOrdinaire != 0) {        // S'il reste des clients dans la file ordinaire
                                fileOrdinaire--;
                                X0 = DureeGeneree(X0, a, c, m, stations[iStation]);
                                stations[iStation].statusClient = "ordinaire";
                                if (nbStations == nbStationsMin && temps <= 20) {
                                    Console.WriteLine($" Nouveau client {stations[iStation].statusClient} dans la station n°{iStation + 1} : durée d'attente de {stations[iStation].tempsRestant} minute(s)");
                                }
                                couts[iCouts].clientsOrdinaires += 1.0 / 60 * stations[iStation].tempsRestant * 25;
                                couts[iCouts].stationsOrdinairesOccupées += 1.0 / 60 * stations[iStation].tempsRestant * 50;
                                stations[iStation].tempsRestant--;
                            } else if (filePrioritaire != 0) {       // Si la file ordinaire est vide mais pas la file prioritaire
                                filePrioritaire--;
                                X0 = DureeGeneree(X0, a, c, m, stations[iStation]);
                                stations[iStation].statusClient = "prioritaire";
                                if (nbStations == nbStationsMin && temps <= 20) {
                                    Console.WriteLine($" Nouveau client {stations[iStation].statusClient} dans la station n°{iStation + 1} : durée d'attente de {stations[iStation].tempsRestant} minute(s)");
                                }
                                couts[iCouts].clientsPrioritaires += 1.0 / 60 * stations[iStation].tempsRestant * 40;
                                couts[iCouts].stationsOrdinairesOccupées += 1.0 / 60 * stations[iStation].tempsRestant * 50;
                                stations[iStation].tempsRestant--;
                            } else {    // Si les deux files sont vide
                                stations[iStation].statusClient = "inexistant";
                                couts[iCouts].stationsInoccupées += 1.0 / 60 * 20;
                            }
                        } else {    // S'il a toujours quelqu'un à cette station
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
            int X0 = 19;    // 19
            int a = 61;    // 261
            int c = 49;      // 7
            int m = 1024;  // 13 000
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
