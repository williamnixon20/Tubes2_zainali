   M S B A M L   `   `   `  ���� U  QSystem.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a7,clr-namespace:System;assembly=System.RuntimeSystem  I ETubes2_zainali, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Tubes2_zainaliTubes2_zainali    Tubes2_zainali.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?system,clr-namespace:System;assembly=System.Runtime     .localclr-namespace:Tubes2_zainali     $	��720��$
��1280��.���� ���#272537  �� CornerRadius$  20��._��  � ColumnDefinitions �� $	��250 ��� .+��� .+�r� -    r�Name$ myMediaElement�� r� Volume$ 10Q� r� IsMuted$ False�� r� LoadedBehavior$ Manual=� r� UnloadedBehavior$ Manual=� r� Source$ .\assets\zainali.mp3?��� $��在哪里 ~ Zài Nǎ Lǐ��$��Center=�$
��Left=�$��20q�
�������$��22(��� $��Choose Mode: ��$��Center=�$
��Left=�$��10,10,10,10q�
�������$��22(�	� -   $	��DFS�� d� 	IsChecked$
 TrueY�$��50��
�������$��14(�   
RadioTheme#��[  	� -   $	��BFS��$��50��
�������$��14(�#��[  �� -   $��TSP Enabled��$��50��
�������$��14(�  ButtonTheme#��[ �� -   $��Pruning Enabled��$��50��
�������$
 TrueY�$��14(�#��[ �� $��File Path: ��$��Center=�$
��Left=�$��10,10,10,10q�
�������$��22(��� -   $��	fileInput��$	��200��$��40��$��Center=�$
��Left=�$��10q�$��1��  TextBoxTheme#��[ �� -   $��btnOpenFile��$	��200��$��40��$��Center=�$
��Left=�$��10q�
���   �
�������$��1��$��File Explorer��	 � 	Resources	 
 �� 
TargetType �� Property �� Value��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7���� -   $��btnVis��$	��200��$��40��$��Center=�$
��Left=�$��
10,25,0,25q�
���   �
�������$��1��$��	Visualize��	 ��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7���� -	   $��buttonSearch��$	��200��$��40��$��Center=�$
��Left=�$��10q�
���   �
�������$��1��$��Search��$y�Hidden=�	 ��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7��� $	��720��$
��1030��$��1��.+��� -
    ��Name$ Board��$	��500��$	��500��$��143,97,387,123q����� .R�A� $��GridU��
������  A� Rows �� *NRows+ A� Columns �� *NCols+���� .R��� 
���   �$��1q�$	��0.5q�$  1������ *	ColorTile+
 � Tag �� ._��� $��2q� �� StretchDirection$ DownOnly=����� *	CellImage+�� -   $��btnNext��$	��200��$��40��$��Center=�$��Right=�$��0,250,96,430q�
���   �$y�Hidden=�
�������$
��Next��	 ��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7���� -   $��btnPrev��$	��200��$��40��$��Center=�$
��Left=�$��734,316,0,364q�
���   �$y�Hidden=�
�������$
��Prev��	 ��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7���� -   $��btnAuto��$	��200��$��40��$��Center=�$
��Left=�$��734,442,0,238q�
���   �$y�Hidden=�
�������$��	Auto: Off��	 ��%9   '��       �� "
 ��.���  �@��CornerRadius$ 7���� -   $��steps��$��Steps:��
�������$y�Hidden=�$��20(�$
��Wrap=�$��50��$
��1030��$��8,26,-8,644q��� -   $��numSteps��$��
Num Steps:��
�������$y�Hidden=�$��50��$	��300��$��20(�$��720,647,10,23q��� -   $��numNodes��$��
Num Nodes:��
�������$y�Hidden=�$��50��$	��300��$��20(�$��8,646,722,24q��� -   $
��time��$��Time:��
�������$��50��$y�Hidden=�$	��300��$��20(�$��365,646,365,24q�