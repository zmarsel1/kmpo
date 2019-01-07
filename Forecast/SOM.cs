using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forecast
{
    public static class ShuffleClass
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
            yield return elements[0];
        }
    }
    class Vector
    {
        public double[] Input { get; set; }
        public double[] Output { get; set; }
    }
    class SOM
    {
        const int X = 0;
        const int Y = 1;
        const int DISTANCE = 2;

        public int Width { get; private set; } // количество рядов в карте
        public int Height { get; private set; } // количество столбцов в карте
        public double Alpha { get; private set; } //скорость обучения нейрона победителя
        public double Beta { get; private set; }
        public int Radius { get; private set; }
        public int InputDims { get; private set; }
        public int OutputDims { get; private set; }

        Vector[,] _nodes;

        public SOM(int inputDims, int outputDims, int width = 10, int height = 10, double alpha = 0.9, double beta = 0.1 )
        {
            InputDims = inputDims;
            OutputDims = outputDims;
            Width = width;
            Height = height;
            Alpha = alpha;
            Beta = beta;
            Radius = 4;

            _nodes = new Vector[width, height];
            for(int x = 0; x <= _nodes.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _nodes.GetUpperBound(1); y++)
                {

                    Random rnd = new Random();
                    var node = new Vector();
                    node.Input = new double[inputDims];
                    for (int i = 0; i < InputDims; i++)
                    {
                        node.Input[i] = rnd.NextDouble();
                    }
                    node.Output = new double[outputDims];
                    for (int i = 0; i < OutputDims; i++)
                    {
                        node.Output[i] = rnd.NextDouble();
                    }
                    _nodes[x, y] = node;
                }
            }
        }

        public void Train(Vector[] inputs, int iterations = 1000)
        {
            var indeces = Enumerable.Range(0, inputs.Count());
            for (int iteration = 0; iteration < iterations; iteration++)
            { 
                double alpha = Alpha * Math.Pow( Beta / Alpha, (float)(iteration/iterations));
                double radius = (Radius * Math.Pow( 0.01 / Radius, (double)(iteration)/iterations));

                if ((iteration + 1) % 20 == 0) { indeces = ShuffleClass.Shuffle(indeces, new Random(DateTime.Now.Second)); }
                foreach (int index in indeces)
                {
                    int[] best = FindBMU(inputs[index].Input);
                    foreach (var location in FindNeighborhood(best, radius))
                    {
                        double influence = Math.Exp( (-1.0 * (double)location[DISTANCE])/ (2* radius * Math.Pow(Math.Pow(0.01/ Radius, (double)iteration/iterations),2)));
                        double inf_lrd = influence * alpha;

                        _nodes[(int)location[X], (int)location[Y]].Input = inputs[index].Input.Zip(_nodes[(int)location[X], (int)location[Y]].Input, (v1, v2) => v2 + inf_lrd * (v1 - v2)).ToArray();
                        _nodes[(int)location[X], (int)location[Y]].Output = inputs[index].Output.Zip(_nodes[(int)location[X], (int)location[Y]].Output, (v1, v2) => v2 + inf_lrd * (v1 - v2)).ToArray();
                    }
                }
            }
        
        }

        int[] FindBMU(double[] v)
        {
            int[] coord = new int[2];
            double minSum = - 1.0;
            for (int row = 0; row <= _nodes.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= _nodes.GetUpperBound(1); col++)
                {
                    var a = _nodes[row, col].Input.Zip(v, (v1, v2) => Math.Pow((v1 - v2), 2) ).Sum();
                    if (minSum > a || minSum < 0)
                    {
                        minSum = a;
                        coord[X] = row;
                        coord[Y] = col;
                    }
                }
            }
            return coord;
        }

        IEnumerable<object[]> FindNeighborhood(int[] coord, double radius)
        {
            var r2 = radius * radius;
            List<object[]> neighbors = new List<object[]>();
            int min_y = (int)Math.Max(coord[Y] - radius, 0.0);
            int max_y = (int)Math.Min(coord[Y] + radius, Height - 1);
            int min_x = (int)Math.Max(coord[X] - radius, 0.0);
            int max_x = (int)Math.Min(coord[X] + radius, Width - 1);
            for (int x = min_x; x <= max_x; x++)
            {
                for (int y = min_y; y <= max_y; y++)
                {
                    var dist = Math.Pow(x - coord[X], 2) + Math.Pow(y - coord[Y], 2);
                    if (dist <= r2)
                        neighbors.Add(new object[] { x, y, dist });
                }
            }
            return neighbors.AsEnumerable();
        }

        public double[] Compute(double[] input)
        {
            var coord = FindBMU(input);
            return _nodes[coord[X], coord[Y]].Output;
        }
    }
}
