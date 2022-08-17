# -*- coding: utf-8 -*-
"""
Created on Sat Aug 13 18:15:03 2022

@author: brill
"""
import json

import matplotlib.pyplot
import numpy as np
import matplotlib.pyplot as plt
from matplotlib import cm


def import_color_map_dict(path):
    """
    Imports json resource that was exported from colorbrewer2.org and creates a dictionary containing
    the colors as value lists.

    Parameters
    ----------
        path: file path to json resource

    Returns
    -------
        Color dictionary of the form dict[color_scheme_key][number_of_data_classes]

    """

    # Load file and init dictionary
    file = open(path)
    data = json.load(file)
    color_map_dictionary = {}

    for color_map in data:

        print(color_map)

        # Create current colormap values
        color_sub_tree = data[color_map]
        color_map_colors_dict = {}
        for key in color_sub_tree.keys():

            # Skip type value for now
            if key == 'type':
                continue

            # Parse raw color values and map to number of data classes key
            dimension_color_string_list = color_sub_tree[key]
            dimension_colors = []
            for dimension_color_string in dimension_color_string_list:
                dimension_color_string = dimension_color_string[4:len(dimension_color_string) - 1]
                color_values = dimension_color_string.split(",")
                for i in range(0, len(color_values)):
                    if len(color_values[i]) == 0:
                        color_values[i] = "0"
                color = (int(color_values[0]), int(color_values[1]), int(color_values[2]))

                dimension_colors.append(color)

            # reverse imported list to set lowest value color at the beginning
            dimension_colors.reverse()

            # print(dimension_colors)

            color_map_colors_dict[key] = dimension_colors

        color_map_dictionary[color_map] = color_map_colors_dict

    return color_map_dictionary


# Global colormap dictionary
color_json_path = "C:/Repos/VRLab/ComputerOrientierteMathematik/Python/SkalareFelder/sfcalc/res/colorbrewer.json"  # "./res/colorbrewer.json"
cm_dict = import_color_map_dict(color_json_path)


def id(x, y):
    return x


def export_color_map_texture(cm, data_class=8):
    n = int(data_class)  # 8
    m = 2
    x = np.linspace(0, 1, n)
    y = np.linspace(0, 1, m)
    x, y = np.meshgrid(x, y)
    z = id(x, y)

    # print(z)

    mydpi = 1
    fig = plt.figure(figsize=(8 / mydpi, 2 / mydpi))
    fig.set_size_inches(15, 2)
    ax = plt.axes([0, 0, 1, 1])
    img = ax.imshow(z, interpolation='nearest', origin='lower')
    img.set_cmap(cm)  # 'BrBG')
    plt.axis('off')

    plt.savefig(cm + "_" + str(data_class) + ".png", format='png', bbox_inches='tight',
                dpi=mydpi,
                pad_inches=0)

    plt.close(fig)


if __name__ == "__main__":

    # print(cm_dict)

    for cm in cm_dict:
        for data_class in cm_dict[cm]:

            print("colorbrewer cm", cm, "dc", data_class)

            export_color_map_texture(cm, data_class)

    for cm in matplotlib.pyplot.colormaps():

        print("matplotlib cm", cm)

        export_color_map_texture(cm)
