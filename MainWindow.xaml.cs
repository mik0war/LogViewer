using Microsoft.Win32; // Для открытия файлового диалога
using System.IO;
using System.Windows;

namespace LogViewer
{
    public partial class MainWindow : Window
    {
        private string _logContent; // Исходное содержимое файла

        public MainWindow()
        {
            InitializeComponent();
            _logContent = "";
        }

        // Обработчик кнопки "Открыть лог"
        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Log files (*.log;*.txt)|*.log;*.txt|All files (*.*)|*.*",
                Title = "Открыть файл логов"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _logContent = File.ReadAllText(openFileDialog.FileName); // Читаем содержимое файла
                    LogTextBox.Text = _logContent; // Отображаем в TextBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Обработчик кнопки "Применить фильтр"
        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_logContent))
            {
                MessageBox.Show("Файл не загружен. Откройте файл логов.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string filter = FilterTextBox.Text;
            if (string.IsNullOrEmpty(filter))
            {
                MessageBox.Show("Введите фильтр для поиска.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filteredLogs = _logContent
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Where(line => line.Contains(filter, StringComparison.OrdinalIgnoreCase)) // Игнорируем регистр
                .ToArray();

            if (filteredLogs.Length > 0)
            {
                LogTextBox.Text = string.Join(Environment.NewLine, filteredLogs);
            }
            else
            {
                LogTextBox.Text = "Результаты не найдены.";
            }
        }

        // Обработчик кнопки "Сбросить фильтр"
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_logContent))
            {
                LogTextBox.Text = _logContent;
                FilterTextBox.Clear(); // Очищаем поле ввода фильтра
            }
        }
    }
}
