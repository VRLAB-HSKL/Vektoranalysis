# -*- coding: utf-8 -*-
"""
Beispiel aus Stackoverflow für das Abfragen von Konturlinien
aus der Funktion contour in Matplotlib.

https://stackoverflow.com/questions/5666056/matplotlib-extracting-data-from-contour-lines

@author: brill
"""
import numpy as np
import matplotlib.pyplot as plt
from matplotlib import cm


x = np.array([1, 2, 3, 4])
y = np.array([1, 2, 3, 4])
z = np.array([[15, 14, 13, 12],
              [14, 12, 10, 8],
              [13, 10, 7, 4],
              [12, 8, 4, 0]])


levels = [9.5, 12.0]

fig = plt.figure(figsize=(16.0, 16.0))
cplot = plt.contour(x, y, z, levels=levels,
                    cmap=cm.terrain,
                    linewidths=3,
                    origin='lower',
                    extent=(1.0, 4.0, 1.0, 1.0))

# Jetzt die Koordinaten der Konturlinie abfragen
points = cplot.collections[0].get_paths()[0]
verts = points.vertices
x = verts[:, 0]
y = verts[:, 1]

print(x, y)

num_levels = len(cplot.allsegs)
print(num_levels)
num_elements = len(cplot.allsegs[0])
print(num_elements)
num_elements = len(cplot.allsegs[1])
print(num_elements)


# Segment 1
num_vertices = len(cplot.allsegs[0][0])
print("Anzahl der Punkte im Polygonzug zum Level 1")
print(num_vertices)
points = cplot.allsegs[0][0]
print(points[:, 0])
print(points[:, 1])

num_vertices = len(cplot.allsegs[0][0])
print(num_vertices)

# Segment 2
num_vertices = len(cplot.allsegs[1][0])
print("Anzahl der Punkte im Polygonzug zum Level 2")
print(num_vertices)
points = cplot.allsegs[1][0]
print(points[:, 0])
print(points[:, 1])

# Jetzt: f(x, y) = x^2 + y^2, Level sind die Radien von Kreisen,
# oder wir nehmen eine lineare Form, da kommen Geraden raus.
# Berechnen und zeichnen, abfragen und in einer weiteren Grafik
# die erhaltenen Polylines nochmal zeichnen.
