   M S B A M L   `   `   `  ����F  BPakExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PakExplorerPakExplorer  5   	       PakExplorer.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PakExplorer     5   	     � Title$  PakXStractor��5   	   $	��450��6   $	��800��6+   .��5	      ]� .+�5
   
   o� $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� -   5        $��	_Open Pak��5      6.   �� 5      m� 5      -   5        $��_Quit��5      6*   5      5      �� 5   
    �� Orientation$ 
Horizontal=�6   $	��120��6.   $
��Left=�6=   .+�5      �� -   5         ��Name$ 
lstPakFile��5   &   $	��120��6    �� View 5      �� 5      .P�5      �� 5      5      �� 5      -   5        $ 
lstWadFile��5   &   $	��120��6   $y�	Collapsed=�6e    5      �� 5      .P�5      �� 5      5      5       �� 5!   
   $ Vertical=�6   .+�5"      �� $ Vertical=�6   .+�5#      �� -   5         ��Name$ lblFilename��5#      $��
Filename: ��6-   $y�	Collapsed=�6B   5$      �� 5%      -   5         ��Name$ 
pnlPalette��5%      $ Vertical=�6-   $y�	Collapsed=�6D   .+�5&      �� -   5        $ 
lblPalette��5&      $��Palette loading...��6,   5'      �� 5(      -   5        $ pnlLoadPalette��5(      $ Vertical=�61   $y�	Collapsed=�6H   .+�5)      �� -	   5        $��Find Palette in Pak...��5)      $��Center=�6:   $��10q�6t   5*      �� 5+      $ 
Horizontal=�6   $��Stretch=�62   $	��Top=�6P   $	��380��6h   .+�5,      �� -
   5        $ pnlImage��5,      $ 
Horizontal=�6/   $y�	Collapsed=�6H   $
��Left=�6_   .+�5-      �� $ Vertical=�6!   $
��Left=�68   $	��200��6S   .+�5.      �� -   5        $ pnlModelTextures��5.   V   $ 
Horizontal=�6%   $	��Top=�6>   $y�	Collapsed=�6p   .+�5/      �� $��
Textures: ��6$   6;   �� 50      -   5         ��Name$ cmbModelTextures��50   '   $	��120��6A   6P   51      �� 52      $ 
Horizontal=�6%   $	��Top=�6>   .+�53      �� $��Width: ��6$   �� 54      -   5         ��Name$ txtWidth��54   &   $��Center=�68   $	��100��6S   55      �� 56      $ 
Horizontal=�6%   $	��Top=�6>   .+�57      �� $��Height: ��6$   �� 58      -   5        $ 	txtHeight��58   &   $��Center=�69   $	��100��6T   59      �� 5:      -   5        $ pnlMdlDetails��5:   V   $ 
Horizontal=�6%   $	��Top=�6>   $y�	Collapsed=�6m   .+�5;      �� -   5        $ lblVerts��5;   9   $��
Vertices: ��6$   69   �� 5<      -   5        $ 	lblFrames��5<   7   $��Frames: ��6$   67   5=      �� 5?      $ 
Horizontal=�6%   $��Bottom=�6>   $��Center=�6Y   .+�5@      �� -   5         ��Name$ btnExportGfx��5@   %   $��Export Graphic��6;   $��Bottom=�6T   6o   �� 5A      -   5        $ btnExportModelToObj��5A   %   $��
Export Obj��6B   $��Bottom=�6W   $y�	Collapsed=�6�   5B      5C      �� 5D      $ 
Horizontal=�6!   $��Right=�6:   .+�5E      �� -   5        	 ��Name$	 iImage��5E       $	��200��60   $	��200��6?   $	��400��6O   $	��400��6^   6p   5F      5G      �� 5H      -   5        $ pnlMap��5H      $ 
Horizontal=�6-   $
��Left=�6F   $y�	Collapsed=�6a   .+�5I      �� -   5        $��Export Map Textures��5I      6;   �� 5J      -   5        $��Export Entities as Json��5J      6?   �� 5K      -   5        $��Export Map to .Obj��5K      6:   5L      5M      5N      5O      5P      