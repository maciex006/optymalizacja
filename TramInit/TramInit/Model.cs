using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramInit
{
    class Model
    {
        private const int MAX_DISTANCE = 20;
        private int[,] costMatrix;
        private Station[] stationsArray;
        private int stationsNumber;
        Random random = new Random();

        /// <summary>
        /// Metoda generująca macierz reprezentującą połączenia między przystankami.
        /// </summary>
        /// <param name="stationsNumber">Liczba stacji w modelu.</param>
        /// <param name="intervalsNumber">Liczba interwałów czasowych w modelu.</param>
        private void GenerateCost()
        {
            costMatrix = new int[stationsNumber, stationsNumber];
            for (int i = 0; i < stationsNumber; ++i)
            {
                for (int j = 0; j < stationsNumber; ++j)
                {
                    if (i < j)
                    {
                        costMatrix[i, j] = random.Next(0, 2);
                        costMatrix[i, j] = costMatrix[i, j] * random.Next(1, MAX_DISTANCE + 1);
                    }
                    else if (i > j)
                    {
                        costMatrix[i, j] = costMatrix[j, i];
                    }
                }
            }
        }

        public string CostMatrixToString()
        {
            return FormMatrixString(costMatrix);
        }

        public string TrafficMatrixToString(int stationId)
        {
            if (stationId > -1 && stationId < stationsArray.Length)
            {
                return FormMatrixString(stationsArray[stationId].GetTrafficMatrix());
            }
            else
            {
                return "Nieodpowiedni id.";
            }
        }

        private string FormMatrixString(int[,] matrix)
        {
            string line = "";
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                line = line + "|";
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    line = (matrix[i, j] < 10) ? (line + " " + matrix[i, j]) : (line + matrix[i, j]);
                    line = (j == matrix.GetLength(1) - 1) ? (line + "|") : (line + ",");
                }
                line = line + System.Environment.NewLine;
            }

            return line;
        }

        /// <summary>
        /// Konstruktor generujący model dla podanych parametrów.
        /// </summary>
        /// <param name="stationsNumber">Liczba przystanków w modelu.</param>
        /// <param name="intervalsNumer">Liczba interwałów czasowych dla których generujemy ruch. Domyślnie jeden dla najprostszego modelu.</param>
        public Model(int stationsNumber, int intervalsNumer = 1)
        {
            this.random = new Random();
            this.stationsNumber = stationsNumber;
            GenerateCost();
            stationsArray = new Station[stationsNumber];
            for (int i = 0; i < stationsNumber; ++i)
            {
                stationsArray[i] = new Station(i, stationsNumber, intervalsNumer, random);
            }
        }

        /// <summary>
        /// Konstruktor generujący model na podstawie podanych macierzy.
        /// </summary>
        public Model()
        {
            // TODO mfrancikiewicz 06-05-2015 Dopisać implementację konstruktora.
        }
    }

    class Station
    {
        private const int PASSENGER_MAX_NUMBER = 10;
        private int id;
        private string name;
        private int[,] trafficMatrix;
        private Random random;
        /// <summary>
        /// Metoda generująca macierz reprezentującą ruch pasażerów na przystanku.
        /// </summary>
        /// <param name="idStacji"> Identyfikator stacji. </param>
        /// <param name="stationsNumber">Liczba stacji w modelu.</param>
        /// <param name="intervalsNumber">Liczba interwałów czasowych w modelu.</param>
        private void GenerateTraffic(int idStacji, int stationsNumber, int intervalsNumber)
        {
            trafficMatrix = new int[stationsNumber, intervalsNumber];
            for (int i = 0; i < stationsNumber; ++i)
            {
                for (int j = 0; j < intervalsNumber; ++j)
                {
                    if (i == id)
                    {
                        trafficMatrix[i, j] = -1;
                    }
                    else
                    {
                        trafficMatrix[i, j] = random.Next(0, PASSENGER_MAX_NUMBER);
                    }
                }
            }
        }

        public int[,] GetTrafficMatrix()
        {
            return trafficMatrix;
        }

        /// <summary>
        /// Konstruktor generujący przystanek dla podanych parametrów.
        /// </summary>
        /// <param name="id">Id przystanku.</param>
        /// <param name="stationsNumber">Liczba stacji w modelu.</param>
        /// <param name="intervalsNumer">Liczba interwałów czasowych w modelu.</param>
        /// <param name="name">Nazwa przystanku - opcjonalnie.</param>
        public Station(int id, int stationsNumber, int intervalsNumer, Random random, string name = "Default")
        {
            this.random = random;
            this.id = id;
            this.name = name;
            GenerateTraffic(id, stationsNumber, intervalsNumer);
        }


    }
}
