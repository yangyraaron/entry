   M S B A M L   `   `   `  ����W  SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   NavigationTreeViewItemStyle  �� 
TargetType �� Property  BackgroundBrush �� Value  	FontBrush  FontSize  
FontFamily �� RelativeSource �� AncestorType �� 
TargetType � IsSharedSizeScope � ColumnDefinitions z� SharedSizeGroup	 � RowDefinitions
 �Name ��Name  BorderBrush  BrightGlassBrush �� Orientation d�Name �� 	ClickMode  TreeViewToggleButtonStyle d� 	IsChecked ��Name �� Padding J� SnapsToDevicePixels ��Name ��Name �� Triggers P� 
SourceName P� Property P� Value �� 
TargetName ]� 
Conditions �� Property �� Value  DisabledFontBrush �� Triggers�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      8d2http://schemas.microsoft.com/expression/blend/2008  69   Bmc;http://schemas.openxmlformats.org/markup-compatibility/2006  6v   %�
  &	        �� 5      "  Q�60   .�5   
   ��  � ��# �  6'   �� 5   
    � ��# �  6'   �� 5   
    � ��# �  6%   �� 5   
    � ��# �  6'   �� 5	   
    � �� 67   �� *HorizontalContentAlignment+ �� " ���� 5
   
    � �� 65   �� *VerticalContentAlignment+ �� " ���� 5   
    � �� 5      �� 5      " Q�6"   .R�5      �  . .+�5      �  5      �� 5      $
��Auto �6'   $ Width��64   	 5      �� 5      $
��Auto �6$   $ Header��62   �� 5   "   $
 Body��60   .+�5      � $
 HeaderContainer��6   $y�	Collapsed=�63   	 5      �� 5      $
��0.5* �6%   �� 5      $
��0.5* �6%   .+�5      �� $ OuterBorder��6    $��2��5      #���  5      $	��0.5��5      �� 5       $ GlassBorder��6    #���  5!      5"      5#      � 5%      $
 Nodes��6    5&      �� 5'      $
��Auto �6'   $ Width��64   	 5)      �� 5*      $
��Auto �6$   $ Header��62   �� 5+      $
 Body��6$   .+�5.      �� $��2��6    $��5,1,5,0q�5/      $ 
Horizontal=�50      $	��Top=�6.   .+�51      d� $ Expander��6#   $��0,0,2,0q�52      $ Press=�54      #���  56      ��53      Z�  55      �� *
IsExpanded+ �� *TemplatedParent+56      �� 57      $ Bd��6   # z��58       . ._�5:      �� $ PART_Header��6(   #��z��5;      #��z��5<      $��Header��5=      # z 5>      5?      5@      �� 5A      $ 	ItemsHost��6$   $��1��5B      $��2��5C      $��10,0,0,0q�5D      5E      5G       5H      P� 5I      $ Bd��6"    �@J�IsMouseOver . . �5J      �� $ HeaderContainer��6%    � y�$ Visible=�6X   5M      P� 5N       �@Q�
IsSelected . . �5O      �� $ OuterBorder��6%    � y�$ Visible=�6T   �� 5P      $ OuterBorder��6%   # �  6N    � ��5Q      P� 5R       �@Q�
IsExpanded .  . �5S      �� $ 	ItemsHost��6%    � y�$ 	Collapsed=�6R   5T      P� 5U       �@��HasItems .  . �5V      �� $ Expander��6%    � y�$ 	Collapsed=�6Q   5W      ]� 5Y       5Z      �� 5[   "    �@Q�
IsSelected . 6B   �� 5\   "    �@Q�IsSelectionActive .  6I   ./�5^      �� # �  6;    � ���� 5_      $ Bd��6%   # �  6K    � ��5`      P� 5a       � {� .  . �5b      �� # �  6;    � ��5c      5f      5h      �� 5i   
    � ��# �  6(   �� 5j   
    � ��$ 1q�6,   �� 5k   
    � ��$	 5,1q�6#    5l   
   P� 5m       �@$�IsVirtualizing . . �5n      ��  � �� 5o      �� 5p      .R�5q      $� 5r      5t      5u      5w      5y      