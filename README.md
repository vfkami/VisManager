# VisManager

VisManager is a package for Unity that helps in the process of creating a visualization.  You be able able to carry out the main steps exposed in the Visualization Pipeline created by Ben Shneiderman in Readings in *Information Visualization: Using Vision to Think*.

# Usage

TODO: Write the Usage section

# API Reference

The API contains fundamental functions for creating a visualization, including reading the database, selecting, filtering, and mapping the attributes and rendering the visualization. Initially, the API reads files in `.csv` separated by `, ` only.

## DatasetReader.cs

Add in one GameObject to read a database and store the data.

**ReadFile(`string` path)** <br>
Call this function with a string parameter containing the path of the database preferably in `.csv` and separated by `, `.

**GetDataset()** <br> 
This function returns all the attributes present in the database. Each `string []` literally represents a row in the database.

**GetLabels()**<br>
This function returns a list of `string` containing the name (label) of each attribute in the database.

**GetTypes()**<br>
Although the information is stored in `string` type, this function returns a list of `Type` containing the type of each attribute in the database for better treatment.

**GetAllElementInformation(`string` element, `int` index)** <br>
Used when there is an attribute in the database that is unique (ID) for each element. Call with the position of this attribute and the content (ID), and it will return the complete line (`string[]`) corresponding to the element..

**GetDatabasePath()** <br>
Returns a `string` with the path where the database is stored


## FilterManager.cs

Preferably add to the same `GameObject` that already has `datasetreader.cs`;

**SetDatabase(`List<string[]>` originalDatabase, `List<Type>` attributeTypes)** <br>
Stores a copy of the current database to perform the filters.

**UpdateFilterListByIndex(`int` index, `List<string>` filteredParameters)** <br>
Adds a new filter. Each filter corresponds to only one attribute. The `index` of the attribute and a list of `string` of the attributes to be removed from the final database.

**DestroyFilterList()**<br>
Delete all previously added filters

**GetFilteredDatabase()**<br>
Returns a database with only the elements that have not been removed by the applied filters.

## ProjectUtils.cs

Contains a list of useful functions for the operation of the project

## Another Scripts in This Project
These are scripts created to manage the elements of `UIScene.unity` elements. Use your own scripts for managing 3D Objects, 2D and UI elements. Are they:
- *TgroupBehavior.cs;*
- *MinMaxSliderBehavior.cs;*
- *UIManager.cs;*
- *UIButtonBehavior.cs*


## Generating the Visualizations
For visualizations, we use an external component called [VizGen](https://github.com/tiagodavi70/VizGen) a web service in node.js that receives requests through the parameters sent by Unity and returns an image containing the visualization referring to the parameters sent.

## TODO: 
- Write the Usage section
- Finish ProjectUtils.cs description
- All Scripts on Editor
- Add multiple separators options
- Add multiple file extensions suport
- Create a `.unitypackage` for external usage
- Save/Export the final visualization

## Authors:
- [Vinicius Queiroz](viniciusqquei@gmail.com) (Bachelor of Information Systems at the Federal University of Pará)