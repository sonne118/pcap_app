   M S B A M L   `   `   `  ����A  =wpfapp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:wpfapp.Controlswpfapp.Controls  5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   
   b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 5   	   3clr-namespace:wpfapp.ViewModelwpfapp.ViewModel  5
   	       wpfapp.MainWindow   5   	   &ctlsclr-namespace:wpfapp.Controls  5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   
   Cii:http://schemas.microsoft.com/expression/2010/interactivity  5   	   WcmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	   	   *localclr-namespace:wpfapp.ViewModel  5
   	   -   5        $
��home��5        � Title$  
MainWindow��5   	   $	��650��6   $
��1000��6)    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$
 None=�6.   
������  � AllowsTransparency$
 True��5   "    � 
ResizeMode$ NoResize=�5   	   $
��PCAP)�6    � DataContext 5   	   �� *Main+   ViewModelLocator �� Source# [  .��5      ��   PrimaryBackgroundColor#���  6    �� CornerRadius$ 8��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      �� $��0��6     SecundaryBackgroundColor#���  5      $ 8 0 0 8��5      ._�5      � -   5        	 �Name$	 GridNav��5      $��60��6(   
 � RowDefinitions
 5      �� 5       $��60 �6(   �� 5!      $��* �6(   .+�5$      � $��0��6   .+�5%      �� $��14 0 0 0q�5&      $��32��5'      $��24��6*   $
��Left=�5(      5)      �� 5+      $
��PCAP��6      PrimaryTextColor#���  5,       $
��Bold"�5-       $��16(�5.       $��Center=�5/       $
��Left=�50       $��60 0 0 0q�51       52      �� 55      $��1��6!   $��
-1 40 0 20q�6.   .+�56      �� -   5         ��Name$ btnHome��56   !     PopupButtonStyle#���  6R   ��62   �� *HomeCommand+.��57         wpfapp.Controls.MenuItem     	GroupName$ MenuItem��6,     PrimaryBlueColor   IndicatorBrush# �  6A     home   Icon# �  6u      	IconWidth$ 16��6�      Text$
 Home��6�   $��Center=�6�   .  (System.Windows.Interactivity.Interaction   Triggers 58      /  )System.Windows.Interactivity.EventTrigger  59   "      	EventName$ Click��61   .  (System.Windows.Interactivity.TriggerBase   Actions. 5:   &   /  )GalaSoft.MvvmLight.Command.EventToCommand     Command 69   ��  �� Path$ HomeCommand�5;   #   5=      �� 5>      -   5        $ btnDashboard��5>   !   #���  6\   ��67   �� *DashboardCommand+.��5?        $ MenuItem��6,   # �  6A     	dashboard# �  6u   $ 16��6�   $ 	Dashboard��6�   $��Center=�6�    5@        5A   "   $ Click��61   . 5B   &      69   �� $ DashboardCommand�5C   #   5E      �� 5F      -   5        $ btnProducts��5F   !   #���  66   .��5G        $ MenuItem��6,   # �  6A     products# �  6u   $ 16��6�   $ Products��6�   $��Center=�6�    5H        5I   "   $ Click��61   . 5J   &      69   �� $ TreeCommand�5K   #   5M      �� 5N      -   5        $ 
btnSetting��5N   !   #���  65   .��5O        $ MenuItem��6,   # �  6A    	 settings# � 	 6u   $ 16��6�   $ Setting��6�   $��Center=�6�    5P        5Q   "   $ Click��61   . 5R   &      69   �� $ SettingsCommand�5S   #   5U      5V      � 5Z      -   5         �Name$ Popup��5Z       � AllowsTransparency$
 True��6,    � HorizontalOffset$ 0��6G    � VerticalOffset$ 10��6\   .��5[         wpfapp.Controls.UserPopup  -   5          Name$ Header��5[   )   #���  69   5\      5_      5`      � 5b      $��1��6   
 5c      �� 5d      $��60 �6$   �� 5e      $��* �6$   .+�5g      �� $
��Left=�6   $	��Top=�69   .+�5i      d� -	   5         d�Name$ Tg_Btn��5i   #   $��1��63   $��Center=�6C    
 iToggleButtonStyle#��� 
 6^   $
��Left=�6�   $��0q�6�   
������ $��
10,15,0,15q�6�   $��30��6�   $��30��6�    � Triggers 5j      :� 5k       :� RoutedEvent$ ToggleButton.Unchecked��6+   .[�5l   "   �� .��5m   &   �� -
   5         ��Name$ HideStackPanel��5m   1   .��5n   *   W�  �� 
TargetName$ GridNav��6:     �� TargetProperty$  Width�6Z   ! g� 	BeginTime	! 0:0:06|   " W� From" 2106�   	# W� To# 606�   $ g� Duration$$ 0:0:0.5D�6�   5o   +   5p   '   5q   #   5r      :� 5s      $ ToggleButton.Checked��6+   .[�5t   "   �� .��5u   &   �� -   5        $ ShowStackPanel��5u   1   .��5v   *   W� $ GridNav��6:   $  Width�6Z   	! 0:0:06|   " 606�   # 2106�   $$ 0:0:0.5D�6�   5w   +   5x   '   5y   #   5z      5|      5~      �� 5�      $��0��6   $
��Left=�6+   $	��Top=�6G   $��
250,5,10,0q�6`   $��40��6t   % �� Orientation$% 
Horizontal=�6�   $��LeftToRight=�6�   $��2��6�   .+�5�      �� -   5        & ��Name$& cDevice��5�        ComboBoxFlatStyle#��[ 60   $	��300��6�   $��26��6�   ' J� RenderTransformOrigin$' 0.596,0.774&�6�   ��6[   �� *Items+( �� SelectedItem( 6y   �� *_SelectedItem+6�   5�      �� 5�      $��0��6   $��Right=�6*   $	��Top=�6F   $��0,5,10,0q�6^   $��30��6p   $% 
Horizontal=�6|   $��RightToLeft=�6�   $��2��6�   .+�5�      �� -   5        $ btnClose��5�        IconButtonsStyle#��[ 6/    	 close#���  6Y   
) � Tag$) IsCloseButton��6�   �� 5�      -   5        $ 
btnRestore��5�      $	��5,0q�61   ��5�      �� 5�      * �� 
TargetType"* ��6$   + �� BasedOn#+ [ 6A   .�5�   "   �� , �� Property, � ��  maximize- �� Value#- �  6<   . �� Triggers. 5�   "   �� 5�   &   / �� Binding/ 62   �� 0 �� ElementName$
0 home��$ WindowState�1 �� Value1 	Maximized6i   .a�5�   *   �� , � ��  restore#- �  6D   5�   '   5�      5�      �� 5�      -   5        $ btnMinimize��5�      #���  62     minimize#���  6]   2 �� IsCancel$
2 True��6�   5�       � 5�      -   5        $��
fContainer��5�      $��1��5�      $��20q�5�      3  � NavigationUIVisibility$3 Hidden=�5�      ��5�   &   �� *CurrentView+5�      5�      5�      5�      5�      