   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=0.10.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    LoanManagement.Desktop-   'LoanManagement.Desktop.wpfSelectPayment   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5        (   "MahApps.Metro.Controls.MetroWindow   Name$
  wdw1��5       � Title$ Select Payment��5   	   $��350.062��6    $��315.789��61    � 
ResizeMode$ NoResize=�6A    � WindowStartupLocation$ CenterScreen=�6W    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $N Hpack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $\ Vpack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml?�6%   %    5      .��5      � .+�5   
   �� -   5         ��Name$ 
btnDeposit��5      $
��Left=�6%   $��46��6@   $��	11,12,0,0q�6L   $	��Top=�6_   $	��289��6w   ���	#FF4AEE0D.��5      ]� ��. .+�5      �� -   5         ��Name$
 lbl1��5      $��Center=�6&   $��Right=�6J   ���	#FF343434$��32��6x   $��16(�6�   $��Deposit Cheques��6�   �� 5      $��Images/Deposit.ico��6   5      5      �� 5   
   -   5        $ btnHold��5      $
��Left=�6"   $��46��6=   $��	11,63,0,0q�6I   $	��Top=�6\   $	��289��6t   ���	#FF4AEE0D.��5      ]� ��. .+�5      �� -   5        $
 lbl2��5      $��Center=�6&   $��Right=�6J   ���	#FF343434$��33��6x   $��16(�6�   $��Hold/Unhold Cheque(s)��6�   �� 5      $��Images/Hold.ico��6   5      5       �� 5!   
   -   5        $ btnClear��5!      $
��Left=�6#   $��46��6>   $��
11,114,0,0q�6J   $	��Top=�6^   $	��289��6v   ���	#FF4AEE0D.��5"      ]� ��. .+�5#      �� -   5        $
 lbl3��5#      $��Center=�6&   $��Right=�6J   ���	#FF343434$��34��6x   $��16(�6�   $��Clear Cheque(s)��6�   �� 5$      $��Images/Clear.ico��6   5%      5&      �� 5'   
   -   5        $ 	btnReturn��5'      $
��Left=�6$   $��46��6?   $��
11,165,0,0q�6K   $	��Top=�6_   $	��289��6w   ���	#FF4AEE0D.��5(      ]� ��. .+�5)      �� -	   5        $
 lbl4��5)      $��Center=�6&   $��Right=�6J   ���	#FF343434$��31��6x   $��16(�6�   $��Return Cheque(s)��6�   �� 5*      $��Images/Return.ico��6   5+      5,      �� 5-   
   -
   5        $ btnVoid��5-      $
��Left=�6"   $��46��6=   $��
11,216,0,0q�6I   $	��Top=�6]   $	��289��6u   ���	#FF4AEE0D.��5.      ]� ��. .+�5/      �� -   5        $
 lbl5��5/      $��Center=�6&   $��Right=�6J   ���	#FF343434$��31��6x   $��16(�6�   $��Void Payments��6�   �� 50      $��Images/Void.ico��6   51      52      �� 53   
   -   5        $ btnFullPayment��53      $
��Left=�6)   $��46��6D   $��
11,267,0,0q�6P   $	��Top=�6d   $	��289��6|   ���	#FF4AEE0D.��54      ]� ��. .+�55      �� -   5        $
 lbl6��55      $��Center=�6&   $��Right=�6J   ���	#FF343434$��31��6x   $��16(�6�   $��Full Payment��6�   �� 56      $��Images/Refresh.ico��6   57      58      5:      5;      