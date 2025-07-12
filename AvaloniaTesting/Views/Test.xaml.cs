using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.Win32;
using static AvaloniaTestingProgramm.Entities;
using static AvaloniaTestingProgramm.Controller.Checks;

namespace AvaloniaTestingProgramm
{
    public partial class Test : Window
    {
        private TestsDatabase currentTest;
        private int _currentQuestionIndex = 0;
        private int _correctAnswers = 0;
        private int _incorrectAnswers = 0;
        private Dictionary<int, int> _correctAnswerPositions = new Dictionary<int, int>();

        public Test()
        {
            InitializeComponent();
            LoadTest();
        }

        private void LoadTest()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Tests"),
                Filter = "JSON files (*.json)|*.json",
                Title = "Выберите тест для прохождения"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    currentTest = JsonDataService.LoadData(openFileDialog.FileName);
                    if (!ValidateTestContent(currentTest.BaseQuestions.Count))
                    {
                        Close();
                        return;
                    }

                    TestName.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    TestName.IsEnabled = false;
                    ShowQuestion(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки теста: {ex.Message}");
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void ShowQuestion(int index)
        {
            if (!ValidateQuestionIndex(index, currentTest.BaseQuestions.Count))
                return;

            var question = currentTest.BaseQuestions[index];
            Question.Text = question.Qouestion;
            Question.IsEnabled = false;

            var answers = new List<string>
            {
                question.CorrectAnser,
                question.IncorrectAnswer1,
                question.IncorrectAnswer2 ?? string.Empty,
                question.IncorrectAnswer3 ?? string.Empty
            }.Where(a => !string.IsNullOrEmpty(a)).OrderBy(a => Guid.NewGuid()).ToList();

            Answer1.Text = answers[0];
            Answer2.Text = answers[1];
            Answer3.Text = answers.Count > 2 ? answers[2] : string.Empty;
            Answer4.Text = answers.Count > 3 ? answers[3] : string.Empty;

            _correctAnswerPositions[index] = answers.IndexOf(question.CorrectAnser);

            Answer1cb.IsChecked = false;
            Answer2cb.IsChecked = false;
            Answer3cb.IsChecked = false;
            Answer4cb.IsChecked = false;

            Answer3.IsEnabled = Answer3cb.IsEnabled = answers.Count > 2;
            Answer4.IsEnabled = Answer4cb.IsEnabled = answers.Count > 3;

            _currentQuestionIndex = index;
        }

        private void CheckAnswer()
        {
            int selectedAnswer = -1;
            if (Answer1cb.IsChecked == true) selectedAnswer = 0;
            else if (Answer2cb.IsChecked == true) selectedAnswer = 1;
            else if (Answer3cb.IsChecked == true && Answer3.IsEnabled) selectedAnswer = 2;
            else if (Answer4cb.IsChecked == true && Answer4.IsEnabled) selectedAnswer = 3;

            if (!ValidateAnswerSelection(selectedAnswer)) return;

            if (selectedAnswer == _correctAnswerPositions[_currentQuestionIndex])
                _correctAnswers++;
            else
                _incorrectAnswers++;

            if (_currentQuestionIndex < currentTest.BaseQuestions.Count - 1)
            {
                ShowQuestion(_currentQuestionIndex + 1);
            }
            else
            {
                FinishTest();
            }
        }

        private void FinishTest()
        {
            var results = JsonDataService.LoadData();
            results.Results.Add(new Results
            {
                ResultID = results.Results.Count + 1,
                TestName = TestName.Text,
                Correct = _correctAnswers,
                Incorrect = _incorrectAnswers
            });
            JsonDataService.SaveData(results);

            double percentage = (double)_correctAnswers / (_correctAnswers + _incorrectAnswers) * 100;
            string resultMessage = percentage >= 50 ? "Успешно!" : "Провал!";

            MessageBox.Show($"Тест завершен!\nПравильных ответов: {_correctAnswers}\n" +
                          $"Неправильных ответов: {_incorrectAnswers}\nРезультат: {resultMessage}");

            Close();
        }

        private void createNextQuestioBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
        }

        private void saveAndExitTestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти? Результаты не будут сохранены.",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        // Обработчики для чекбоксов
        private void Answer1cb_Checked(object sender, RoutedEventArgs e)
        {
            UncheckOtherCheckBoxes(Answer1cb);
        }

        private void Answer2cb_Checked(object sender, RoutedEventArgs e)
        {
            UncheckOtherCheckBoxes(Answer2cb);
        }

        private void Answer3cb_Checked(object sender, RoutedEventArgs e)
        {
            UncheckOtherCheckBoxes(Answer3cb);
        }

        private void Answer4cb_Checked(object sender, RoutedEventArgs e)
        {
            UncheckOtherCheckBoxes(Answer4cb);
        }

        private void UncheckOtherCheckBoxes(CheckBox currentCheckBox)
        {
            var checkBoxes = new[] { Answer1cb, Answer2cb, Answer3cb, Answer4cb };
            foreach (var cb in checkBoxes.Where(cb => cb != currentCheckBox))
            {
                cb.IsChecked = false;
            }
        }
    }
}