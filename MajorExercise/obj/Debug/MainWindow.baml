   M S B A M L   `   `   `  ����H  DMajorExercise, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:MajorExerciseMajorExercise  5   	       MajorExercise.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:MajorExercise     5   	     � Title$  
MainWindow��5   	   $	��750��6   $
��1128��6)    � Icon$ /IconApp.png��5	   	    � ShowInTaskbar . .��5      � $��	2,54,0,44q�6    � ColumnDefinitions 5   
   �� 5      $
��265* �6   �� 5      $
��167* �6   .+�5   
   � $��0��6   
����ح�.+�5      �� $��	Thumbnail��6   $
��Left=�6(   $	��Top=�6C   $��16(�6\   �� 5      $
��Auto=�6   $��0,44,0,0q�6>   .��5      �� -   5        $��ImageItemsControl��5      ��5      �� 5      .R�5      � 5      ��5      �� 5      .R�5      ��  ��Name$ ImageBorder��6p   
�������$��1q�68   $��5q�6L   
������ ._�5   "   �� -   5        $	��150��5   ;   $	��150��6G   $��Uniform=�6T   ��6(   �� 6f   5       �� Triggers 5       P� 5!   "    P� 
SourceName$ ImageBorder��6*    P� Property �@J�IsMouseOver P� Value . . �5"   &   �� 	 �� 
TargetName$	 ImageBorder��6-   
 �� Property
 � �� �� Value �#c5cad86\   5$   #   5&      5(      5)      5*      � 5-   
   $��1��6   
������.+�5.      �� $��
Image View��6   $
��Left=�6)   $	��Top=�6D   $��16(�6]   �� 5/      -   5        $��btnStart��5/      $��Start��6%   $
��Left=�65   $	��Top=�6P   $��71��6h   $��30��6�   $��0,38,0,0q�6�   �� 50      -   5        $��	ImageView��50      $��Uniform=�6%   $��	0,75,5,85q�67   �� 51      -   5        $��lblImageInfo��51      $	��123��6(   $
��Left=�66   $��Bottom=�6Q   $	��148��6l   $��32��6x   $��16(�6�   $��2,0,0,31q�6�   �� 52      -   5        $��lblImageSize��52      $	��456��6(   $
��Left=�66   $��Bottom=�6Q   $	��148��6l   $��31��6x   $��16(�6�   $��2,0,0,1q�6�   �� 53      -   5        $��unfavoriteImage��53      $
��None=�6+   $��	/star.png��6:   $��Bottom=�6M   $��Right=�6h   $��0,0,9,5q�6�   6�   �� 54      -   5        $��favoriteImage��54      $
��None=�6)   $��/star color.png��68   $��Bottom=�6Q   $��Right=�6l   $��0,0,9,5q�6�   $y�Hidden=�6�   6�   55      �� 56   
   $��0��6   $��5��6'   
�������$��Right=�6C   �� 57   
   $��2��6   $��	2,0,7,-44q�6)   $��Bottom=�6<    �� Orientation$ 
Horizontal=�6W   $��43��6p   .+�58      �� -	   5        $��
ImageFound��58      $��Image Found:��6&   $��16(�6=   $��Center=�6K   $	��153��6f   �� 59      -
   5        $��lbFavoriteImages��59      $��Favorite Images:��6,   $��16(�6G   $��Center=�6U   $	��165��6p   �� 5:      -   5        $��CurrentStatus��5:      $��Current Status:��6+   $��16(�6E   $��Center=�6S   $	��150��6n   5;      5<      5=      