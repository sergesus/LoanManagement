   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=0.10.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    LoanManagement.Desktop+   %LoanManagement.Desktop.wpfRenewClosed   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   -   5        (   "MahApps.Metro.Controls.MetroWindow   Name$
  wdw1��5       � Title$ wpfRenewClosed��5   	   $��664.993��6    $��426.316��61    � 
ResizeMode$ NoResize=�6V    � WindowStartupLocation$ CenterScreen=�6l    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $N Hpack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $\ Vpack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml?�6%   %    5      .��5      � $��0,68,0,2q�6   .+�5   
   �� $
��Left=�6   $	��389��62   $��
10,124,0,0q�6?   $	��Top=�6S   $	��399��6k   $
��Auto=�6w   ��. .��5      �� -   5         ��Name$
 stck��5       �� Orientation$ Vertical=�6'   $	��394��6>   $	��339��6K   5      �� 5   
   -   5        	 ��Name$	 cmbBank��5      $
��Left=�6$   $��32��6?   $��
145,10,0,0q�6K   $	��Top=�6_   $	��264��6w   $��18(�6�   �� 5   
   $!��Remaining Cheques to Renew:��6   $
��Left=�66   $	��Top=�6Q   $��	10,88,0,0q�6i   $	��240��6|   $��16(�6�   ���	#CC48CB00$
��Bold"�6�   
 J� RenderTransformOrigin$
 
1.107,0.71&�6�   �� 5   
   $��Bank:��6   $
��Left=�6    $	��Top=�6;   $��	10,11,0,0q�6S   $	��135��6f   $��16(�6r   ���	#CC48CB00$
��Bold"�6�   $
 
1.107,0.71&�6�   �� 5   
   $��First Due Date:��6   $
��Left=�6*   $	��Top=�6E   $��	10,47,0,0q�6]   $	��135��6p   $��16(�6|   ���	#CC48CB00$
��Bold"�6�   $
 
1.107,0.71&�6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dtPcker��5      $
��Left=�6&   $��36��6A   $��
145,47,0,0q�6M   $	��Top=�6a   $	��263��6y   $��18(�6�   � 5   
   $
��Left=�6   $��74��6*   $	��Top=�66   $	��479��6N   ���	#7F48CB00$��-4,-69,-55,0q�6q   .+�5      �� $
��Left=�6   $��74��6/   $	��Top=�6;   $��97��6S   $��Images/Renew.ico��6^   �� 5      $��Closed Account Renewal��6   $
��Left=�65   $��54��6P   $��	87,10,0,0q�6\   $	��Top=�6o   $	��340��6�   $��30(�6�   5       �� 5!   
   -   5         ��Name$ btnRenew��5!      $
��Left=�6#   $��40��6>   $��244,518,0,0q�6J   $	��Top=�6_   $	��164��6w   ���	#CC48CB00$��2q�6�   ���	#7F48CB00.��5"      ]� ��. .+�5#      �� $��Center=�6   $��Right=�6<   ���	#FF343434$��31��6j   $��16(�6v   $��Renew��6�   �� 5$      $��Images/Renew.ico��6   5%      5&      5(      5)      