using System.Windows;

namespace AvaloniaTestingProgramm.Controller
{
    public static class Checks
    {
        //Проверка на заполнение минимально необходимых полей
        public static bool ValidateInputs(string correct, string incorrect)
        {
            if (string.IsNullOrWhiteSpace(correct) || string.IsNullOrWhiteSpace(incorrect))
            {
                ShowError("Заполните Правильный и хотя бы 1 неправильный ответ!");
                return false;
            }
            return true;
        }
        //Проверка на заполнение названия
        public static bool ValidateQuestionInputs(string correct, string incorrect, string testName, bool isNameLocked)
        {
            if (!isNameLocked && string.IsNullOrWhiteSpace(testName))
            {
                ShowError("Введите название теста!");
                return false;
            }
            return ValidateInputs(correct, incorrect);
        }
      
        //Проверка не пвытается ли пользователь открыть пустой джсон тест
        public static bool ValidateTestContent(int questionsCount)
        {
            if (questionsCount == 0)
            {
                ShowError("Выбранный тест пуст!");
                return false;
            }
            return true;
        }

        public static bool ValidateQuestionIndex(int index, int questionsCount)
        {
            return index >= 0 && index < questionsCount;
        }
        //Проверка что пользователь выбрал хотя бы один ответ
        public static bool ValidateAnswerSelection(int selectedAnswer)
        {
            if (selectedAnswer == -1)
            {
                ShowError("Выберите хотя бы один ответ!");
                return false;
            }
            return true;
        }
        //Ошибка
        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}