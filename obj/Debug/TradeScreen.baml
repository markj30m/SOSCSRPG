   M S B A M L   `   `   `  ����@  <WPFUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPFUIWPFUI  5   	       WPFUI.TradeScreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:WPFUI     5   	     � WindowStartupLocation$  CenterOwner=�5   	   $
��11pt(�5	   	    � Title$ Trade Screen��5
   	   $	��480��6   $	��640��6+   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6   $��2��5      $��Center=�5      ��5      �� *CurrentTrader.Name+�� 5   
   $��1��6   $��0��6   $��Center=�5      $��Your Inventory��5       �� 5!   
   $��1��6   $��1��6   $��Center=�5"      $��Trader's Inventory��5#      &   System.Windows.Controls.DataGrid  5%   
   $��2��6   $��0��6    $��10q�5&         AutoGenerateColumns .     HeadersVisibility$ Column=�5)         CanUserAddRows .     CanUserDeleteRows .  "   VerticalScrollBarVisibility$
 Auto=�5,      ��5'      �� *CurrentPlayer.Inventory+	   Columns	 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Description��6%      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ * 51   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 52   %   ��  �� Path$
 Name�  53      $
 Price��6%    . $
 Auto 55   %    56   %   �� $ Price�4  .System.Windows.Controls.DataGridTemplateColumn  57         MinWidth$ 75Q�6)      CellTemplate 58      �� 59      .R�5:      �� -   5        $��55��5;   %   $
��Sell��5<   %   5=      5?      5B        5D   
   $��2��6   $��1��6    $��10q�5E       .  $ Column=�5H       .   .  $
 Auto=�5K      ��5F      �� *CurrentTrader.Inventory+	 5M        5N      $
 Description��6%    . $ * 5P   %    5Q   %   �� $
 Name�  5R      $
 Price��6%    . $
 Auto 5T   %    5U   %   �� $ Price�  5V      $ 75Q�6)    5W      �� 5X      .R�5Y      �� -   5        $��55��5Z   %   $	��Buy��5[   %   5\      5^      5a      �� 5c   
   -   5        $��3��5c      $��1��6   $��Right=�5d      $��75��5e      $��Close��5f      5g      5h      5i      