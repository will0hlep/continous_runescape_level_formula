using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;

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

    private static double XpContinous(double level)
    {
        double xp = XpApprox(level);
        Vector<double> coefficents = PowerFourierSeries();
        xp += coefficents[0] * level;
        for (int i = 1; i < 64; i++)
        {
            xp += coefficents[2*i]*Math.Cos(2*Math.PI*level*i/127);
            xp += coefficents[2*i-1]*Math.Sin(2*Math.PI*level*i/127);
        }
        return xp;
    }

    private static double LevelContinous(int xp)
    {
        int lowLevel = 1;
        int highLevel = 127;
        while (lowLevel < highLevel)
        {
            int mid = (lowLevel + highLevel + 1) / 2;
            if (XpCalc(mid) <= xp)
            {
                lowLevel = mid;
            } else {
                highLevel = mid;
            }
        }

        int level = 
        double xpTemp = XpCalc(level);
        while (xp < xpTemp)
        {
            level--;
            xpTemp = XpCalc(level);
        }
        if (xpTemp == xp)
        {
            return level;
        }
        return Brent.FindRoot(XpContinous, level-1, level);
    }

}