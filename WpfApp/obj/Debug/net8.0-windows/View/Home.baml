   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5      b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 5      A =wpfapp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:wpfapp.ViewModelwpfapp.ViewModel 5
      3clr-namespace:wpfapp.ViewModelwpfapp.ViewModel 5         wpfapp.View.Home   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5      ]cmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform     5      1local1clr-namespace:wpfapp.ViewModel     5
      4	ViewModelclr-namespace:wpfapp.ViewModel     5         
Page_Style#��[  5      .��5      ��   �� CornerRadius$  5��6     SecundaryBackgroundColor#���  6   ._�5   
   %  wpfapp.ViewModel.ScrollDataGrid  &   System.Windows.Controls.DataGrid   RowBackground
 �����$��Center=�67   $   HorizontalScrollBarVisibility$ Disabled=�6Y      AutoGenerateColumns$ False��6�      HeadersVisibility$ Column=�6�      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�     ScrollDataGridRow#��[ 6�      
IsReadOnly$
 True��6�   $��
0,25,0,160q�6b  $��2��6w  $��2��6�     SelectionMode$ Single=�5         SelectionUnit$ FullRow=�6   $��4��66   	 �� SelectedItem	 5     �� *SelectedRow+
 �� Mode$
 TwoWay=���6;  �� *_StreamingData+.   (System.Windows.Interactivity.Interaction   Triggers 5      /   )System.Windows.Interactivity.EventTrigger  5         	EventName$ MouseDoubleClick��6!   .   (System.Windows.Interactivity.TriggerBase   Actions. 5      /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand$
 True��6_      Command 6)   ��  �� Path$" OnDataGridDoubleClickCommand�   CommandParameter 5      `� 5        MultiValueConverter `� 	Converter# [ 6+   .1�5   "   �� �� 5   "   $ SelectedItem�6p    �� RelativeSource 6*   ��  �� AncestorType local1:ScrollDataGrid6p   5      5      5       � 	Resources 5       �� 
TargetType �� Property �� Value)  #System.Windows.Controls.DataGridRow:	  4System.Windows.Controls.Primitives.DataGridRowHeader=
  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
ResourceId �� TypeInTargetAssembly��%�  '        '  J     '	  �     '
  �     (�� (    5+   5   $" DataGridSelectAllButtonStyle��"  )�� 5      "  6   .�5       ��  � ��
 � � �63   5!      �� 5"      "  6   .�5#      ��  � ��
 ��  �63   5$      �� 5%      " 	 6   .�5&      ��  � ��
 � ���63   5'      �� 5(      " 
 6   .�5)      ��  � ��
 �����63   5*      �� 5+      " ��6   .�5,      ��  � ��
 �   �63   5-      5.         Columns 5/      0  *System.Windows.Controls.DataGridTextColumn  50      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6&      Width$ 40 63   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *Id+  51      $ 	Source ip��6%   $ 80 69    6D   �� *	Source_ip+  52      $
 port��6%   $ 40 63    6>   �� *Source_port+  53      $ Dest ip��6%   $ 80 66    6A   �� *	Dest_ip+  54      $
 port��6%   $ 40 63    6>   �� *	Dest_port+  55      $ 
Source mac��6%   $	 100 69    6E   �� *
Source_mac+  56      $	 Pro��6%   $ 35 62    6=   �� *Proto+  57      $ Dest mac��6%   $	 100 67    6C   �� *
Dest_mac+  58      $ 	Hast name��6%   $	 100 68    6D   �� *	Host_name+4  .System.Windows.Controls.DataGridTemplateColumn  59      $ Select��6)      CellTemplate 5:      �� 5;      .R�5<      ��   CustomCheckBox#��[ 6'     d� 	IsChecked  6P   �� *
IsSelected+5=      5?      5A      5B      5C      