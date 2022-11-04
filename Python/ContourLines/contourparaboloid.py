# -*- coding: utf-8 -*-
"""
Berechnung und Extraktion von Konturlinien
am Beispiel eines elliptischen Paraboloids

Der Code folgt dabei dem Stackoverflow-Eintrag

https://stackoverflow.com/questions/5666056/matplotlib-extracting-data-from-contour-lines
"""
import matplotlib.pyplot as plt
from matplotlib import cm
import numpy as np


def quadric(x, y, a=1.0, b=0.0, c=1.0, d=np.array([0, 0])):
    """ Quadratische Form als skalares Feld"""
    return a*x*x + 2.0*b*x*y + c*y*y + d[0]*x + d[1]*y


xmin = -2.5
xmax = 2.5
ymin = -2.5
ymax = 2.5

x = np.linspace(xmin, xmax, 100)
y = np.linspace(ymin, ymax, 100)
x, y = np.meshgrid(x, y)
# Kreisförmige Konturlinien fü a = c = 1, b = 0.
z = quadric(x, y)

Cmin = 1.0
Cmax = 5.1
Cstep = 1.0
levels = np.arange(Cmin, Cmax, Cstep)

fig = plt.figure(figsize=(16.0, 16.0))
plt.subplot(121)
cplot = plt.contour(x, y, z,
                    levels=levels,
                    cmap=cm.jet,
                    linewidths=3,
                    origin='lower',
                    extent=(xmin, xmax, ymin, ymax))

plt.xlabel('x')
plt.ylabel('y')
plt.title('$f(x, y) = x^2 +  y^2$',
          y=1.05, fontsize=24)
cbar = plt.colorbar(cplot, shrink=0.8, extend='both')
cbar.ax.get_yaxis().labelpad = 30
cbar.ax.set_ylabel('Funktionswerte', rotation=270, fontsize=18)

# In der Variable cplot können wir die Koordinaten der Konturlinien abfragen

# Abfraben, wie viele Konturwerte es gibt - Ergebnis steht in num_levels
num_levels = len(cplot.allsegs)
print('Anzahl der Konturwerte:', num_levels)

# Linie 1
points = cplot.allsegs[0][0]
p1x = points[:, 0]
p1y = points[:, 1]

# Linie 2
points = cplot.allsegs[1][0]
p2x = points[:, 0]
p2y = points[:, 1]

# Linie 3
points = cplot.allsegs[2][0]
p3x = points[:, 0]
p3y = points[:, 1]

# Linie 4
points = cplot.allsegs[3][0]
p4x = points[:, 0]
p4y = points[:, 1]

# Linie 5
points = cplot.allsegs[4][0]
p5x = points[:, 0]
p5y = points[:, 1]

# Als Test erstellen wir eine Grafik mit den Feldern p*x, p*y
plt.subplot(122)
plt.plot(p1x, p1y, 'k-')
plt.plot(p2x, p2y, 'k-')
plt.plot(p3x, p3y, 'k-')
plt.plot(p4x, p4y, 'r-')
plt.plot(p5x, p5y, 'r-')

plt.xlabel('x')
plt.ylabel('y')
plt.title('$f(x, y) = x^2 +  y^2$ - Abgespeicherte Linien',
          y=1.05, fontsize=24)
