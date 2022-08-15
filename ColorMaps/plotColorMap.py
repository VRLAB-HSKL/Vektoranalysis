# -*- coding: utf-8 -*-
"""
Created on Sat Aug 13 18:15:03 2022

@author: brill
"""
import numpy as np
import matplotlib.pyplot as plt
from matplotlib import cm


def id(x, y):
    return x


n = 8
m = 2
x = np.linspace(0, 1, n)
y = np.linspace(0, 1, m)
x, y = np.meshgrid(x, y)
z = id(x, y)

print(z)

mydpi = 1
fig = plt.figure(figsize=(8/mydpi, 2/mydpi))
fig.set_size_inches(15, 2)
ax = plt.axes([0, 0, 1, 1])
img = ax.imshow(z, interpolation='nearest', origin='lower')
img.set_cmap('BrBG')
plt.axis('off')

plt.savefig("BrBG.png", format='png', bbox_inches='tight',
            dpi=mydpi,
            pad_inches=0)
