# PakExplorer/PakXStractor

## A Quake Pak Editing Tool
### Introduction
PakXStractor is a tool to be able to extract Quake content files from Paks and be able to edit them with modern tools such as Photoshop or Blender.
It was originally developed for a "boomer shooter" game engine I had been developing and was trying reconstruct Quake by extracting the files and
converting them into either proprietary or modern file formats. I've given up on the game engine idea but I still am finding the tool particularly
useful so I am continuing to develop it and hopefully it will eventually be of some use to the people out there who are still modding Quake.

### Features
- Lists all files within a pak.
- If a pak contains a palette it automatically loads it. If not when a user tries to load something that requires a palette it will allow them to pick the pak file with the palette before continuing with loading.
- Allows export of .lmp and .wad files to .png.
- Allows export of mdl textures to .png
- Allows export of all mdl frames to .obj files
- Can export all used textures from BSP files to .png.
- Can export the bsp entities along with properties to a .Json file.
  
### Todo
#### Immediately
- Fully support exporting BSP maps to .obj files (along with proper UV coordinates)
- Add support to export .Spr files as .png files.
- Add support to extract and play .Wav files.

#### Soon
- Add model viewer for .mdl and BSPs
- Animate animated textures

#### Down the Road
- Allow for writing to .pak files or creating new ones from scratch
- Convert a series of .obj files to an .mdl file (so long as they all have the same number of vertices)
- Convert .pngs to .lmp, .wads, or .sprs or add as a texture to an .mdl
- Import .wavs
- Support Quake2/Valve paks/files as well
