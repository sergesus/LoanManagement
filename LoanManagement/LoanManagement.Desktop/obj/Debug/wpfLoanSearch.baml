   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=0.10.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    LoanManagement.Desktop*   $LoanManagement.Desktop.wpfLoanSearch   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5        (   "MahApps.Metro.Controls.MetroWindow   Name$
  wdw1��5       � Title$ wpfLoanSearch��5   	   $��484.564��6   $��705.553��60    � 
ResizeMode$ NoResize=�6U    � WindowStartupLocation$ CenterScreen=�6k    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $N Hpack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $\ Vpack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��371* �6   �� 5      $
��144* �6   .+�5   
   � $
��Left=�6   $��74��6*   $	��Top=�66   $	��863��6N   ���	#7F48CB00$��2��6q   $��
0,0,-163,0q�6�   .+�5      �� $
��Left=�6   $��74��6/   $	��Top=�6;   $��97��6S   $��Images/Search.ico��6^   �� 5      $��Loan Search��6   $
��Left=�6*   $��54��6E   $��	87,10,0,0q�6Q   $	��Top=�6d   $	��285��6|   $��30(�6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgLoan��5      $
��Left=�6#   $	��287��6>   $��
10,115,0,0q�6K   $	��Top=�6_   $	��682��6w   	   CanUserAddRows	 .  
   CanUserDeleteRows
 .  $��18(�6�      GridLinesVisibility$
 None=�6�   $��2��6�   ���	#CC48CB00$��2q�6	  ���	#7FF0F0F0 5       �� Color��%Z   (��       5   "   * SystemColors.HighlightBrushKey+)�� 6   $ 	#CC48CB00��5       5!      5"      �� 5#   
   -   5         ��Name$ btnView��5#      $
��Left=�6"   $��39��6=   $��
22,407,0,0q�6I   $	��Top=�6]   $	��164��6u   ���	#CC48CB00$��2q�6�   ���	#7F48CB00$��1��6�   .��5$      ]� ��. .+�5%      �� $��Center=�6   $��Right=�6<   ���	#FF343434$��28��6j   $��16(�6v   $��Select��6�   �� 5&      $��Images/View.ico��6   5'      5(      �� 5)   
   -   5         ��Name$ 	txtSearch��5)      $��2��6%   $
��Left=�69   $��31��6T   $��	79,79,0,0q�6`    �� TextWrapping$
 Wrap=�6s   $	��Top=�6�   $	��611��6�   $��Segoe UI)�6�   $��18(�6�   6�   �� 5*   
   $��Search:��6   $
��Left=�6"   $��54��6=   $��	10,74,0,0q�6I   $	��Top=�6\   $��69��6t   $��18(�6   �� 5+   
   -   5        $ btnViewFolder��5+      $
��Left=�6(   $��39��6C   $��357,407,0,0q�6O   $	��Top=�6d   $	��164��6|   ���	#CC48CB00$��2q�6�   ���	#7F48CB00$��2��6�   .��5,      ]� ��. .+�5-      �� $��Center=�6   $��Right=�6<   ���	#FF343434$��28��6j   $��16(�6v   $��View Folder��6�   �� 5.      $��Images/View.ico��6   5/      50      52      53      