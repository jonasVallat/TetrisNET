   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.login   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  login��5   	   $	��250��6   $	��400��6$   .��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ 	error_msg��5      $��0��6'   $��0��64   $��2��6D    �� Padding$ 10q�6X    �� TextAlignment$ Center=�6e   
���  ��$y�Hidden=�6�   Error : blabla6�   �� 5   
   $��Email :��6   $��1��6"   $��0��6/   $
��Left=�6?   $��Center=�6Z   �� 5   
   $��
Password :��6   $��0��6%   $��2��65   $
��Left=�6B   $��Center=�6]   �� 5   
   -   5         ��Name$ email��5      $��1��6!   $��1��61   $��20 0 0 0q�6>   6R   G� 5   
   -   5         G�Name$ password��5      $��1��6(   $��2��68   $��20 0 0 0q�6E   6Y   �� 5   
   -   5         ��Name$ register_btn��5      $��1��6'   $��3��67   $��Register��6D   $��5q�6W   $��
20 20 0 20q�6c   $
��left=�6x   6�   �� 5   
   -   5        $ 	login_btn��5      $��1��6$   $��3��64   $��Log in��6A   $��5q�6R   $
��0 20q�6^   $��Right=�6m   6�   5      5      