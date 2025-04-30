using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TextBoxDemo.Pages
{
    /// <summary>
    /// UndoRedoPage.xaml 的交互逻辑
    /// </summary>
    public partial class UndoRedoPage : Page
    {
        private ObservableCollection<string> _undoHistory;
        private ObservableCollection<string> _redoHistory;
        private int _changeCount = 0;

        public UndoRedoPage()
        {
            InitializeComponent();
            
            _undoHistory = new ObservableCollection<string>();
            _redoHistory = new ObservableCollection<string>();
            
            UndoHistoryListBox.ItemsSource = _undoHistory;
            RedoHistoryListBox.ItemsSource = _redoHistory;
        }

        #region 基本撤销重做操作

        private void UndoBasic_Click(object sender, RoutedEventArgs e)
        {
            if (BasicUndoRedoTextBox.CanUndo)
            {
                BasicUndoRedoTextBox.Undo();
                BasicOperationStatus.Text = "已撤销上一步操作";
            }
            else
            {
                BasicOperationStatus.Text = "没有可撤销的操作";
            }
        }

        private void RedoBasic_Click(object sender, RoutedEventArgs e)
        {
            if (BasicUndoRedoTextBox.CanRedo)
            {
                BasicUndoRedoTextBox.Redo();
                BasicOperationStatus.Text = "已重做上一步操作";
            }
            else
            {
                BasicOperationStatus.Text = "没有可重做的操作";
            }
        }

        private void ClearBasic_Click(object sender, RoutedEventArgs e)
        {
            BasicUndoRedoTextBox.Clear();
            BasicOperationStatus.Text = "文本已清空";
        }

        #endregion

        #region 控制撤销重做功能

        private void IsUndoEnabledCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = IsUndoEnabledCheckBox.IsChecked ?? false;
            UndoEnabledTextBox.IsUndoEnabled = isChecked;
            UndoEnabledTextBox.Text = isChecked ? 
                "此文本框启用了撤销/重做功能" : 
                "此文本框禁用了撤销/重做功能";
        }

        #endregion

        #region 撤销和重做的编程控制

        private void ProgrammaticUndoRedoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUndoRedoStatus();
        }

        private void UpdateUndoRedoStatus()
        {
            // 添加null检查
            if (ProgrammaticUndoRedoTextBox == null || UndoButton == null || 
                RedoButton == null || UndoStatusTextBlock == null || RedoStatusTextBlock == null)
                return;
                
            // 更新撤销按钮状态
            UndoButton.IsEnabled = ProgrammaticUndoRedoTextBox.CanUndo;
            UndoStatusTextBlock.Text = ProgrammaticUndoRedoTextBox.CanUndo ? 
                "可以撤销" : "没有可撤销的操作";

            // 更新重做按钮状态
            RedoButton.IsEnabled = ProgrammaticUndoRedoTextBox.CanRedo;
            RedoStatusTextBlock.Text = ProgrammaticUndoRedoTextBox.CanRedo ? 
                "可以重做" : "没有可重做的操作";
        }

        private void UndoProgrammatic_Click(object sender, RoutedEventArgs e)
        {
            if (ProgrammaticUndoRedoTextBox.CanUndo)
            {
                ProgrammaticUndoRedoTextBox.Undo();
                UpdateUndoRedoStatus();
            }
        }

        private void RedoProgrammatic_Click(object sender, RoutedEventArgs e)
        {
            if (ProgrammaticUndoRedoTextBox.CanRedo)
            {
                ProgrammaticUndoRedoTextBox.Redo();
                UpdateUndoRedoStatus();
            }
        }

        private void UndoLimitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProgrammaticUndoRedoTextBox != null && UndoLimitComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)UndoLimitComboBox.SelectedItem;
                if (int.TryParse(selectedItem.Content.ToString(), out int undoLimit))
                {
                    ProgrammaticUndoRedoTextBox.UndoLimit = undoLimit;
                    UpdateUndoRedoStatus();
                }
            }
        }

        #endregion

        #region 撤销单元（UndoAction）

        private void InsertDateTime_Click(object sender, RoutedEventArgs e)
        {
            // 普通方式插入日期和时间（会被记录为多个独立的撤销操作）
            UndoActionTextBox.AppendText($"当前日期: {DateTime.Now.ToShortDateString()}\r\n");
            UndoActionTextBox.AppendText($"当前时间: {DateTime.Now.ToLongTimeString()}\r\n");
            UndoActionTextBox.AppendText("-------------------\r\n");
        }

        private void InsertDateTimeAsUndoAction_Click(object sender, RoutedEventArgs e)
        {
            // 将多个操作合并为一个撤销单元
            UndoActionTextBox.BeginChange();
            try
            {
                UndoActionTextBox.AppendText($"当前日期: {DateTime.Now.ToShortDateString()}\r\n");
                UndoActionTextBox.AppendText($"当前时间: {DateTime.Now.ToLongTimeString()}\r\n");
                UndoActionTextBox.AppendText("-------------------\r\n");
            }
            finally
            {
                UndoActionTextBox.EndChange();
            }
        }

        #endregion

        #region 实际应用示例

        private void EditorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 添加null检查
            if (EditorTextBox == null || _undoHistory == null || _redoHistory == null)
                return;
                
            UpdateEditorUndoRedoStatus();
            
            // 更新操作计数
            _changeCount++;
            
            // 更新撤销历史
            if (EditorTextBox.CanUndo)
            {
                if (_undoHistory.Count == 0 || _undoHistory[0] != $"操作 {_changeCount}")
                {
                    _undoHistory.Insert(0, $"操作 {_changeCount}");
                }
            }
            
            // 清空重做历史（因为新的编辑操作会清除重做栈）
            if (e.Changes.Count > 0 && !EditorTextBox.CanRedo)
            {
                _redoHistory.Clear();
            }
        }

        private void UpdateEditorUndoRedoStatus()
        {
            // 添加null检查
            if (EditorTextBox == null || EditorUndoButton == null || 
                EditorRedoButton == null || EditorStatusTextBlock == null)
                return;
                
            EditorUndoButton.IsEnabled = EditorTextBox.CanUndo;
            EditorRedoButton.IsEnabled = EditorTextBox.CanRedo;
            
            EditorStatusTextBlock.Text = string.Format("状态: 可撤销:{0} 可重做:{1}", 
                EditorTextBox.CanUndo ? "是" : "否", 
                EditorTextBox.CanRedo ? "是" : "否");
        }

        private void EditorUndo_Click(object sender, RoutedEventArgs e)
        {
            // 添加null检查
            if (EditorTextBox == null || _undoHistory == null || _redoHistory == null)
                return;
                
            if (EditorTextBox.CanUndo)
            {
                EditorTextBox.Undo();
                
                // 将撤销的操作添加到重做历史
                if (_undoHistory.Count > 0)
                {
                    string item = _undoHistory[0];
                    _undoHistory.RemoveAt(0);
                    _redoHistory.Insert(0, item);
                }
                
                UpdateEditorUndoRedoStatus();
            }
        }

        private void EditorRedo_Click(object sender, RoutedEventArgs e)
        {
            // 添加null检查
            if (EditorTextBox == null || _undoHistory == null || _redoHistory == null)
                return;
                
            if (EditorTextBox.CanRedo)
            {
                EditorTextBox.Redo();
                
                // 将重做的操作添加回撤销历史
                if (_redoHistory.Count > 0)
                {
                    string item = _redoHistory[0];
                    _redoHistory.RemoveAt(0);
                    _undoHistory.Insert(0, item);
                }
                
                UpdateEditorUndoRedoStatus();
            }
        }

        private void EditorClear_Click(object sender, RoutedEventArgs e)
        {
            // 添加null检查
            if (EditorTextBox == null)
                return;
                
            EditorTextBox.Clear();
            UpdateEditorUndoRedoStatus();
        }

        private void EditorInsertTime_Click(object sender, RoutedEventArgs e)
        {
            // 添加null检查
            if (EditorTextBox == null || EditorStatusTextBlock == null)
                return;
                
            // 使用BeginChange和EndChange来确保插入时间作为一个单一的撤销操作
            EditorTextBox.BeginChange();
            try
            {
                EditorTextBox.AppendText($"\r\n[{DateTime.Now}]\r\n");
            }
            finally
            {
                EditorTextBox.EndChange();
            }
            
            EditorStatusTextBlock.Text = "已插入当前时间";
            UpdateEditorUndoRedoStatus();
        }

        #endregion
    }
} 