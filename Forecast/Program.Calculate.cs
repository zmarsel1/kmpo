using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using DAL.Concrete;
using AForge.Neuro.Learning;
using AForge;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace Forecast
{
    partial class Program
    {
        static int retrospective = 5;
        static ActivationNetwork CreateTimeSeriesModel_v2(string tagname, out double yMax, out double yMin, DayOfWeek model_type) 
        {
            //основные параметры обучения
            double sigmoidAlphaValue = 2;
            double learningRate = 0.1;
            double momentum = 0.9;
            int iterations = 10000;

            //количество  примеров

            int samples = 200;

            var context = new TagDbContext();

            double[] data;
            if(model_type == DayOfWeek.Saturday || model_type == DayOfWeek.Sunday) {
                
                data = context.TagValues
                    .Where(t => t.Tag.TagName.Trim() == tagname &&  (SqlFunctions.DatePart("weekday", t.DateTime) -1) == (int)model_type)
                    .OrderByDescending(v => v.DateTime)
                    .Select(v => v.Value)
                    .Take(samples + retrospective + 1)
                    .AsEnumerable()
                    .Reverse()
                    .ToArray();
            }
            else {
                data = context.TagValues
                    .Where(t => t.Tag.TagName.Trim() == tagname && (SqlFunctions.DatePart("weekday", t.DateTime) - 1) != (int)DayOfWeek.Sunday && (SqlFunctions.DatePart("weekday", t.DateTime) - 1) != (int)DayOfWeek.Saturday)
                    .OrderByDescending(v => v.DateTime)
                    .Select(v => v.Value)
                    .Take(samples + retrospective + 1)
                    .AsEnumerable()
                    .Reverse()
                    .ToArray();
            }
            
            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            // для приведения входных даннанных к промежутку от 0 до 1
            double max = data.Max() == 0 ? 1 : data.Max();
            double min = data.Min();

            yMin = min;
            yMax = max;

            for (int i = 0; i < samples; i++) {
                input[i] = new double[retrospective];
                output[i] = new double[1];

                input[i] = data.Skip(i).Take(retrospective).Select(v => (max - v) / (max - min)).ToArray();
                // set output
                output[i][0] = (max - data[retrospective + i + 1]) / (max - min);
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                retrospective, retrospective * 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations

            for (int iteration = 0, trynumber = 1; iteration < iterations; iteration++) {
                double error = teacher.RunEpoch(input, output) / samples;
                if (iteration == (iterations - 1) && error > 0.05 && trynumber < 4) {
                    trynumber++;
                    iteration = 0;
                };
            }
            return network;
        }

        static ActivationNetwork CreateTimeSeriesModel(string tagname, out double yMax, out double yMin)
        {
            //основные параметры обучения
            double sigmoidAlphaValue = 2;
            double learningRate = 0.1;
            double momentum = 0.9;
            int iterations = 10000;

            //количество  примеров

            int samples = 7 * 24;

            var context = new TagDbContext();
            double[] data = context.TagValues
                .Where(t => t.Tag.TagName == tagname)
                .OrderByDescending(v => v.DateTime)
                .Select(v => v.Value)
                .Take(samples + retrospective + 1)
                .AsEnumerable()
                .Reverse()
                .ToArray();

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            // для приведения входных даннанных к промежутку от 0 до 1
            double max = data.Max() == 0 ? 1 : data.Max();
            double min = data.Min();

            yMin = min;
            yMax = max;

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[retrospective];
                output[i] = new double[1];

                input[i] = data.Skip(i).Take(retrospective).Select(v => (max - v) / (max - min)).ToArray();
                // set output
                output[i][0] = (max - data[retrospective + i + 1]) / (max - min);
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                retrospective, retrospective * 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                double error = teacher.RunEpoch(input, output) / samples;
            }
            return network;
        }

        static DistanceNetwork CreateSOM(string tagname)
        {
            int samples = 2 * 31 * 24;
            int retro = 3 ;
            var context = new TagDbContext();
            var data = context.TagValues
                .Where(t => t.Tag.TagName == tagname)
                .OrderByDescending(v => v.DateTime)
                .Select(v => new double[] { (double)v.DateTime.Hour, (double)v.DateTime.DayOfWeek, v.Value })
                .Take(samples + retrospective + 3)
                .AsEnumerable()
                .Reverse()
                .ToArray();

            double[][] trainingSet = new double[data.Length][];

            for (int index = 0; index < samples; index++)
            {
                trainingSet[index] = new double[] { data[index + 3][0], data[index + 3][1], data[index + 3][2],
                    data[index + 2][2], data[index + 1][2], data[index][2] };
            }

            var networkSize = 15;
            var iterations = 500;
            var learningRate = 0.3;
            var learningRadius = 3;

            Neuron.RandRange = new Range(0, 255);
            // create network
            DistanceNetwork network = new DistanceNetwork(2, networkSize * networkSize);

            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network, networkSize, networkSize);
            // create map
            //map = new int[networkSize, networkSize, 3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (true)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)learningRadius * (iterations - i) / iterations;
                // run training epoch
                trainer.RunEpoch(trainingSet);
                // increase current iteration
                i++;
                // stop ?
                if (i >= iterations)
                    break;
            }
            return network;
        }

        static SOM CreateSomModel_v3(string tagname, DayOfWeek day)
        {
            int samples = 24 * 10;

            var context = new TagDbContext();

            double[] data = context.TagValues
                .Where(t => t.Tag.TagName == tagname && (SqlFunctions.DatePart("weekday", t.DateTime) -1) == (int)day)
                .OrderByDescending(v => v.DateTime)
                .Select(v => v.Value)
                .Take(samples + retrospective + 1)
                .AsEnumerable()
                .Reverse()
                .ToArray();

            var inputs = new Vector[samples];
            for (int i = 0; i < samples; i++)
            {
                inputs[i] = new Vector();
                inputs[i].Input = data.Skip(i).Take(retrospective).ToArray();
                // set output
                inputs[i].Output = new double[] { data[retrospective + i + 1] };
            }

            SOM model = new SOM(5, 1, 12, 12);
            model.Train(inputs, 500);

            return model;
        }
        static SOM CreateSomModel_v2(string tagname)
        {
            int samples = 1500;

            var context = new TagDbContext();
            double[] data = context.TagValues
                .Where(t => t.Tag.TagName == tagname)
                .OrderByDescending(v => v.DateTime)
                .Select(v => v.Value)
                .Take(samples + retrospective + 1)
                .AsEnumerable()
                .Reverse()
                .ToArray();

            var inputs = new Vector[samples];
            for (int i = 0; i < samples; i++)
            {
                inputs[i] = new Vector();
                inputs[i].Input = data.Skip(i).Take(retrospective).ToArray();
                // set output
                inputs[i].Output = new double[] { data[retrospective + i + 1] };
            }

            SOM model = new SOM(5, 1, 15, 15);
            model.Train(inputs, 500);

            return model;
        }
        static SOM CreateSomModel(string tagname, out double[] vMax, out double[] vMin)
        {
            int retro = 3;
            int samples = 1500;

            var context = new TagDbContext();

            var data = context.TagValues
                                .Where(t => t.Tag.TagName.Trim() == tagname)
                                .OrderByDescending(v => v.DateTime)
                                .Select(v => new {v.DateTime.Hour, Day = SqlFunctions.DatePart("weekday", v.DateTime) + 1, Value = v.Value })
                                .Take(samples + retro + 1)
                                .AsEnumerable()
                                .Reverse()
                                .ToArray();

            vMax = new double[] { data.Max(d => d.Hour), data.Max(d => (double)d.Day), data.Max(d => d.Value), data.Max(d => d.Value), data.Max(d => d.Value) };
            vMin = new double[] { data.Min(d => d.Hour), data.Min(d => (double)d.Day), data.Min(d => d.Value), data.Min(d => d.Value), data.Min(d => d.Value) };
            
            var inputs = new Vector[samples];
            for (int index = 0; index < samples; index++)
            {
                inputs[index] = new Vector();
                inputs[index].Input = new double[] { 
                    (double)(vMax[0] - (double)data[index].Hour) / (vMax[0] - vMin[0]),
                    (double)(vMax[1] - (double)data[index].Day) /  (vMax[1] - vMin[1]),
                    (double)(vMax[2] - data[index + 1].Value) / (vMax[2] - vMin[2]),
                    (double)(vMax[3] - data[index + 2].Value) / (vMax[3] - vMin[3]),
                    (double)(vMax[4] - data[index + 3].Value) / (vMax[4] - vMin[4]) };
                inputs[index].Output = new double[] { data[index].Value };
            }
            SOM model = new SOM(5, 1, 15, 15);
            model.Train(inputs, 500);
            return model;
        }
    }
}
