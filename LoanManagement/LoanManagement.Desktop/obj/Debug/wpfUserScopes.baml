   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=0.10.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    LoanManagement.Desktop*   $LoanManagement.Desktop.wpfUserScopes   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5        (   "MahApps.Metro.Controls.MetroWindow   Name$
  wdw1��5       � Title$ wpfUserScopes��5   	   $��454.752��6   $��432.571��60    � 
ResizeMode$ NoResize=�6U    � WindowStartupLocation$ CenterScreen=�6k    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $N Hpack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   %    5      .��5      � .+�5   
   �� $
��Left=�6   $	��301��60   $	��Top=�6=   $	��407��6U   $��
10,79,-4,0q�6a   .@�5      �� $��Maintenance��6   $��16(�6+   .��5      � ���	#FFE5E5E5$	��267��6.   $	��Top=�6;   $��0,0,0,-5q�6S    � ColumnDefinitions 5      �� 5      $��0* �6+   �� 5      $	��83* �6+   �� 5      $��9* �6+   �� 5      $
��107* �6+   �� 5      $
��107* �6+   .+�5      �� -   5         ��Name$ cClient��5      $��4��60   $��Client��6D   $
��Left=�6U   $��26��6p   $��	10,10,0,0q�6|   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5      -   5        $ 	cServices��5      $��4��62   $��Services��6F   $
��Left=�6Y   $��26��6t   $��	10,36,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5      -   5        $ cAgents��5      $��4��60   $��Agents��6D   $
��Left=�6U   $��26��6p   $��	10,62,0,0q�6|   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5       -   5        $ cBanks��5       $��4��6/   $��Banks��6C   $
��Left=�6S   $��26��6n   $��	10,88,0,0q�6z   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5!      -   5        $ 	cEmployee��5!      $��4��62   $��Employee��6F   $
��Left=�6Y   $��26��6t   $��
10,114,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5"      -   5        $ 	cPosition��5"      $��4��62   $��Position��6F   $
��Left=�6Y   $��26��6t   $��
10,140,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5#      -   5        $ cHoliday��5#      $��4��61   $��Holiday��6E   $
��Left=�6W   $��26��6r   $��
10,166,0,0q�6~   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5$      -	   5        $ 
cOnlineReg��5$      $��4��63   $��Registration Confirmation��6G   $
��Left=�6k   $��26��6�   $��
10,192,0,0q�6�   $	��Top=�6�   $	��241��6�   $��18(�6�   5%      5&      �� 5'      $��Transaction��6   $��16(�6+   $��
-2,-2,-2,0q�69   .��5(      � ���	#FFE5E5E5$	��267��6.   $	��Top=�6;   $��0,0,0,-5q�6S    5)      �� 5*      $��0* �6+   �� 5+      $	��83* �6+   �� 5,      $��9* �6+   �� 5-      $
��107* �6+   �� 5.      $
��107* �6+   .+�50      �� -
   5        $ cApplication��50      $��4��65   $��Application��6I   $
��Left=�6_   $��26��6z   $��	10,44,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 51      -   5        $ 	cApproval��51      $��4��62   $��Approval��6F   $
��Left=�6Y   $��26��6t   $��	10,70,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 52      -   5        $ 
cReleasing��52      $��4��63   $��	Releasing��6G   $
��Left=�6[   $��26��6v   $��	10,96,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 53      -   5        $ 	cPayments��53      $��4��62   $��Payments��6F   $
��Left=�6Y   $��26��6t   $��
10,122,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 54      $��Main:��6   $
��Left=�6,   $��31��6G   $��10,8,0,0q�6S   $	��Top=�6e   $	��117��6}   
���   �$��18(�6�   	 J� RenderTransformOrigin$	 0.511,-2.728&�6�   $��1��6�   $��3��6�   �� 55      $��Others(For Financing):��6   $
��Left=�6=   $��42��6X   $��
10,148,0,0q�6d   $	��Top=�6x   $	��202��6�   
���   �$��18(�6�   $	 0.511,-2.728&�6�   $��1��6�   $��3��6�   �� 56      -   5        $ cManageClosed��56      $��4��66   $��Manage Closed Accounts��6J   $
��Left=�6k   $��26��6�   $��
10,179,0,0q�6�   $	��Top=�6�   $	��230��6�   $��18(�6�   �� 57      -   5        $ cRestructure��57      $��4��65   $��Loan Restructure��6I   $
��Left=�6d   $��26��6   $��
10,205,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 58      -   5        $ cAdjustment��58      $��4��64   $��Payment Adjustment��6H   $
��Left=�6e   $��26��6�   $��
10,231,0,0q�6�   $	��Top=�6�   $	��202��6�   $��18(�6�   �� 59      -   5        $ 
cOnlineApp��59      $��2��63   $��Online App Confirmation��6G   $
��Left=�6i   $��26��6�   $��	34,44,0,0q�6�   $	��Top=�6�   $	��236��6�   $��18(�6�   $��3��6�   �� 5:      -   5        $ cCollection��5:      $��2��64   $��Collection Payment��6H   $
��Left=�6e   $��26��6�   $��	34,65,0,0q�6�   $	��Top=�6�   $	��236��6�   $��18(�6�   $��3��6�   �� 5;      -   5        $ cRenewal��5;      $��2��61   $��Renewal��6E   $
��Left=�6W   $��26��6r   $��	34,91,0,0q�6~   $	��Top=�6�   $	��166��6�   $��18(�6�   $��3��6�   5<      5=      �� 5>      $��Others��6   $��16(�6&   $��
-2,-2,-2,0q�64   .��5?      � ���	#FFE5E5E5$	��267��6.   $	��Top=�6;   $��0,0,0,-5q�6S    5@      �� 5A      $��0* �6+   �� 5B      $	��83* �6+   �� 5C      $��9* �6+   �� 5D      $
��107* �6+   �� 5E      $
��107* �6+   .+�5G      �� -   5        $ cArchive��5G      $��4��61   $��Archive��6E   $
��Left=�6W   $��26��6r   $��	10,10,0,0q�6~   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5H      -   5        $ cBackup��5H      $��4��60   $��Back Up & Restore��6D   $
��Left=�6d   $��26��6   $��	10,36,0,0q�6�   $	��Top=�6�   $	��181��6�   $��18(�6�   �� 5I      -   5        $ cUsers��5I      $��4��6/   $��User Accounts��6C   $
��Left=�6[   $��26��6v   $��	10,62,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5J      -   5        $ cReports��5J      $��4��61   $��Reports��6E   $
��Left=�6W   $��26��6r   $��	10,88,0,0q�6~   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5K      -   5        $ cStatistics��5K      $��4��64   $��View Statistics��6H   $
��Left=�6b   $��26��6}   $��
10,114,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5L      -   5        $ cUserScopes��5L      $��4��64   $��User Scopes��6H   $
��Left=�6^   $��26��6y   $��
10,140,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   �� 5M      -   5        $ cOnlineSettings��5M      $��4��68   $��Online Settings��6L   $
��Left=�6f   $��26��6�   $��
10,166,0,0q�6�   $	��Top=�6�   $	��166��6�   $��18(�6�   5N      5O      5P      � 5Q   
   $
��Left=�6   $��74��6*   $	��Top=�66   $	��988��6N   ���	#7F48CB00$��
0,0,-567,0q�6q   .+�5R      �� $
��Left=�6   $��74��6/   $	��Top=�6;   $��97��6S   $��Images/Lock.ico��6^   �� 5S      $��Level Of Access��6   $
��Left=�6.   $��54��6I   $��	87,10,0,0q�6U   $	��Top=�6h   $	��285��6�   $��30(�6�   5T      �� 5U   
   -   5        
 ��Name$
 btnCont��5U      $
��Left=�6"   $��36��6=   $��277,385,0,0q�6I   $	��Top=�6^   $	��140��6v   ���	#CC48CB00$��2q�6�   ���	#7F48CB00.��5V      ]� ��. .+�5W      �� -   5         ��Name$ btnContinue��5W      $��Center=�6-   $��Right=�6Q   ���	#FF343434$��31��6   $��16(�6�   $
��Save��6�   �� 5X      $��Images/Save.ico��6   5Y      5Z      5\      5]      