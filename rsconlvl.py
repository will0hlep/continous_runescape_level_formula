"""Calculates a float runescape level value based on the xp value input."""

import argparse
from math import nextafter

from numpy import array, cos, pi, polyfit, sin, zeros
from numpy.linalg import solve
from scipy.optimize import fsolve

parser = argparse.ArgumentParser(
    description='Calculates a float runescape level value based on the xp value input.')
parser.add_argument(
    'xp', help='the integar amount of xp to be converted to a level value', type=float)
parser.add_argument(
    "-e", "--elite", help="if present the formula for elite skills will be used",
    action="store_true")
args = parser.parse_args()

if __name__ == "__main__":
    if args.xp < 0 or args.xp > 200000000:
        raise ValueError("xp must between 0 and 200m (inclusive)")
    if args.elite:
        LEVEL = 150


        def xp_calc(elite_level: int) -> int:
            """Returns an integer xp value for an integer level value"""
            elite = [0, 830, 1861, 2902, 3980, 5126, 6380, 7787, 9400, 11275,
                     13605, 16372, 19656, 23546, 28134, 33520, 39809, 47109,
                     55535, 65209, 77190, 90811, 106221, 123573, 143025,
                     164742, 188893, 215651, 245196, 277713, 316311, 358547,
                     404634, 454796, 509259, 568254, 632019, 700797, 774834,
                     854383, 946227, 1044569, 1149696, 1261903, 1381488,
                     1508756, 1644015, 1787581, 1939773, 2100917, 2283490,
                     2476369, 2679917, 2894505, 3120508, 3358307, 3608290,
                     3870846, 4146374, 4435275, 4758122, 5096111, 5449685,
                     5819299, 6205407, 6608473, 7028964, 7467354, 7924122,
                     8399751, 8925664, 9472665, 10041285, 10632061, 11245538,
                     11882262, 12542789, 13227679, 13937496, 14672812,
                     15478994, 16313404, 17176661, 18069395, 18992239,
                     19945833, 20930821, 21947856, 22997593, 24080695,
                     25259906, 26475754, 27728955, 29020233, 30350318,
                     31719944, 33129852, 34580790, 36073511, 37608773,
                     39270442, 40978509, 42733789, 44537107, 46389292,
                     48291180, 50243611, 52247435, 54303504, 56412678,
                     58575824, 60793812, 63067521, 65397835, 67785643,
                     70231841, 72737330, 75303019, 77929820, 80618654,
                     83370445, 86186124, 89066630, 92012904, 95025896,
                     98106559, 101255855, 104474750, 107764216, 111125230,
                     114558777, 118065845, 121647430, 125304532,129038159,
                     132849323, 136739041, 140708338, 144758242, 148889790,
                     153104021, 157401983, 161784728, 166253312, 170808801,
                     175452262, 180184770, 185007406, 189921255, 194927409]
            return elite[elite_level-1]


        def power_fourier_series():
            """Returns the coeffiecents of the continous level to xp formula"""
            polynomials = [(1,2,1), (2,10,4), (10,20,4), (20,30,4), (30,40,4),
                        (40,50,4), (50,60,4), (60,70,4), (70,80,4), (80,90,4),
                        (90,100,4), (100,150,6)]
            polynomials_coeff = [array([830,-830])]
            fourier_coeff = [array([0])]
            for i in range(1,12):
                series_size = polynomials[i][1]-polynomials[i][0]+1
                elite_level = zeros((series_size))
                elite_xp = zeros((series_size))
                for j in range(series_size):
                    elite_level[j] = polynomials[i][0] + j
                    elite_xp[j] = xp_calc(polynomials[i][0] + j)
                polynomials_coeff.append(polyfit(elite_level,elite_xp,polynomials[i][2]))
                A = zeros((series_size,series_size))
                b = zeros((series_size))
                for j in range(series_size):
                    A[j,0] = 1
                    for k in range(1,int((series_size+1)/2)):
                        A[j,2*k] = cos(2*pi*elite_level[j]*k/(series_size + (1 if i == 11 else 0)))
                        A[j,2*k-1] = sin(
                            2*pi*elite_level[j]*k/(series_size + (1 if i == 11 else 0)))
                    b[j] = elite_xp[j]
                    polynomial_degree = polynomials[i][2]
                    for k in range(polynomial_degree+1):
                        b[j] -= polynomials_coeff[i][polynomial_degree-k] * elite_level[j]**k
                fourier_coeff.append(solve(A,b))
            return polynomials, polynomials_coeff, fourier_coeff


        def xp_continous(elite_level: float, elite_coeff = power_fourier_series()) -> float:
            """Returns a float xp value for a float level value"""
            tier = 11
            while elite_coeff[0][tier][0] > elite_level:
                tier -= 1
            polynomial_degree = elite_coeff[0][tier][2]
            elite_xp = 0
            for i in range(polynomial_degree+1):
                elite_xp += elite_coeff[1][tier][polynomial_degree-i] * elite_level**i
            elite_xp += elite_coeff[2][tier][0]
            series_size = elite_coeff[0][tier][1]-elite_coeff[0][tier][0]+1
            for i in range(1,int((series_size+1)/2)):
                elite_xp += (
                    elite_coeff[2][tier][2*i]
                    *cos(2*pi*elite_level*i/(series_size + (1 if tier == 11 else 0))))
                elite_xp += (
                    elite_coeff[2][tier][2*i-1]
                    *sin(2*pi*elite_level*i/(series_size + (1 if tier == 11 else 0))))
            return elite_xp


    else:
        LEVEL = 127


        def xp_calc(level: int) -> int:
            """Returns an integer xp value for an integer level value"""
            xp = 0
            for i in range(1,level):
                xp += 0.25*((i + 300 * 2 ** (i / 7))//1)
            return xp // 1


        def xp_approx(level: float) -> float:
            """Returns an approximate float xp value for a float level value"""
            xp = 2**(level/7)
            xp -= 2**(1/7)
            xp *= 75
            xp /= 2**(1/7)-1
            xp += level*(level-1)/8
            return xp


        def power_fourier_series():
            """Returns the coeffiecents of the continous level to xp formula"""
            A = zeros((127,127))
            b = zeros((127))
            for i in range(127):
                A[i,0] = i
                for j in range(1,64):
                    A[i,2*j] = cos(2*pi*i*j/127)
                    A[i,2*j-1] = sin(2*pi*i*j/127)
                b[i] = xp_calc(i) - xp_approx(i)
            return solve(A,b)


        def xp_continous(level: float, coeff = power_fourier_series()) -> float:
            """Returns a float xp value for a float level value"""
            xp = xp_approx(level)
            xp += coeff[0] * level
            for i in range(1,64):
                xp += coeff[2*i]*cos(2*pi*level*i/127)
                xp += coeff[2*i-1]*sin(2*pi*level*i/127)
            return xp


    def level_continous(xp: float) -> float:
        """Returns a float level value for a float xp value"""
        level = LEVEL
        xp_tmp = xp_calc(level)
        while xp<xp_tmp:
            level -= 1
            xp_tmp = xp_calc(level)
        if xp_tmp == xp:
            return level
        def func(level):
            return xp_continous(level) - xp
        return min(max(level,fsolve(func,level)[0]),nextafter(level+1, float('-inf')))

    print(level_continous(args.xp))
