   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=0.10.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    LoanManagement.Desktop&    LoanManagement.Desktop.wpfClient   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5        (   "MahApps.Metro.Controls.MetroWindow   Name$
  wdw1��5       � Title$ Clients��5   	   $��502.143��6   $��1053.722��6*    � 
ResizeMode$ NoResize=�6s    � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $N Hpack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $\ Vpack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml?�6%   %    5      .��5      � $��0,0,2,0q�6    � ColumnDefinitions 5   
   �� 5      �� 5      $��0* �6   .+�5   
   � $	��467��6   $��0,79,10,-73q�6   $	��Top=�61    J� RenderTransformOrigin$ 0.321,0.525&�6I    5      �� 5      $
��386* �6#   �� 5      $
��629* �6#   �� 5      $	��21* �6#   .+�5      &   System.Windows.Controls.DataGrid  -   5        	  Name$	 dgClient��5      $
��Left=�6)   $	��292��6D   $��
344,41,0,0q�6Q   $	��Top=�6e   $	��682��6}   
   CanUserAddRows
 .     CanUserDeleteRows .  $��18(�6�      GridLinesVisibility$
 None=�6�   $��3��6  ���	#CC48CB00$��2q�6<  ���	#7FF0F0F0 5        �� Color��%Z   (��       5!   &   * SystemColors.HighlightBrushKey+)�� 6   $ 	#CC48CB00��5"      5#      5$      �� 5%      $��Basic Information��6   $
��Left=�62   $	��367��6M   $��0,10,0,0q�6Z   $	��Top=�6l   $	��329��6�   $��3q�6�   .��5&      � $
��Left=�6   $	��315��62   $	��Top=�6?   $	��319��6W   $��0,0,-2,0q�6c   .+�5'      �� -   5         ��Name$	 img��5'      $
��Left=�6)   $	��100��6D   $	��Top=�6Q   $	��100��6i   $ 0.698,1.038&�6u   $��10,0,0,0q�6�   �� 5(      -   5         ��Name$ lblName��5(      $�� ��6-   $
��Left=�68   $��32��6S   $��
1,114,-4,0q�6_   $	��Top=�6s   $	��244��6�   $ 0.547,-0.275&�6�   $��Cambria)�6�   $��18(�6�   5)      5*      �� 5+      -   5         ��Name$ btnAdd��5+      $��2��6%   $
��Left=�69   $��39��6T   $��344,338,0,0q�6`   $	��Top=�6u   $	��164��6�   ���	#CC48CB00$��2q�6�   ���	#7F48CB00.��5,      ]� ��. .+�5-      �� -   5        $
 iLBL��5-      $��Center=�6*   $��Right=�6N   ���	#FF343434$��28��6|   $��16(�6�   $	��ADD��6�   �� 5.      $��Images/Add.ico��6   5/      50      �� 51      -   5        $ btnView��51      $
��Left=�6&   $��39��6A   $��127,338,0,0q�6M   $	��Top=�6b   $	��164��6z   $��1��6�   ���	#CC48CB00$��2q�6�   ���	#7F48CB00.��52      ]� ��. .+�53      �� $��Center=�6   $��Right=�6@   ���	#FF343434$��28��6n   $��16(�6z   $��VIEW/UPDATE��6�   �� 54      $��Images/View.ico��6   55      56      �� 57      -   5         ��Name$ 	txtSearch��57      $
��Left=�6)   $��14��6D   $��	47,10,0,0q�6P    �� TextWrapping$
 Wrap=�6c   $	��Top=�6w   $	��572��6�   $��1��6�   $��Segoe UI)�6�   6�   �� 58      $��2��6   $��Search:��6(   $
��Left=�6:   $��31��6U   $��
344,10,0,0q�6a   $	��Top=�6u   $��84��6�   $��18(�6�   59      � 5:   
   $
��Left=�6   $��74��6*   $	��Top=�66   $
��1076��6N   ���	#7F48CB00$��2��6r   $��	0,0,-30,0q�6�   .+�5;      �� $
��Left=�6   $��74��6/   $	��Top=�6;   $��97��6S   $��Images/Bank.ico��6^   �� 5<      -	   5        $ myLbL��5<      $��Client Maintenance��6#   $
��Left=�6@   $��54��6[   $��	87,10,0,0q�6g   $	��Top=�6z   $	��285��6�   $��30(�6�   5=      �� 5>   
   -
   5        $ btnRet��5>      $
��Left=�6!   $��39��6<   $��352,417,0,0q�6H   $	��Top=�6]   $	��164��6u   ���	#CC48CB00$��2q�6�   ���	#7F48CB00$y�Hidden=�6�   .��5?      ]� ��. .+�5@      �� -   5        $ lblAdd1��5@      $��Center=�6)   $��Right=�6M   ���	#FF343434$��28��6{   $��16(�6�   $��Retrieve��6�   �� 5A      $��Images/Retrieve.png��6   5B      5C      5E      5F      