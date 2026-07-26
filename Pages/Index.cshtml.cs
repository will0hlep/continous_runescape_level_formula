using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;
using System.Runtime.CompilerServices;

namespace RSConLvl.Pages;

public class IndexModel : PageModel
{   
    private static int XpCalc(int level)
    {
        Console.WriteLine("test");
        //"""Returns an integer xp value for an integer level value"""
        double xp = 0;
        for (int i = 1; i < level; i++)
        {
            xp += 0.25*Math.Floor(i + 300 * Math.Pow(2, i / 7));
        }
        return (int) Math.Floor(xp);
    }

    private static double XpApprox(double level)
    {
        //"""Returns an approximate float xp value for a float level value"""
        double xp = Math.Pow(2, level/7);
        xp -= Math.Pow(2, 1/7);
        xp *= 75;
        xp /= Math.Pow(2, 1/7) - 1;
        xp += level*(level-1)/8;
        return xp;
    }

    private static Vector<double> PowerFourierSeries()
    {
        //"""Returns the coeffiecents of the continous level to xp formula"""
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;
        Matrix<double> A = M.Dense(127,127);
        Vector<double> b = V.Dense(127);
        for (int i = 0; i < 127; i++)
        {
            A[i,0] = i;
            b[i] = XpCalc(i) - XpApprox(i);
            for (int j = 1; j < 64; j++)
            {
                A[i,2*j] = Math.Cos(2*Math.PI*i*j/127);
                A[i,2*j-1] = Math.Sin(2*Math.PI*i*j/127);
            }
        }
        return A.Solve(b);
    }

    private static readonly Vector<double> coefficients = PowerFourierSeries();

    private static double XpContinuous(double level)
    {
        //Returns a float xp value for a float level value
        double xp = XpApprox(level);
        xp += coefficients[0] * level;
        for (int i = 1; i < 64; i++)
        {
            xp += coefficients[2*i]*Math.Cos(2*Math.PI*level*i/127);
            xp += coefficients[2*i-1]*Math.Sin(2*Math.PI*level*i/127);
        }
        return xp;
    }

    private static double LevelContinous(int xp)
    {
        //Returns a float level value for a float xp value
        double level = Brent.FindRoot(XpContinuous, 1, 127);
        int nearest = (int) Math.Round(level);
        if (XpCalc(nearest) == xp)
        {
            return nearest;
        }
        return level;
    }

    private static readonly int[] eliteLevels =  [0, 830, 1861, 2902, 3980,
        5126, 6380, 7787, 9400, 11275, 13605, 16372, 19656, 23546, 28134,
        33520, 39809, 47109, 55535, 65209, 77190, 90811, 106221, 123573,
        143025, 164742, 188893, 215651, 245196, 277713, 316311, 358547, 404634,
        454796, 509259, 568254, 632019, 700797, 774834, 854383, 946227,
        1044569, 1149696, 1261903, 1381488, 1508756, 1644015, 1787581, 1939773,
        2100917, 2283490, 2476369, 2679917, 2894505, 3120508, 3358307, 3608290,
        3870846, 4146374, 4435275, 4758122, 5096111, 5449685, 5819299, 6205407,
        6608473, 7028964, 7467354, 7924122, 8399751, 8925664, 9472665,
        10041285, 10632061, 11245538, 11882262, 12542789, 13227679, 13937496,
        14672812, 15478994, 16313404, 17176661, 18069395, 18992239, 19945833,
        20930821, 21947856, 22997593, 24080695, 25259906, 26475754, 27728955,
        29020233, 30350318, 31719944, 33129852, 34580790, 36073511, 37608773,
        39270442, 40978509, 42733789, 44537107, 46389292, 48291180, 50243611,
        52247435, 54303504, 56412678, 58575824, 60793812, 63067521, 65397835,
        67785643, 70231841, 72737330, 75303019, 77929820, 80618654, 83370445,
        86186124, 89066630, 92012904, 95025896, 98106559, 101255855, 104474750,
        107764216, 111125230, 114558777, 118065845, 121647430, 125304532,
        129038159, 132849323, 136739041, 140708338, 144758242, 148889790,
        153104021, 157401983, 161784728, 166253312, 170808801, 175452262,
        180184770, 185007406, 189921255, 194927409];

    private class EliteCoefficients(
        int[][] polynomials,
        double[][] polynomialCoefficients,
        Vector<double>[] fourierCoefficients
        )
    {
        public int[][] Polynomials { get; } = polynomials;
        public double[][] PolynomialCoefficients { get; } = polynomialCoefficients;
        public Vector<double>[] FourierCoefficients { get; } = fourierCoefficients;
    }

    private static EliteCoefficients ElitePowerFourierSeries()
    {
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;
        int[][] polynomials = [[1,2,1], [2,10,4], [10,20,4], [20,30,4],
            [30,40,4], [40,50,4], [0,60,4], [60,70,4], [70,80,4], [80,90,4],
            [90,100,4], [100,150,6]];
        double[][] polynomialCoefficients = new double[12][];
        Vector<double>[] fourierCoefficients = new Vector<double>[12];
        polynomialCoefficients[0] = [830.0, -830.0];
        fourierCoefficients[0] = V.Dense([0.0]);
        for (int i = 1; i < 12; i++)
        {
            int seriesSize = polynomials[i][1] - polynomials[i][0] + 1;
            double[] level = new double[seriesSize];
            double[] xp = new double[seriesSize];
            for (int j = 0; j < seriesSize; j++)
            {
                level[j] = polynomials[i][0] + j;
                xp[j] = eliteLevels[polynomials[i][0] + j - 1];
            }
            polynomialCoefficients[i] = Fit.Polynomial(level, xp, polynomials[i][2]);
            Matrix<double> A = M.Dense(seriesSize,seriesSize);
            Vector<double> b = V.Dense(seriesSize);
            for (int j = 0; j < seriesSize; j++)
            {
                A[j,0] = 1;
                b[j] = xp[j];
                for (int k = 1; k < (seriesSize+1)/2; k++)
                {
                    A[j,2*k] = Math.Cos(2*Math.PI*level[j]*k/(seriesSize + (i == 11? 1 : 0)));
                    A[j,2*k-1] = Math.Sin(2*Math.PI*level[j]*k/(seriesSize + (i == 11? 1 : 0)));
                }
                for (int k = 0; k <= polynomials[i][2]; k++)
                {
                    b[j] -= polynomialCoefficients[i][polynomials[i][2]-k] * Math.Pow(level[j], k);
                }
            }
            fourierCoefficients[i] = A.Solve(b);
        }
        return new EliteCoefficients(polynomials, polynomialCoefficients, fourierCoefficients);
    }

    private static readonly EliteCoefficients eliteCoefficients = ElitePowerFourierSeries();

    private static double EliteXpContinuous(double level)
    {
        //Returns a float xp value for a float level value
        int tier = 11;
        while (eliteCoefficients.Polynomials[tier][0] > level)
        {
            tier --;
        }
        int polynomial_degree = eliteCoefficients.Polynomials[tier][2];
        double xp = 0;
        for (int i = 0; i <= polynomial_degree; i++)
        {
            xp += eliteCoefficients.PolynomialCoefficients[tier][polynomial_degree-i] * Math.Pow(level, i);
        }
        xp += eliteCoefficients.FourierCoefficients[tier][0];
        int seriesSize = eliteCoefficients.Polynomials[tier][1]-eliteCoefficients.Polynomials[tier][0] + 1;
        for (int i = 1; i < (seriesSize+1)/2; i++)
        {
            xp += eliteCoefficients.FourierCoefficients[tier][2*i] * Math.Cos(2*Math.PI*level*i/(seriesSize + (tier == 11? 1 : 0)));
            xp += eliteCoefficients.FourierCoefficients[tier][2*i-1] * Math.Sin(2*Math.PI*level*i/(seriesSize + (tier == 11? 1 : 0)));
        }
        return xp;
    }

    private static double EliteLevelContinous(int xp)
    {
        //Returns a float level value for a float xp value
        double level = Brent.FindRoot(EliteXpContinuous, 1, 151);
        int nearest = (int) Math.Round(level);
        if (eliteLevels[nearest - 1] == xp)
        {
            return nearest;
        }
        return level;
    }
}